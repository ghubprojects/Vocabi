using DictionaryService.Application.UseCases.DictionaryEntries.Dtos;

namespace DictionaryService.Application.UseCases.DictionaryEntries.Queries.SearchDictionaryEntries;

public sealed record SearchDictionaryEntriesResult(
    IReadOnlyList<DictionaryEntrySearchItem> DictionaryEntries
);