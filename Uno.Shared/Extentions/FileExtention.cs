using Microsoft.AspNetCore.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Uno.Shared.Extentions;

public static class FileExtention
{
    public static async Task<string> ConvertToBase64(this IFormFile file)
    {
        using(var ms = new MemoryStream()) 
        { 
            await file.CopyToAsync(ms);
            var fileAsByteArray = ms.ToArray();
            var fileAsBase64 = Convert.ToBase64String(fileAsByteArray);
            return fileAsBase64;
        }
    }

    public static async Task<byte[]> ConvertToByteArray(this string base64String)
    {
        var fileAsByteArray = Convert.FromBase64String(base64String);
        return fileAsByteArray;
    }

    public static byte[] ConvertToJsonFileAsByteArray(this string jsonAsString)
    {
        var jsonDocument = JsonDocument.Parse(jsonAsString);
        using (var ms = new MemoryStream())
        {
            using (Utf8JsonWriter writer = new Utf8JsonWriter(ms))
            {
                jsonDocument.WriteTo(writer);
            }

            return ms.ToArray();
        }
    }
}
