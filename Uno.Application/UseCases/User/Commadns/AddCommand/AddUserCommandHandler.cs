namespace Uno.Application.Services;

public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Response<object>>
{
    private readonly IDbContext _dbContext;

    public AddUserCommandHandler(IDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<Response<object>> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var newUser = new User().MapFrom(request);

        _dbContext.Set<User>().Add(newUser);
        var saveResponse = await _dbContext.SaveChangeResposeAsync(cancellationToken);

        return saveResponse.IsSuccess ? Response<object>.Success(newUser) : Response<object>.Error(saveResponse.Message);
    }
}
