namespace Uno.Infrastructure.ExternalServices.Services.Pipelines;

public class SendIssuePipelineBuilder : ISendIssuePipelineBuilder
{
    private List<ISendIssuePipeline> _handlers = new();

    public ISendIssuePipelineBuilder AddHandler(ISendIssuePipeline handler)
    {
        _handlers.Add(handler);
        return this; 
    }

    public ISendIssuePipeline Build()
    {
        for(var i =0 ; i< _handlers.Count - 1 ; i ++ )
            _handlers[i].SetNextHandler(_handlers[i + 1]);

        return _handlers.First();
    }
}