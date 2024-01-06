namespace Uno.Application.Services;

public record AddProjectCommand : IRequest<Response<object>>
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string IP { get; set; }
}
