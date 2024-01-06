using Uno.Domain.Common;
using Uno.Domain.Enums;
using Uno.Shared.Extentions;

namespace Uno.Domain.Entities;

[Entity]
public class Connector : IEntity
{
    public Guid Id { get; set; }
    public Project Project { get; set; }
    public Guid ProjectId { get; set; }
    public string? Key { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Url { get; set; }
    public ConnectorTypes Type { get; set; } = ConnectorTypes.Jira;
    public ICollection<ConnectorInIssue> ConnectorInIssues { get; set; } = new List<ConnectorInIssue>();
    public ICollection<ConnectorReportPriorities> ConnectorReportPriorities { get; set; } = new List<ConnectorReportPriorities>();
    public ICollection<ConnectorReportTypes> ConnectorReportTypes { get; set; } = new List<ConnectorReportTypes>();

    public string TypeDescription => Type.GetDescription();
}