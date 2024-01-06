using System.ComponentModel;

namespace Uno.Domain.Enums;

/// <summary>
/// This enum represent diffrent available types of Connector .
/// </summary>
public enum ConnectorTypes : byte
{
    [Description(nameof(Jira))]
    Jira = 10,

    [Description(nameof(Slack))]
    Slack = 20
}