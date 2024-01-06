using Microsoft.AspNetCore.Http;
using Uno.Shared.Common;

namespace Uno.Application.Behaviors.FileValidators.Contracts;

public interface IFileValidationService
{
    Response Validate(IFormFile file);
}
