using Vocabi.Application.Features.MediaFiles.DTOs;
using Vocabi.Web.Models;

namespace Vocabi.Web.Extensions;

public static class MediaFileMappingExtensions
{
    public static void UpdateFrom(this MediaFileInputModel target, MediaFileDto source)
    {
        if (source == null || target == null) return;

        target.Id = source.Id;
        target.FileName = source.FileName;
        target.FilePath = source.FilePath;
        target.ContentType = source.ContentType;
        target.MediaType = source.MediaType;
        target.Size = source.Size;
        target.SourceCategory = source.SourceCategory;
        target.SourceName = source.SourceName;
    }
}
