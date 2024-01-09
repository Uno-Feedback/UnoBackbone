using Uno.Api.Quartz.Config;

namespace Uno.Api;

public static class ConfigureServices
{
    public static IServiceCollection RegisterPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        services.RegisterQuartzServices(configuration);
        return services;
    }
}
