#nullable disable

namespace Uno.Infrastructure.ExternalServices.Services.Factories;
public class ClientAdapterFactory : IClientAdapterFactory
{
    private readonly IServiceProvider _serviceProvider;
    public ClientAdapterFactory(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    public IClientAdapter GetInstance(ConnectorTypes connectorType)
        => connectorType switch
        {
            ConnectorTypes.Jira => (IClientAdapter)_serviceProvider.GetService(typeof(JiraAdapter)),
            _ => (IClientAdapter)_serviceProvider.GetService(typeof(JiraAdapter))
        };
}
