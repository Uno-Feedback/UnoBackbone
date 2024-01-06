using System.ComponentModel;

namespace Uno.Domain.Enums;

/// <summary>
/// This enum represents diffrent status for ConnectorInIssue .
/// </summary>
public enum IssueStatus
{
    [Description("آماده برای ارسال")]
    ReadyForSend = 10,

    [Description("در حال بررسی")]
    InProgress = 20,

    [Description("ارسال شده بدون فایل")]
    SendWithoutAttachment = 30,

    [Description("ارسال شده")]
    Finished = 40,

    [Description("خطا")]
    Failed = 50,
}
