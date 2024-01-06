namespace Uno.Application.Services;

public class AddIssueCommandValidator : AbstractValidator<AddIssueCommand>
{
    private readonly IDbContext _dbContext;

    public AddIssueCommandValidator(IDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.ConnectorMetaData)
            .NotNull()
            .WithMessage(ServiceMessages.InvalidConnectorMetaData);

        RuleFor(x => x.AttachmentType)
            .Must(x => x.IsValidEnumValue<IssueAttachmentTypes>())
            .WithMessage(ServiceMessages.InvalidIssueAttachmentType);

        RuleFor(x => x.Description)
            .NotNull()
            .WithMessage(ServiceMessages.InvalidIssueDescription);

        RuleFor(x => x.Subject)
           .NotNull()
           .WithMessage(ServiceMessages.InvalidIssueDescription);

        RuleFor(x => x.File)
            .NotNull()
            .WithMessage(ServiceMessages.FileIsRequired);

        RuleFor(x => x.ProjectToken)
            .MustAsync(IsProjectExists)
            .WithMessage(ServiceMessages.ProjectNotFound);

        RuleFor(x => x.ConnectorId)
            .MustAsync(IsConnectorExist)
            .WithMessage(ServiceMessages.ConnectorNotFound);

    }

    private async Task<bool> IsProjectExists(Guid projectId, CancellationToken cancellationToken)
        => await _dbContext.Set<Project>().AnyAsync(x => x.Id == projectId, cancellationToken);

    private async Task<bool> IsConnectorExist(Guid connectorId, CancellationToken cancellationToken)
        => await _dbContext.Set<Connector>().AnyAsync(x => x.Id == connectorId, cancellationToken);

}
