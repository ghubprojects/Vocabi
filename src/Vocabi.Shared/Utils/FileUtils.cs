using Microsoft.AspNetCore.StaticFiles;
using static Vocabi.Shared.Common.Enums;

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

    public static MediaType GetMediaType(string contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
            return MediaType.Unknown;

        if (contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            return MediaType.Image;

        if (contentType.StartsWith("video/", StringComparison.OrdinalIgnoreCase))
            return MediaType.Video;

        if (contentType.StartsWith("audio/", StringComparison.OrdinalIgnoreCase))
            return MediaType.Audio;

        if (contentType.StartsWith("application/pdf", StringComparison.OrdinalIgnoreCase) || contentType.StartsWith("text/", StringComparison.OrdinalIgnoreCase))
            return MediaType.Document;

        return MediaType.Other;
    }

    public static string GenerateSafeFileName(string originalFileName)
    {
        string extension = Path.GetExtension(originalFileName);
        string randomPart = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());

        return $"{randomPart}{extension}";
    }

    public static string GetFileNameFromUrl(string url)
    {
        var uri = new Uri(url);
        return Path.GetFileName(uri.AbsolutePath);
    }
}
