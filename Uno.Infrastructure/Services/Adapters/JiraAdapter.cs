using Newtonsoft.Json;
using Uno.Shared.Extentions;
using Issue = Atlassian.Jira.Issue;

namespace Uno.Infrastructure.ExternalServices.Services;

/// <summary>
/// Jira ClientAdapter.
/// </summary>
public class JiraAdapter : IClientAdapter
{
    private readonly IDbContext _dbContext;
    private readonly IJiraClientFactory _jiraClientFactory;

    public JiraAdapter(IDbContext dbContext, IJiraClientFactory jiraClientFactory)
    {
        _dbContext = dbContext;
        _jiraClientFactory = jiraClientFactory;
    }

    private async Task<Response<string>> CreateIssue(SendIssueDto issueDto,
        JiraIssueConnectorMetaData connectorMetaData, CancellationToken cancellationToken = default)
    {
        Issue newIssue;
        var jiraClient = Jira
            .CreateRestClient(issueDto.Connector.Url, issueDto.Connector.UserName, issueDto.Connector.Password);
        try
        {
            newIssue = new Issue(jiraClient, issueDto.Connector.Key)
            {
                Summary = issueDto.Issue.Subject,
                Description = issueDto.Issue.GeneratedDescription,
                Type = connectorMetaData.IssueType,
                Priority = connectorMetaData.IssuePriority,
            };

            await newIssue.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Response<string>.Error(ex.Message);
        }

        var connectorInIssue = await _dbContext.Set<ConnectorInIssue>()
            .FindAsync(issueDto.ConnectorInIssueId, cancellationToken);

        connectorInIssue.IssueMetaData =
            JsonConvert.SerializeObject(new JiraIssueMetadataDto(newIssue.Key.ToString()));

        var saveChangeResponse = await _dbContext.SaveChangeResposeAsync(cancellationToken);

        return saveChangeResponse.IsSuccess
            ? Response<string>.Success(newIssue.Key.ToString())
            : Response<string>.Error(saveChangeResponse.Message);
    }

    private async Task<Response> UploadAttachment(SendIssueDto issueDto, CancellationToken cancellationToken)
    {
        var jiraClient = _jiraClientFactory.GetJiraClient(new JiraConfig(issueDto.Connector.Url,
            issueDto.Connector.UserName, issueDto.Connector.Password));
        if (issueDto.Issue.Attachments.Any() is false)
            return Response.Error();

        try
        {
            var createdIssue = await jiraClient.Issues.GetIssueAsync(issueDto.IssueMetaData, cancellationToken);
            if (createdIssue is null)
                return Response.Error();

            foreach (var attachment in issueDto.Issue.Attachments)
            {
                var jiraAttachment =
                    new UploadAttachmentInfo($"{attachment.Name}", await attachment.Content.ConvertToByteArray());

                var jiraClientMetaData = new UploadAttachmentInfo(
                    $"{issueDto.Issue.Subject.Trim()}-{issueDto.IssueMetaData}.json",
                    issueDto.ClientMetaData.ConvertToJsonFileAsByteArray());

                await createdIssue.AddAttachmentAsync(
                    new UploadAttachmentInfo[]
                    {
                        jiraAttachment,
                        jiraClientMetaData
                    },
                    cancellationToken);
            }

            return Response.Success();
        }
        catch (Exception ex)
        {
            return Response.Error(ex.Message);
        }
    }


    public async Task<Response<object>> GetMetaData(ConnectorDto connectorDto, CancellationToken cancellationToken)
    {
        var jiraClient =
            _jiraClientFactory.GetJiraClient(new JiraConfig(connectorDto.Url, connectorDto.UserName,
                connectorDto.Password));
        try
        {
            var projects = await jiraClient.Projects.GetProjectsAsync(cancellationToken);
            var issuePriorities = await jiraClient.Priorities.GetPrioritiesAsync(cancellationToken);
            var issueTypes = await jiraClient.IssueTypes.GetIssueTypesAsync(cancellationToken);
            return Response<object>.Success(new
            {
                Projects = projects.Select(x => new { x.Id, x.Name, x.Key }),
                Priorities = issuePriorities.Select(x => new { x.Id, x.Name }),
                Types = issueTypes.Select(x => new { x.Id, x.Name })
            });
        }
        catch (Exception ex)
        {
            return Response<object>.Error(ex.Message);
        }
    }

    public async Task<Response<Domain.Enums.IssueStatus>> SendIssue(SendIssueDto issueDto,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(issueDto.IssueMetaData))
        {
            var connectorMetadata =
                JsonConvert.DeserializeObject<JiraIssueConnectorMetaData>(issueDto.ConnectorMetaData);
            var createIssueResponse =  await CreateIssue(issueDto, connectorMetadata, cancellationToken);

            return createIssueResponse.IsSuccess ?
                Response<Domain.Enums.IssueStatus>.Success(Domain.Enums.IssueStatus.SendWithoutAttachment) :
                Response<Domain.Enums.IssueStatus>.Error(createIssueResponse.Message);
        }
        else
        {
            issueDto.IssueMetaData =
                JsonConvert.DeserializeObject<JiraIssueMetadataDto>(issueDto.IssueMetaData).IssueKey;
            var uploadAttachmentResponse =  await UploadAttachment(issueDto, cancellationToken);

            return uploadAttachmentResponse.IsSuccess ?
                Response<Domain.Enums.IssueStatus>.Success(Domain.Enums.IssueStatus.Finished) :
                Response<Domain.Enums.IssueStatus>.Error(uploadAttachmentResponse.Message);
        }
    }
}