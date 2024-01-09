using Microsoft.Extensions.Options;
using Uno.Application.Settings;


namespace Uno.Application.Services;

#nullable disable
public class SendIssueCommandHandler : IRequestHandler<SendIssueCommand, Response<object>>
{
    private readonly IDbContext _dbContext;
    private readonly IClientAdapterFactory _clientAdapterFactory;
    private readonly SendIssueConfig _sendIssueJobConfig;
    private static object _lock = new object();

    public SendIssueCommandHandler(IDbContext dbContext, IClientAdapterFactory clientAdapterFactory,
        IOptionsMonitor<SendIssueConfig> optionsMonitor)
    {
        _dbContext = dbContext;
        _clientAdapterFactory = clientAdapterFactory;
        _sendIssueJobConfig = optionsMonitor.CurrentValue;
    }

    private Response<IList<ConnectorInIssue>> ListConnectorInIssues()
    {
        lock (_lock)
        {
            var connectorInIssues = _dbContext.Set<ConnectorInIssue>()
                .Where(x =>
                    (x.Status == IssueStatus.ReadyForSend &&
                     x.TryCount <= _sendIssueJobConfig.CreateIssueTryCountAmount) ||
                    (x.Status == IssueStatus.SendWithoutAttachment &&
                     x.TryCount <= _sendIssueJobConfig.UploadAttachmentTryCountAmount))
                .Include(x => x.Connector)
                .Include(x => x.Issue)
                .ToList();

            if (connectorInIssues.Count is 0)
                return Response<IList<ConnectorInIssue>>.Success(connectorInIssues);

            connectorInIssues.ForEach(x =>
            {
                x.TryCount++;
                x.Status = IssueStatus.ReadyForSend;
            });

            var saveChangeResponse = _dbContext.SaveChangeResponse();

            return saveChangeResponse.IsSuccess
                ? Response<IList<ConnectorInIssue>>.Success(connectorInIssues)
                : Response<IList<ConnectorInIssue>>.Error(saveChangeResponse.Message);
        }
    }

    public async Task<Response<object>> Handle(SendIssueCommand request, CancellationToken cancellationToken)
    {
        var listConnectorInIssuesResponse = ListConnectorInIssues();
        if (listConnectorInIssuesResponse.IsFailure || listConnectorInIssuesResponse.Result.Count is 0)
            return Response<object>.Error(ServiceMessages.ServerError);

        var connectorInIssues = listConnectorInIssuesResponse.Result;
        foreach (var connectorInIssue in connectorInIssues)
        {
            var sendIssueServiceResponse = await _clientAdapterFactory
                .GetInstance(connectorInIssue.Connector.Type)
                .SendIssue(
                    #region Create sendIssueDto
                    new SendIssueDto()
                    {
                        ConnectorInIssueId = connectorInIssue.Id,
                        ConnectorMetaData = connectorInIssue.ConnectorMetaData,
                        ClientMetaData = connectorInIssue.ClientMetaData,
                        IssueMetaData = connectorInIssue.IssueMetaData,
                        Status = connectorInIssue.Status,
                        Issue = new IssueDto()
                        {
                            Id = connectorInIssue.IssueId,
                            Subject = connectorInIssue.Issue.Subject,
                            Description = connectorInIssue.Issue.Description,
                            GeneratedDescription = connectorInIssue.Issue.GenerateDescription(),
                            Attachments = connectorInIssue.Issue.Attachments.Select(x =>
                                new IssueAttachmentDto
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Content = x.Content
                                }).ToList(),
                        },
                        Connector = new ConnectorDto()
                        {
                            Id = connectorInIssue.ConnectorId,
                            Url = connectorInIssue.Connector.Url,
                            UserName = connectorInIssue.Connector.UserName,
                            Password = connectorInIssue.Connector.Password,
                            Key = connectorInIssue.Connector.Key,
                            Type = connectorInIssue.Connector.Type
                        },
                    }

                    #endregion
                    , cancellationToken);

            connectorInIssue.Status = sendIssueServiceResponse.IsSuccess
                ? sendIssueServiceResponse.Result
                : IssueStatus.ReadyForSend;

            if (connectorInIssue.Status != IssueStatus.Finished)
                continue;

            foreach (var attachment in connectorInIssue.Issue.Attachments)
                attachment.Content = string.Empty;
        }

        var saveChangeResponse = await _dbContext.SaveChangeResposeAsync(cancellationToken);

        return saveChangeResponse.IsSuccess
            ? Response<object>.Success(default)
            : Response<object>.Error(saveChangeResponse.Message);
    }
}