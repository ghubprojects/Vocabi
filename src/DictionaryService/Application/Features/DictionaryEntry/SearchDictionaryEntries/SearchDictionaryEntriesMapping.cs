using AutoMapper;

namespace DictionaryService.Application.Features.DictionaryEntry.SearchDictionaryEntries;

public sealed class SearchDictionaryEntriesMapping : Profile
{
    public SearchDictionaryEntriesMapping()
    {
        CreateMap<Domain.Aggregates.DictionaryEntry, SearchDictionaryEntriesResult.DictionaryEntrySearchItem>();
    }
}