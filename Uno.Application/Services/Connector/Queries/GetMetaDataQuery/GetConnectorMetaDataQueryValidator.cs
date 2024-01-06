namespace Uno.Application.Services;

public class GetConnectorMetaDataQueryValidator : AbstractValidator<GetConnectorMetaDataQuery>
{
    private readonly IDbContext _dbContext;

    public GetConnectorMetaDataQueryValidator(IDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.ConnectorId)
            .NotNull()
            .MustAsync(IsConnectorExists)
            .WithMessage(ServiceMessages.ConnectorNotFound);
    }

    private async Task<bool> IsConnectorExists(Guid connectorId, CancellationToken cancellationToken)
    {
        var connector = await _dbContext.Set<Connector>().FirstOrDefaultAsync(x => x.Id == connectorId, cancellationToken);
        return connector is not null;
    }
}
