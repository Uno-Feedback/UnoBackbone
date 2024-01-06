using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Uno.Application.Behaviors;
using Uno.Application.Behaviors.FileValidators;
using Uno.Application.Behaviors.FileValidators.Contracts;

namespace Uno.Application;

/// <summary>
/// This extention is programmed for registering Application services .
/// </summary>
public static class ConfigServices
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        services.AddScoped<IFileValidationServiceFactory, FileValidationServiceFactory>();
        services.AddScoped<VideoValidationService>().AddScoped<IFileValidationService, VideoValidationService>();
        return services;
    }
}
