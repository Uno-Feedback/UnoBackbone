using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Uno.Application.Common;
using Uno.Infrastructer.AppDbContext;

namespace Uno.Infrastructer;

/// <summary>
/// This extention is programmed for registering Infrastructer services .
/// </summary>
public static class ConfigureServices
{
    public static IServiceCollection RegisterInfrastructerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IDbContext, UnoDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString(nameof(UnoDbContext))));
        return services;
    }
}
