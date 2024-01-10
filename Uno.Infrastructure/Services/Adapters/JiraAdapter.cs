using Uno.Infrastructure.ExternalServices.Services.Pipelines;
using IssueStatus = Uno.Domain.Enums.IssueStatus;

namespace Uno.Infrastructure.ExternalServices.Services;

/// <summary>
/// Jira ClientAdapter.
/// </summary>
public class JiraAdapter : IClientAdapter
{
    private readonly ISendIssuePipelineBuilder _pipelineBuilder;
    private readonly IJiraCreateIssueHandler _createIssueHandler;
    private readonly IJiraUploadAttachmentHandler _uploadAttachmentHandler;
    private readonly IDbContext _dbContext;

    public JiraAdapter(ISendIssuePipelineBuilder pipelineBuilder, IJiraCreateIssueHandler createIssueHandler,
        IJiraUploadAttachmentHandler uploadAttachmentHandler, IDbContext dbContext)
    {
        _pipelineBuilder = pipelineBuilder;
        _createIssueHandler = createIssueHandler;
        _uploadAttachmentHandler = uploadAttachmentHandler;
        _dbContext = dbContext;
    }

    public async Task<Response<object>> GetMetaData(ConnectorDto connectorDto, CancellationToken cancellationToken)
    {
        var jiraClient = Jira
            .CreateRestClient(connectorDto.Url, connectorDto.UserName, connectorDto.Password);

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

    public async Task<Response<IssueStatus>> SendIssue(SendIssueDto issueDto,
        CancellationToken cancellationToken)
    {
        var sendIssuePipeline = _pipelineBuilder
            .AddHandler(_createIssueHandler)
            .AddHandler(_uploadAttachmentHandler)
            .Build();

        var pipelineResponse = await sendIssuePipeline.Handle(issueDto, cancellationToken); 
        
        var connectorInIssue = await _dbContext.Set<ConnectorInIssue>()
            .FindAsync(new object?[issueDto.ConnectorInIssueId], cancellationToken);

        connectorInIssue.Status = pipelineResponse.Result;
        
        var saveChangeResponse = await _dbContext.SaveChangeResposeAsync(cancellationToken);
        if (saveChangeResponse.IsFailure)
            return Response<IssueStatus>.Error(saveChangeResponse.Message); 
        
        
        return pipelineResponse; 
    }
}