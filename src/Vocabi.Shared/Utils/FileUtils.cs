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

    /// <summary>
    /// Lấy tên file an toàn (tránh null/empty).
    /// </summary>
    public static string GetSafeFileName(string filePath)
    {
        return string.IsNullOrWhiteSpace(filePath)
            ? "unknown"
            : Path.GetFileName(filePath);
    }

    /// <summary>
    /// Kiểm tra file có phải là hình ảnh hay không (jpg, png, gif...).
    /// </summary>
    public static bool IsImage(string fileName)
    {
        var contentType = GetContentType(fileName);
        return contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
    }
}
