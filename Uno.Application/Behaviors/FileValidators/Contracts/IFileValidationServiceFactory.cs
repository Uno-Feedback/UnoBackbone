using Uno.Domain.Enums;

namespace Uno.Application.Behaviors.FileValidators.Contracts;

/// <summary>
/// This Interface is designed for creating instance of classes that implemented specific interface .
/// </summary>
public interface IFileValidationServiceFactory
{
    /// <summary>
    /// This Method is designed for returning created class based on given Type.
    /// </summary>
    /// <param name="attachmentType"></param>
    /// <returns></returns>
    IFileValidationService GetInstance(IssueAttachmentTypes attachmentType);
}
