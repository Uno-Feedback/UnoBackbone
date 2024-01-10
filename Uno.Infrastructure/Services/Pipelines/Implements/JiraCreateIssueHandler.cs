using Newtonsoft.Json;
using Issue = Atlassian.Jira.Issue;
using IssueStatus = Uno.Domain.Enums.IssueStatus;

namespace Uno.Infrastructure.ExternalServices.Services.Pipelines;

public class JiraCreateIssueHandler : IJiraCreateIssueHandler
{
    private ISendIssuePipeline _nextHandler;
    private readonly IDbContext _dbContext;

    public JiraCreateIssueHandler(IDbContext dbContext)
        => _dbContext = dbContext;


    public void SetNextHandler(ISendIssuePipeline handler)
        => _nextHandler = handler;

    public async Task<Response<IssueStatus>> Handle(SendIssueDto issueDto, CancellationToken cancellationToken)
    {
        if (_nextHandler is not null && (issueDto.IssueMetaData is not null ||
                                         issueDto.Status is IssueStatus.SendWithoutAttachment))
            return await _nextHandler.Handle(issueDto, cancellationToken);


        var jiraClient = Jira
            .CreateRestClient(issueDto.Connector.Url, issueDto.Connector.UserName, issueDto.Connector.Password);
        try
        {
            var connectorMetadata = JsonConvert
                .DeserializeObject<JiraIssueConnectorMetaData>(issueDto.ConnectorMetaData);

            var newIssue = new Issue(jiraClient, issueDto.Connector.Key)
            {
                Summary = issueDto.Issue.Subject,
                Description = issueDto.Issue.GeneratedDescription,
                Type = connectorMetadata.IssueType,
                Priority = connectorMetadata.IssuePriority,
            };

            await newIssue.SaveChangesAsync(cancellationToken);
            issueDto.IssueMetaData = newIssue.Key.ToString();
            issueDto.Status = IssueStatus.SendWithoutAttachment;
        }
        catch (Exception ex)
        {
            return Response<IssueStatus>.Error(issueDto.Status, ex.Message);
        }

        var connectorInIssue = await _dbContext.Set<ConnectorInIssue>()
            .FindAsync(new object?[] { issueDto.ConnectorInIssueId }, cancellationToken);

        connectorInIssue.IssueMetaData = JsonConvert
            .SerializeObject(new JiraIssueMetadataDto(issueDto.IssueMetaData));

        var saveChangeResponse = await _dbContext.SaveChangeResposeAsync(cancellationToken);
        if (saveChangeResponse.IsFailure)
            return Response<IssueStatus>.Error(issueDto.Status, saveChangeResponse.Message);

        return _nextHandler is not null
            ? await _nextHandler.Handle(issueDto, cancellationToken)
            : Response<IssueStatus>.Success(IssueStatus.SendWithoutAttachment);
    }
}