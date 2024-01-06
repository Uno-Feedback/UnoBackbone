using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Uno.Application.Behaviors.FileValidators.Contracts;
namespace Uno.Application.Behaviors.FileValidators;

public class VideoValidationService : IFileValidationService
{
    private const int _baseSize = 1024;
    private readonly IConfiguration _configuration;
    private readonly string[] _supportedFormats;
    private readonly int _maxSizeInMb;

    public VideoValidationService(IConfiguration configuration )
    {
        _configuration = configuration;
        _maxSizeInMb = int.Parse(_configuration["FileMaxSize"]);
        _supportedFormats = _configuration.GetSection("FileSupportedFormats:VideoFormats").GetChildren().Select(x => x.Value).ToArray();
    }

    private bool SizeValidation(long fileSize)
       => fileSize / (_baseSize * _baseSize) <= _maxSizeInMb;

    private bool FormatValidation(string fileFormat)
        => _supportedFormats.Contains(fileFormat);

    public Response Validate(IFormFile file)
    {
        if (!FormatValidation(Path.GetExtension(file.FileName)))
            return Response.Error(ServiceMessages.InvalidFileFormat);

        if (!SizeValidation(file.Length))
            return Response.Error(ServiceMessages.InvalidFileSize);

        return Response.Success();
    }
}