namespace Uno.Application.Services;

public record GetProjecttListQuery(Guid? userId, Guid? projectId) : IRequest<Response<object>>;
