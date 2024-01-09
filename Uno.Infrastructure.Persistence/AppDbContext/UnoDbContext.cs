using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Uno.Application.Common;
using Uno.Domain.Common;
using Uno.Domain.Constants;
using Uno.Infrastructure.Extentions;
using Uno.Shared.Common;

namespace Uno.Infrastructure.AppDbContext;

public class UnoDbContext : DbContext, IDbContext
{
    public UnoDbContext(DbContextOptions<UnoDbContext> options) : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        => configurationBuilder.Properties<string>().HaveMaxLength(200);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(EntitySchema.Base);
        modelBuilder.RegisterEntities(typeof(EntityAttribute).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.NoAction;
    }

    public async Task<Response> SaveChangeResposeAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await SaveChangesAsync(cancellationToken) is 0 ? Response.Error() : Response.Success();
        }
        catch (Exception ex)
        {
            return Response.Error(ex.InnerException.Message);
        }
    }

    public Response SaveChangeResponse()
    {
        try
        {
            return SaveChanges() is 0 ? Response.Error() : Response.Success();
        }
        catch (Exception ex)
        {
            return Response.Error(ex.InnerException.Message);
        }
    }

    public new DbSet<TEntity> Set<TEntity>() where TEntity : class, IEntity
        => base.Set<TEntity>();

    public DatabaseFacade DatabaseFacade()
        => Database;
}
