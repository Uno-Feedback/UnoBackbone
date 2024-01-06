

namespace Uno.Application.Services;

public record AddConnectorCommand : IRequest<Response<object>>
{
    public Guid ProjectId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Url { get; set; }
    public string Key { get; set; }
    public ConnectorTypes Type { get; set; }
    public IEnumerable<ConnectorReportsDto> ConnectorReportPriorities { get; set; }
    public IEnumerable<ConnectorReportsDto> ConnectorReportTypes { get; set; }
}

