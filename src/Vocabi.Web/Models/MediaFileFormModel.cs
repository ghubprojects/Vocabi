using Microsoft.FluentUI.AspNetCore.Components;
using Vocabi.Domain.Aggregates.MediaFiles;

namespace Vocabi.Web.Models;

public class MediaFileInputModel
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public MediaType MediaType { get; set; } // MIME type
    public long Size { get; set; } // in bytes
    public MediaSourceCategory SourceCategory { get; set; }
    public string SourceName { get; set; } = string.Empty;

    public int MaxFileSizeInKB { get; set; }
    public string Accept { get; set; } = string.Empty;
    public string AnchorId { get; set; } = string.Empty;
    public string Placeholder { get; set; } = string.Empty;
    public Icon Icon { get; set; } = default!;
}