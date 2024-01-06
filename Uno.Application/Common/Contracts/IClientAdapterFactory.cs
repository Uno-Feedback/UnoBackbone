using Uno.Domain.Enums;

namespace Uno.Application.Common.Contracts;

/// <summary>
/// An Interface for ClientAdapterFactory
/// </summary>
public interface IClientAdapterFactory
{
    /// <summary>
    /// This Method is designed for recieving diffrent ClinetAdapters by given ConnectorType.
    /// </summary>
    /// <param name="connectorType"></param>
    /// <returns></returns>
    IClientAdapter GetInstance(ConnectorTypes connectorType);
}
