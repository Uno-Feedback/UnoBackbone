using Uno.Domain.Common;

namespace Uno.Domain.Entities;

[Entity]
public class Issue : IEntity
{
    public Guid Id { get; set; }
    public Project Project { get; set; }
    public Guid ProjectId { get; set; }
    public string Subject { get; set; }
    public string Description { get; set; }
    public string Reporter { get; set; }
    public string Url { get; set; }
    public DateTime InsertDateTime => DateTime.Now;
    public DateTime SentDateTime { get; set; }
    public ICollection<IssueAttachment> Attachments { get; set; } = new List<IssueAttachment>();
    public ICollection<ConnectorInIssue> ConnectorInIssues { get; set; } = new List<ConnectorInIssue>();

}
