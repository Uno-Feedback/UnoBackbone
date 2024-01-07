using Microsoft.Extensions.DependencyInjection;
using Uno.Infrastructure.ExternalServices.Services;
using Uno.Infrastructure.ExternalServices.Services.Factories;

namespace Uno.Infrastructure.ExternalServices;

/// <summary>
/// This extention is programmed for registering Infrastructure.External services .
/// </summary>
public static class ConfigureServices
{
    public static IServiceCollection RegisterInfrastructureExternalServices(this IServiceCollection services)
    {
        services.AddScoped<IClientAdapterFactory, ClientAdapterFactory>();
        services.AddScoped<JiraAdapter>().AddScoped<IClientAdapter, JiraAdapter>();
        services.AddScoped<IJiraClientFactory, JiraClientFactory>();
        return services;
    }
}
