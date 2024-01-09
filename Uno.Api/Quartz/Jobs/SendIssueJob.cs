using MediatR;
using Quartz;
using Uno.Application.Services;


namespace Uno.Api.Quartz.Jobs;

[DisallowConcurrentExecution]
public class SendIssueJob : IJob
{
    private readonly IMediator _mediatR;

    public SendIssueJob(IMediator mediator)
        => _mediatR = mediator;

    public async Task Execute(IJobExecutionContext context)
        => await _mediatR.Send(new SendIssueCommand());
}