namespace Uno.Application.Settings;

public record HostSettings
{
    public string BaseProject { get; set; }

    public string BaseDirectory { get; set; }

    public string BaseFolder { get; set; }
}
