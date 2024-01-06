using Uno.Application.Common.Dtos;
using Uno.Domain.Enums;
using Uno.Shared.Common;

namespace Uno.Application.Common.Contracts;

/// <summary>
/// An Interface for diffrent ClientAdapters.
/// </summary>
public interface IClientAdapter
{
    /// <summary>
    /// This Method is designed for recieving MetaData of ClientAdatper.
    /// </summary>
    /// <param name="connectorDto"></param>
    /// <returns></returns>
    Task<Response<object>> GetMetaData(ConnectorDto connectorDto, CancellationToken cancellationToken);

    /// <summary>
    /// This Method is designed for sending Issue to ClientAdapter.
    /// </summary>
    /// <param name="issueDto"></param>
    /// <returns></returns>
    Task<Response<IssueStatus>> SendIssue(SendIssueDto issueDto, CancellationToken cancellationToken);
}
