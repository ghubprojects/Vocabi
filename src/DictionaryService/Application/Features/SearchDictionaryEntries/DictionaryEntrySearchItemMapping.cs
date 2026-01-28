using AutoMapper;
using DictionaryService.Domain.Aggregates;

namespace DictionaryService.Application.Features.SearchDictionaryEntries;

public sealed class DictionaryEntrySearchItemMapping : Profile
{
    public DictionaryEntrySearchItemMapping()
    {
        CreateMap<DictionaryEntry, DictionaryEntrySearchItem>();
    }
}