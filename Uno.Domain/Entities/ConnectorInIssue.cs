using Uno.Domain.Common;
using Uno.Domain.Enums;

namespace Uno.Domain.Entities;

[Entity]
public class ConnectorInIssue : IEntity
{
    public int Id { get; set; }
    public Guid ConnectorId { get; set; }
    public Connector Connector { get; set; }
    public Guid IssueId { get; set; }
    public Issue Issue { get; set; }
    public string ConnectorMetaData { get; set; }
    public string ClientMetaData { get; set; }
    public string? IssueMetaData { get; set; }
    public int TryCount { get; set; }
    public IssueStatus Status { get; set; } = IssueStatus.ReadyForSend;

    /// <summary>
    /// This method is programmed for checking Entity validation for send .
    /// </summary>
    /// <returns></returns>
    public bool IsValidForSend()
        => Status != IssueStatus.ReadyForSend || Status != IssueStatus.SendWithoutAttachment || !string.IsNullOrWhiteSpace(ConnectorMetaData);


    /// <summary>
    /// This method is programmed for checking Entity validation for send without attachment .
    /// </summary>
    /// <returns></returns>
    public bool IsValidForSendWithoutAttachment()
        => Status != IssueStatus.SendWithoutAttachment || !string.IsNullOrWhiteSpace(IssueMetaData) || !string.IsNullOrWhiteSpace(ConnectorMetaData);
}
