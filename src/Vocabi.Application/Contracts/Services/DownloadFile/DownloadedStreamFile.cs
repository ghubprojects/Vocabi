namespace Vocabi.Application.Contracts.Services.DownloadFile;

public class DownloadedStreamFile(Stream content, string fileName, string? contentType = null)
{
    public Stream Content { get; } = content;
    public string FileName { get; } = fileName;
    public string? ContentType { get; } = contentType;
}