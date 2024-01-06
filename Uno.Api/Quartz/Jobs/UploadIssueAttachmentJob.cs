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
public class UploadIssueAttachmentJob : IJob
{
    private readonly IDbContext _dbContext;
    private readonly IMediator _mediatR;
    private readonly IssueJobConfig _uploadIssueAttachmentJobConfig;
    private readonly static object _lock = new();

    public UploadIssueAttachmentJob(IDbContext dbContext, IMediator mediator, IOptionsMonitor<IssueJobConfig> optionsMonitor)
    {
        _dbContext = dbContext;
        _mediatR = mediator;
        _uploadIssueAttachmentJobConfig = optionsMonitor.CurrentValue; 
    }


    public async Task Execute(IJobExecutionContext context)
    {
        List<ConnectorInIssue> connectorInIssues = new();
        lock (_lock)
        {
            connectorInIssues = _dbContext.Set<ConnectorInIssue>()
                                          .Where(x => x.Status == IssueStatus.SendWithoutAttachment && x.TryCount < _uploadIssueAttachmentJobConfig.UploadAttachmentTryCountAmount)
                                          .Take(byte.Parse(_uploadIssueAttachmentJobConfig.TakeCount))
                                          .ToList();

            if (connectorInIssues.Any() is false)
                return;

            connectorInIssues.ForEach(cis => { cis.TryCount++; cis.Status = IssueStatus.InProgress; });

            var saveResponse = _dbContext.SaveChangeResponse();
            if (saveResponse.IsFailure)
                return;
        }
        foreach (var connectorInIssue in connectorInIssues)
            await _mediatR.Send(new SendIssueCommand { ConnectorId = connectorInIssue.ConnectorId, IssueId = connectorInIssue.IssueId }, default);
    }
}
