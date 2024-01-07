namespace Uno.Application.Services;

public class AddConnectorCommandValidator : AbstractValidator<AddConnectorCommand>
{
    private readonly IDbContext _dbContext;

    public AddConnectorCommandValidator(IDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage(ServiceMessages.InvalidConnectorUserName);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(ServiceMessages.InvalidConnectorPassword);

        RuleFor(x => x.Url)
            .NotEmpty()
            .WithMessage(ServiceMessages.InvalidConnectorUrl);

        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .MustAsync(IsProjectExists)
            .WithMessage(ServiceMessages.ProjectNotFound);

        RuleFor(x => x.Type)
            .Must(x => x.IsValidEnumValue<ConnectorTypes>())
            .WithMessage(ServiceMessages.InvalidEnumValue);
    }

    private async Task<bool> IsProjectExists(Guid projectId, CancellationToken cancellationToken)
        => await _dbContext.Set<Project>().AnyAsync(x => x.Id == projectId, cancellationToken);   
}
