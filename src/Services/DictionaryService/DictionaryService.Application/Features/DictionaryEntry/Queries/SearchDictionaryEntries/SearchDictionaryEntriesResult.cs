using DictionaryService.Application.Features.DictionaryEntry.Dtos;

namespace DictionaryService.Application.Features.DictionaryEntry.Queries.SearchDictionaryEntries;

public sealed record SearchDictionaryEntriesResult(
    IReadOnlyList<DictionaryEntrySearchItem> DictionaryEntries
);