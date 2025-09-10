using Vocabi.Shared.Utils;

namespace Vocabi.Application.Contracts.Services.DownloadFile;

public class DownloadedBinaryFile
{
    public byte[] Content { get; }
    public string FileName { get; }
    public string? ContentType { get; }

    public DownloadedBinaryFile(byte[] content, string fileName, string? contentType = null)
    {
        Content = content ?? throw new ArgumentNullException(nameof(content));
        FileName = FileUtils.EnsureFileNameHasExtension(fileName, contentType);
        ContentType = contentType ?? FileUtils.GetContentType(FileName);
    }
}