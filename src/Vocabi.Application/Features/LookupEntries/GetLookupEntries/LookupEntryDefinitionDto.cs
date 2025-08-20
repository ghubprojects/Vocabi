using AutoMapper;
using Vocabi.Domain.Aggregates.LookupEntries;

namespace Vocabi.Application.Features.LookupEntries.GetLookupEntries;

public class LookupEntryDefinitionDto
{
    public string Text { get; set; } = string.Empty;
    public List<string> Examples { get; set; } = [];

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<LookupEntryDefinition, LookupEntryDefinitionDto>()
                .ForMember(dest => dest.Examples, opt => opt.MapFrom(src => src.Examples.Select(e => e.Text)));
        }
    }
}
