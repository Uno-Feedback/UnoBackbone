namespace Uno.Api.Quartz.Settings;

public record IssueJobConfig
{
    public string TakeCount { get; set; }
    public string TimePeriod { get; set; }
    public int UploadAttachmentTryCountAmount { get; set; }
    public int CreateIssueTryCountAmount { get; set; }
}
