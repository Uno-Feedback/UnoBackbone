using Uno.Domain.Common;

namespace Uno.Domain.Entities;

[Entity]
public class Project : IEntity
{
    public Guid Id { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string IP { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime InsertTime { get; set; } = DateTime.Now;
    public ICollection<Issue> Issues { get; set; } = new List<Issue>();
    public ICollection<Connector> Connectors { get; set; } = new List<Connector>();

}
