#nullable disable

namespace Vocabi.Domain.Aggregates.MediaFiles;

public class MediaFile
{
    public Guid Id { get; private set; }
    public string FileName { get; private set; }
    public string FilePath { get; private set; }
    public string ContentType { get; private set; }
    public MediaType MediaType { get; private set; }
    public long Size { get; private set; } // in bytes
    public MediaSourceCategory SourceCategory { get; private set; }
    public string SourceName { get; private set; }

    private MediaFile() { }

    private MediaFile(string fileName, string filePath, string contentType, long size, MediaSourceCategory sourceCategory, string sourceName)
    {
        Id = Guid.NewGuid();
        FileName = fileName;
        FilePath = filePath;
        ContentType = contentType;
        MediaType = MediaType.FromContentType(contentType);
        Size = size;
        SourceCategory = sourceCategory;
        SourceName = sourceName;
    }

    public static MediaFile CreateNew(string fileName, string filePath, string contentType, long size, MediaSourceCategory sourceCategory, string sourceName)
    {
        return new MediaFile(fileName, filePath, contentType, size, sourceCategory, sourceName);
    }
}