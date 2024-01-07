namespace Uno.Application.Services;

public record ListConnectorQuery(Guid ProjectId, ConnectorTypes? ConnectorType) : IRequest<Response<object>>;
