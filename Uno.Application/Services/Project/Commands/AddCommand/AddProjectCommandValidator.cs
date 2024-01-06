namespace Uno.Application.Services;

public class AddProjectCommandValidator : AbstractValidator<AddProjectCommand>
{
    private readonly IDbContext _dbContext;
    public AddProjectCommandValidator(IDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.UserId)
               .NotEmpty()
               .MustAsync(IsUserExists)
               .WithMessage(ServiceMessages.InvalidUserId);
    }

    private async Task<bool> IsUserExists(Guid userId, CancellationToken cancellationToken)
        => await _dbContext.Set<User>().AnyAsync(x => x.Id == userId, cancellationToken);

}
