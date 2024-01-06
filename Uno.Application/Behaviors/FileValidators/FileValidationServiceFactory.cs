using Uno.Application.Behaviors.FileValidators.Contracts;
using Uno.Domain.Enums;

namespace Uno.Application.Behaviors.FileValidators;

#nullable disable
public class FileValidationServiceFactory : IFileValidationServiceFactory
{
    private readonly IServiceProvider _serviceProvider;
    public FileValidationServiceFactory(IServiceProvider serviceProvider)
       => _serviceProvider = serviceProvider;

    public IFileValidationService GetInstance(IssueAttachmentTypes attachmentType)
        => attachmentType switch
        {
            IssueAttachmentTypes.Video => (IFileValidationService)_serviceProvider.GetService(typeof(VideoValidationService)),
            _ => (IFileValidationService)_serviceProvider.GetService(typeof(VideoValidationService)),
        };
}