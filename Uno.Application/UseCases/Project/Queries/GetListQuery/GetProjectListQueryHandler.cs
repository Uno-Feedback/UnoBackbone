namespace Uno.Application.Services;

public class GetProjectListQueryHandler : IRequestHandler<GetProjecttListQuery, Response<object>>
{
    private readonly IDbContext _dbContext;

    public GetProjectListQueryHandler(IDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<Response<object>> Handle(GetProjecttListQuery request, CancellationToken cancellationToken)
    {
        var porjects = await _dbContext.Set<Project>()
                                        .Where(x => (request.userId == null || x.UserId == request.userId) &&
                                                    (request.projectId == null || x.Id == request.projectId) &&
                                                    x.IsActive == true)
                                        .Select(x => new
                                        {
                                            x.Id,
                                            x.UserId,
                                            x.Name,
                                            x.IP,
                                            x.InsertTime,
                                        })
                                        .ToListAsync();

        return Response<object>.Success(porjects);
    }
}
