using System.ComponentModel;

namespace Uno.Domain.Enums;

/// <summary>
/// This enum represent diffrent types of supprted IssueAttachment .
/// </summary>
public enum IssueAttachmentTypes : byte
{
    [Description("ویدیو")]
    Video = 10,

    [Description("عکس")]
    Image = 20,

    [Description("متن")]
    Text = 30,
}
