namespace Uno.Application.Settings;

public record SendIssueConfig
{
    public string TakeCount { get; set; }
    public string TimePeriod { get; set; }
    public int UploadAttachmentTryCountAmount { get; set; }
    public int CreateIssueTryCountAmount { get; set; }
}
