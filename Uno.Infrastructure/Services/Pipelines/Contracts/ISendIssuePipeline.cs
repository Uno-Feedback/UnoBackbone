using IssueStatus = Uno.Domain.Enums.IssueStatus;

namespace Uno.Infrastructure.ExternalServices.Services.Pipelines;

public interface ISendIssuePipeline
{
    public void SetNextHandler(ISendIssuePipeline handler);

    public Task<Response<IssueStatus>> Handle(SendIssueDto issueDto, CancellationToken cancellationToken);
}