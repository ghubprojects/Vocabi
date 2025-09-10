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

    public static string? GetExtensionFromContentType(string contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
            return null;

        foreach (var kvp in _provider.Mappings)
            if (string.Equals(kvp.Value, contentType, StringComparison.OrdinalIgnoreCase))
                return kvp.Key;

        return null;
    }

    public static string EnsureFileNameHasExtension(string fileName, string? contentType)
    {
        if (Path.HasExtension(fileName))
            return fileName;

        var ext = GetExtensionFromContentType(contentType ?? string.Empty);
        if (!string.IsNullOrWhiteSpace(ext))
            return fileName + ext;

        return fileName; // fallback giữ nguyên
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

    public static string GetWwwRootPath(string? relativePath = null)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        if (string.IsNullOrWhiteSpace(relativePath))
            return basePath;

        return Path.Combine(basePath, relativePath);
    }

    public static string GetUploadPath()
    {
        return Path.Combine(GetWwwRootPath(), "uploads");
    }

    public static async Task CopyFilesAsync(IEnumerable<string> sourceFiles, string destinationFolder, bool overwrite = true)
    {
        if (!Directory.Exists(destinationFolder))
            Directory.CreateDirectory(destinationFolder);

        var tasks = new List<Task>();

        foreach (var file in sourceFiles)
        {
            if (File.Exists(file))
            {
                string fileName = Path.GetFileName(file);
                string destPath = Path.Combine(destinationFolder, fileName);

                tasks.Add(Task.Run(async () =>
                {
                    using var sourceStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, useAsync: true);
                    using var destinationStream = new FileStream(destPath, overwrite ? FileMode.Create : FileMode.CreateNew, FileAccess.Write, FileShare.None, 8192, useAsync: true);
                    await sourceStream.CopyToAsync(destinationStream);
                }));
            }
            else
            {
                Console.WriteLine($"File không tồn tại: {file}");
            }
        }

        await Task.WhenAll(tasks);
    }
}
