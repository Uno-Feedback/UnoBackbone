using Microsoft.AspNetCore.Http;

namespace Uno.Application.Services;

public record AddIssueCommand : IRequest<Response<object>>
{
    public Guid ProjectToken { get; set; }
    public Guid ConnectorId { get; set; }
    public string Subject { get; set; }
    public string Description { get; set; }
    public IFormFile File { get; set; }
    public string ReportUrl { get; set; }
    public string Reporter { get; set; }
    public string ConnectorMetaData { get; set; }
    public string ClientMetaData { get; set; }
    public IssueAttachmentTypes AttachmentType { get; set; }

}
