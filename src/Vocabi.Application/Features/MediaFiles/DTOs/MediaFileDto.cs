using AutoMapper;
using Vocabi.Domain.Aggregates.MediaFiles;

namespace Vocabi.Application.Features.MediaFiles.DTOs;

public class MediaFileDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long Size { get; set; } // in bytes
    public string SourceName { get; set; } = string.Empty;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<MediaFile, MediaFileDto>();
        }
    }
}