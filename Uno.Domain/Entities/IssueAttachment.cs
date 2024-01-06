using Uno.Domain.Common;
using Uno.Domain.Enums;

namespace Uno.Domain.Entities;

[Entity]
public class IssueAttachment : IEntity
{
    public int Id { get; set; }
    public Issue Issue { get; set; }
    public Guid IssueId { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public IssueAttachmentTypes Type { get; set; }
}