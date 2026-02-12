namespace WebHost.ViewModels.MediaFiles;

public class MediaFileViewModel
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long Size { get; set; } // in bytes
    public string SourceName { get; set; } = string.Empty;
}