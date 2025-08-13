#nullable disable

namespace Vocabi.Domain.Aggregates.MediaFiles;

public class MediaFile
{
    public Guid Id { get; private set; }
    public string FileName { get; private set; }
    public string FilePath { get; private set; }
    public string ContentType { get; private set; }
    public MediaType MediaType { get; private set; } // MIME type
    public long Size { get; private set; } // in bytes
    public MediaSourceCategory SourceCategory { get; private set; }
    public string SourceName { get; private set; }
}