using MediatR;
using Microsoft.Extensions.Options;
using Quartz;
using Uno.Api.Quartz.Settings;
using Uno.Application.Common;
using Uno.Application.Services;
using Uno.Domain.Entities;
using Uno.Domain.Enums;

namespace Uno.Api.Quartz.Jobs;

[DisallowConcurrentExecution]
public class CreateIssueJob : IJob
{
    private readonly IDbContext _dbContext;
    private readonly IMediator _mediatR;
    private readonly IssueJobConfig _sendIssueJobConfig;
    private readonly static object _lock = new();

    public CreateIssueJob(IDbContext dbContext, IMediator mediator, IOptionsMonitor<IssueJobConfig> config)
    {
        _dbContext = dbContext;
        _sendIssueJobConfig = config.CurrentValue;
        _mediatR = mediator;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        List<ConnectorInIssue> connectorInIssues = new();
        lock (_lock)
        {
            connectorInIssues = _dbContext.Set<ConnectorInIssue>()
                                          .Where(x => x.Status == IssueStatus.ReadyForSend && x.TryCount < _sendIssueJobConfig.CreateIssueTryCountAmount) 
                                          .Take(byte.Parse(_sendIssueJobConfig.TakeCount))
                                          .ToList();

            if (connectorInIssues.Any() is false)
                return;

            connectorInIssues.ForEach(cis => { cis.TryCount++; cis.Status= IssueStatus.InProgress; });

            var saveResponse = _dbContext.SaveChangeResponse();
            if (saveResponse.IsFailure)
                return;
        }
        foreach (var connectorInIssue in connectorInIssues)
            await _mediatR.Send(new SendIssueCommand { ConnectorId = connectorInIssue.ConnectorId, IssueId = connectorInIssue.IssueId }, default); 

    }
}