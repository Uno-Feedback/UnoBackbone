
namespace Uno.Application.Services;

#nullable disable
public record SendIssueCommand : IRequest<Response<object>>
{
    public Guid ConnectorId { get; set; }
    public Guid IssueId { get; set; }
}
