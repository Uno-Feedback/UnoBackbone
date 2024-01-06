namespace Uno.Application.Services;

public record GetConnectorMetaDataQuery(Guid ConnectorId) : IRequest<Response<object>>;
