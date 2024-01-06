using Uno.Domain.Common;

namespace Uno.Domain.Entities;

[Entity]
public class User : IEntity
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string CompanyName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime SingupDate { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}
