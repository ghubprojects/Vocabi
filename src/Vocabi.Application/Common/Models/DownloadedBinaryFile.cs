namespace Vocabi.Application.Common.Models;

public class DownloadedBinaryFile(byte[] content, string fileName, string? contentType = null)
{
    public byte[] Content { get; } = content;
    public string FileName { get; } = fileName;
    public string? ContentType { get; } = contentType;
}