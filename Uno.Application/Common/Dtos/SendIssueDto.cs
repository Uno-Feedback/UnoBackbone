using Uno.Domain.Enums;

namespace Uno.Application.Common.Dtos;

public record SendIssueDto
{
    public int ConnectorInIssueId { get; set; }
    public IssueDto Issue { get; set; }
    public ConnectorDto Connector { get; set; }
    public IssueStatus Status { get; set; }
    public string ConnectorMetaData { get; set; }
    public string ClientMetaData { get; set; }
    public string IssueMetaData { get; set; }
}

public record IssueDto
{
    public Guid Id { get; set; }
    public string Subject { get; set; }
    public string Description { get; set; }
    public string GeneratedDescription { get; set; }
    public ICollection<IssueAttachmentDto> Attachments { get; set; }
}

public record IssueAttachmentDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
}