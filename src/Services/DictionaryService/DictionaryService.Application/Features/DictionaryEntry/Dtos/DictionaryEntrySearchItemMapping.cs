using AutoMapper;

namespace DictionaryService.Application.Features.DictionaryEntry.Dtos;

public sealed class DictionaryEntrySearchItemMapping : Profile
{
    public DictionaryEntrySearchItemMapping()
    {
        CreateMap<Domain.Aggregates.DictionaryEntry.DictionaryEntry, DictionaryEntrySearchItem>();
    }
}