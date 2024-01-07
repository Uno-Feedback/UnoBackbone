using Microsoft.EntityFrameworkCore.Infrastructure;
using Uno.Domain.Common;

namespace Uno.Application.Common;

public interface IDbContext : IDisposable, IAsyncDisposable
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class, IEntity;
    DatabaseFacade DatabaseFacade();
    Task<Response> SaveChangeResposeAsync(CancellationToken ct = default);
    Response SaveChangeResponse();
}
