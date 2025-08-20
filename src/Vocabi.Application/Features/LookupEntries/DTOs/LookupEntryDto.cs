using AutoMapper;
using Vocabi.Application.Features.MediaFiles.DTOs;
using Vocabi.Domain.Aggregates.LookupEntries;

namespace Vocabi.Application.Features.LookupEntries.DTOs;

public class LookupEntryDto
{
    public string Headword { get; set; } = string.Empty;
    public string PartOfSpeech { get; set; } = string.Empty;
    public string Pronunciation { get; set; } = string.Empty;
    public List<string> Meanings { get; set; } = [];
    public List<LookupEntryDefinitionDto> Definitions { get; set; } = [];
    public List<MediaFileDto> MediaFiles { get; set; } = [];

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<LookupEntry, LookupEntryDto>()
                .ForMember(dest => dest.Meanings, opt => opt.MapFrom(src => src.Meanings.Select(m => m.Text)))
                .ForMember(dest => dest.Definitions, opt => opt.MapFrom(src => src.Definitions))
                .ForMember(dest => dest.MediaFiles, opt => opt.Ignore());
        }
    }
}
