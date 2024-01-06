using Uno.Application.Common;
using Uno.Shared.Common;

namespace Uno.Application.Services;

public class ListConnectorQueryHandler : IRequestHandler<ListConnectorQuery, Response<object>>
{
    private readonly IDbContext _dbContext;
    public ListConnectorQueryHandler(IDbContext dbContext)
        => _dbContext = dbContext;


    public async Task<Response<object>> Handle(ListConnectorQuery request, CancellationToken cancellationToken)
    {
        var connectors = await _dbContext.Set<Connector>()
                                         .Where(x => (request.ConnectorType == null || x.Type == request.ConnectorType) &&
                                                     x.ProjectId == request.ProjectId)
                                         .Select(x => new
                                         {
                                             x.Id,
                                             x.Type,
                                             x.TypeDescription,
                                             x.UserName,
                                             x.Password,
                                             x.Url,
                                             ConnectorReportPriorities = x.ConnectorReportPriorities.Select(x => new { x.Name, x.Key }),
                                             ConnectorReportTypes = x.ConnectorReportTypes.Select(x => new { x.Name, x.Key })
                                         })
                                         .ToListAsync();

        return Response<object>.Success(connectors);
    }
}
