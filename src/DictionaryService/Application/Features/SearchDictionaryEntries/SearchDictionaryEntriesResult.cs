namespace DictionaryService.Application.Features.SearchDictionaryEntries;

public sealed record SearchDictionaryEntriesResult(
    IReadOnlyList<DictionaryEntrySearchItem> Items
);