using Uno.Api.Quartz.Config;
using Uno.Api.Quartz.Settings;

namespace Uno.Api;

public static class ConfigureServices
{
    public static IServiceCollection RegisterPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        services.RegisterQuartzServices(configuration);

        services.Configure<IssueJobConfig>(configuration.GetSection(nameof(IssueJobConfig)));

        return services;
    }
}
