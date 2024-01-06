namespace Uno.Application.Services;

public class AddProjectCommandHandler : IRequestHandler<AddProjectCommand, Response<object>>
{
    private readonly IDbContext _dbContext;
    public AddProjectCommandHandler(IDbContext dbContext)
         => _dbContext = dbContext;

    public async Task<Response<object>> Handle(AddProjectCommand request, CancellationToken cancellationToken)
    {
        var newProject = new Project().MapFrom(request);

        _dbContext.Set<Project>().Add(newProject);
        var saveResponse = await _dbContext.SaveChangeResposeAsync(cancellationToken);

        return saveResponse.IsSuccess ? Response<object>.Success(new { newProject.Id }) : Response<object>.Error(saveResponse.Message);
    }
}