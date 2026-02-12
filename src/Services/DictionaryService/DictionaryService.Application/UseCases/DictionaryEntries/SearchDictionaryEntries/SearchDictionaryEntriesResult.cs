namespace DictionaryService.Application.UseCases.DictionaryEntries.SearchDictionaryEntries;

public sealed record SearchDictionaryEntriesResult(
    IReadOnlyList<DictionaryEntrySearchItemDto> DictionaryEntries
);