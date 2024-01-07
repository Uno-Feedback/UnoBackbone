namespace Uno.Application.Services;

#nullable disable
public class GetConnectorMetaDataQueryHandler : IRequestHandler<GetConnectorMetaDataQuery, Response<object>>
{
    private readonly IDbContext _dbContext;
    private readonly IClientAdapterFactory _clientAdapterFactory;
    public GetConnectorMetaDataQueryHandler(IDbContext dbContext, IClientAdapterFactory clientAdapterFactory)
    {
        _dbContext = dbContext;
        _clientAdapterFactory = clientAdapterFactory;
    }


    public async Task<Response<object>> Handle(GetConnectorMetaDataQuery request, CancellationToken cancellationToken)
    {
        var connector = await _dbContext.Set<Connector>().FindAsync(request.ConnectorId, cancellationToken);

        return await _clientAdapterFactory.GetInstance(connector.Type).GetMetaData(new ConnectorDto
        {
            Url = connector.Url,
            UserName = connector.UserName,
            Password = connector.Password,
        }, cancellationToken);
    }
}
