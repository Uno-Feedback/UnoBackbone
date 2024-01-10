using Newtonsoft.Json.Linq;
using Uno.Application.Resources;
using Uno.Shared.Extentions;
using IssueStatus = Uno.Domain.Enums.IssueStatus;

namespace Uno.Infrastructure.ExternalServices.Services.Pipelines;

public class JiraUploadAttachmentHandler : IJiraUploadAttachmentHandler
{
    private ISendIssuePipeline _nextHandler;

    public void SetNextHandler(ISendIssuePipeline handler)
        => _nextHandler = handler;

    public async Task<Response<IssueStatus>> Handle(SendIssueDto issueDto, CancellationToken cancellationToken)
    {
        if (issueDto.IssueMetaData is null)
            return Response<IssueStatus>.Error(IssueStatus.ReadyForSend);

        var jiraClient = Jira
            .CreateRestClient(issueDto.Connector.Url, issueDto.Connector.UserName, issueDto.Connector.Password);
        
        try
        {
            var createdIssue = await jiraClient.Issues.GetIssueAsync(issueDto.IssueMetaData, cancellationToken);
            if (createdIssue is null)
                return Response<IssueStatus>.Error(issueDto.Status, ServiceMessages.IssueNotFound);

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
            issueDto.Status = IssueStatus.Finished;
        }
        catch (Exception ex)
        {
            return Response<IssueStatus>.Error(IssueStatus.SendWithoutAttachment, ex.Message);
        }

        return _nextHandler is not null
            ? await _nextHandler.Handle(issueDto, cancellationToken)
            : Response<IssueStatus>.Success(IssueStatus.Finished);
    }
}