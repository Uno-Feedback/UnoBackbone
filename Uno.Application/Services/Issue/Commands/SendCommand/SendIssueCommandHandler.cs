namespace Uno.Application.Services;

#nullable disable
public class SendIssueCommandHandler : IRequestHandler<SendIssueCommand, Response<object>>
{

    private readonly IDbContext _dbContext;
    private readonly IClientAdapterFactory _clientAdapterFactory;

    public SendIssueCommandHandler(IDbContext dbContext, IClientAdapterFactory clientAdapterFactory)
    {
        _dbContext = dbContext;
        _clientAdapterFactory = clientAdapterFactory;
    }

    public async Task<Response<object>> Handle(SendIssueCommand request, CancellationToken cancellationToken)
    {
        var connectorInIssueDto = await _dbContext.Set<ConnectorInIssue>()
            .Where(x => x.IssueId == request.IssueId && x.ConnectorId == request.ConnectorId)
            .Select(x => new SendIssueDto()
            {
                ConnectorInIssueId = x.Id,
                ConnectorMetaData = x.ConnectorMetaData,
                ClientMetaData = x.ClientMetaData,
                IssueMetaData = x.IssueMetaData,
                Status = x.Status,
                Issue = new IssueDto()
                {
                    Id = x.IssueId,
                    Subject = x.Issue.Subject,
                    Description = x.Issue.Description,
                    GeneratedDescription = x.Issue.GenerateDescription(),
                    Attachments = x.Issue.Attachments.Select(x => new IssueAttachmentDto { Id = x.Id, Name = x.Name, Content = x.Content}).ToList(),
                },
                Connector = new ConnectorDto()
                {
                    Id = x.ConnectorId,
                    Url = x.Connector.Url,
                    UserName = x.Connector.UserName,
                    Password = x.Connector.Password,
                    Key = x.Connector.Key,
                    Type = x.Connector.Type
                },
            })
            .FirstOrDefaultAsync();

        var clientAdapterResponse = await _clientAdapterFactory.GetInstance(connectorInIssueDto.Connector.Type).SendIssue(connectorInIssueDto, cancellationToken);

        var connectorInIssue = await _dbContext.Set<ConnectorInIssue>().FirstOrDefaultAsync(x => x.ConnectorId == request.ConnectorId && x.IssueId == request.IssueId, cancellationToken);
        connectorInIssue.Status = clientAdapterResponse.IsSuccess ? clientAdapterResponse.Result : IssueStatus.ReadyForSend;

        if(connectorInIssue.Status == IssueStatus.Finished)
        {
            foreach (var attachment in connectorInIssueDto.Issue.Attachments)
            {
                var uploadedAttachment = await _dbContext.Set<IssueAttachment>().FindAsync(attachment.Id, cancellationToken);
                uploadedAttachment.Content = string.Empty;
            }
        }

        var saveChangeResponse = await _dbContext.SaveChangeResposeAsync(cancellationToken);

        return saveChangeResponse.IsSuccess ?
            Response<object>.Success(clientAdapterResponse.Message) :
            Response<object>.Error(saveChangeResponse.Message);
    }
}
