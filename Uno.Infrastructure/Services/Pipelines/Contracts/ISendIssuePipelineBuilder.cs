
namespace Uno.Infrastructure.ExternalServices.Services.Pipelines;

public interface ISendIssuePipelineBuilder
{
    ISendIssuePipelineBuilder AddHandler(ISendIssuePipeline handler);
    ISendIssuePipeline Build();
}