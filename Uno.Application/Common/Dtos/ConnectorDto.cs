using Uno.Domain.Enums;

namespace Uno.Application.Common.Dtos;

public record ConnectorDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Url { get; set; }
    public ConnectorTypes Type { get; set; }
    public string Key { get; set; }
}
