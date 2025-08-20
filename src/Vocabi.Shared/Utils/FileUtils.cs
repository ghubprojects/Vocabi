using Microsoft.AspNetCore.StaticFiles;

namespace Vocabi.Shared.Utils;

public static class FileUtils
{
    private static readonly FileExtensionContentTypeProvider _provider = new();

    public static string GetContentType(string fileName)
    {
        if (_provider.TryGetContentType(fileName, out var contentType))
            return contentType;

        return "application/octet-stream";
    }

    public static string GenerateSafeFileName(string originalFileName)
    {
        string extension = Path.GetExtension(originalFileName);
        string randomPart = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
        string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

        return $"{randomPart}_{timestamp}{extension}";
    }

    public static string GetFileNameFromUrl(string url)
    {
        var uri = new Uri(url);
        return Path.GetFileName(uri.AbsolutePath);
    }
}
