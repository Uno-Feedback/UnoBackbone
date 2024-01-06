using Uno.Domain.Common;

namespace Uno.Domain.Entities;

[Entity]
public class ConnectorReportPriorities : IEntity
{
    public Guid Id { get; set; }
    public Guid ConnectorId { get; set; }
    public Connector Connector { get; set; }
    public string Name { get; set; }
    public string Key { get; set; }
}
