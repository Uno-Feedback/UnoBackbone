using Microsoft.Extensions.DependencyInjection;
using Uno.Infrastructure.ExternalServices.Services;
using Uno.Infrastructure.ExternalServices.Services.Factories;
using Uno.Infrastructure.ExternalServices.Services.Pipelines;

namespace Uno.Infrastructure.ExternalServices;

/// <summary>
/// This extention is programmed for registering Infrastructure.External services .
/// </summary>
public static class ConfigureServices
{
    public static IServiceCollection RegisterInfrastructureExternalServices(this IServiceCollection services)
    {
        services.AddScoped<IClientAdapterFactory, ClientAdapterFactory>();
        services.AddScoped<ISendIssuePipelineBuilder, SendIssuePipelineBuilder>();
        services.AddScoped<IJiraCreateIssueHandler, JiraCreateIssueHandler>();
        services.AddScoped<IJiraUploadAttachmentHandler, JiraUploadAttachmentHandler>();
        services.AddScoped<JiraAdapter>().AddScoped<IClientAdapter, JiraAdapter>();
        return services;
    }
}
