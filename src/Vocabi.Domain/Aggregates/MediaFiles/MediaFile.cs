#nullable disable

using Vocabi.Domain.SeedWork;

namespace Vocabi.Domain.Aggregates.MediaFiles;

public class MediaFile : IAggregateRoot
{
    public Guid Id { get; private set; }
    public string FileName { get; private set; }
    public string FilePath { get; private set; }
    public string ContentType { get; private set; }
    public long Size { get; private set; } // in bytes
    public string Provider { get; private set; }

    private MediaFile() { }

    private MediaFile(string fileName, string filePath, string contentType, long size, string provider)
    {
        Id = Guid.NewGuid();
        FileName = fileName;
        FilePath = filePath;
        ContentType = contentType;
        Size = size;
        Provider = provider;
    }

    public static MediaFile CreateNew(string fileName, string filePath, string contentType, long size, string provider)
    {
        return new MediaFile(fileName, filePath, contentType, size, provider);
    }
}