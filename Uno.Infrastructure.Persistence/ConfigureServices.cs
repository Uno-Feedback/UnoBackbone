using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Uno.Application.Common;
using Uno.Infrastructure.AppDbContext;

namespace Uno.Infrastructure;

/// <summary>
/// This extention is programmed for registering Infrastructure services .
/// </summary>
public static class ConfigureServices
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IDbContext, UnoDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString(nameof(UnoDbContext))));
        return services;
    }
}
