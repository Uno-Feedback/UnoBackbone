using Microsoft.Extensions.DependencyInjection;
using Uno.Infrastructer.ExternalServices.Services;
using Uno.Infrastructer.ExternalServices.Services.Factories;

namespace Uno.Infrastructer.ExternalServices;

/// <summary>
/// This extention is programmed for registering Infrastructer.External services .
/// </summary>
public static class ConfigureServices
{
    public static IServiceCollection RegisterInfrastructerExternalServices(this IServiceCollection services)
    {
        services.AddScoped<IClientAdapterFactory, ClientAdapterFactory>();
        services.AddScoped<JiraAdapter>().AddScoped<IClientAdapter, JiraAdapter>();
        services.AddScoped<IJiraClientFactory, JiraClientFactory>();
        return services;
    }
}
