using AutoMapper;
using Vocabi.Domain.Aggregates.LookupEntries;

namespace Vocabi.Application.Features.LookupEntries.GetLookupEntries;

public class LookupEntryDto
{
    public string Headword { get; set; } = string.Empty;
    public string PartOfSpeech { get; set; } = string.Empty;
    public string Pronunciation { get; set; } = string.Empty;
    public List<string> Meanings { get; set; } = [];
    public List<LookupEntryDefinitionDto> Definitions { get; set; } = [];
    public List<Guid> MediaFileIds { get; set; } = [];

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<LookupEntry, LookupEntryDto>()
                .ForMember(dest => dest.Meanings, opt => opt.MapFrom(src => src.Meanings.Select(m => m.Text)))
                .ForMember(dest => dest.Definitions, opt => opt.MapFrom(src => src.Definitions))
                .ForMember(dest => dest.MediaFileIds, opt => opt.MapFrom(src => src.MediaFiles.Select(x => x.MediaFileId)));
        }
    }
}
