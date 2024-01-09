namespace Uno.Application.Services;

#nullable disable
public class AddConnectorCommandHandler : IRequestHandler<AddConnectorCommand, Response<object>>
{
    private readonly IDbContext _dbContext;

    public AddConnectorCommandHandler(IDbContext dbContext)
        => _dbContext = dbContext;


    public async Task<Response<object>> Handle(AddConnectorCommand request, CancellationToken cancellationToken)
    {
        var newConnector = new Connector()
        {
            ProjectId = request.ProjectId,
            Url = request.Url,
            UserName = request.UserName,
            Password = request.Password,
            Type = request.Type,
            ConnectorReportPriorities = request.ConnectorReportPriorities
                .Select(x =>
                    new ConnectorReportPriorities
                    {
                        Name = x.Name,
                        Key = x.Key
                    })
                .ToList(),
            ConnectorReportTypes = request.ConnectorReportTypes
                .Select(x =>
                    new ConnectorReportTypes
                    {
                        Name = x.Name,
                        Key = x.Key
                    })
                .ToList()
        };

        _dbContext.Set<Connector>().Add(newConnector);
        var saveResponse = await _dbContext.SaveChangeResposeAsync(cancellationToken);

        return saveResponse.IsSuccess
            ? Response<object>.Success(newConnector.Id)
            : Response<object>.Error(saveResponse.Message);
    }
}