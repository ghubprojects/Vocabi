namespace DictionaryService.Application.Features.SearchDictionaryEntries;

public sealed record SearchDictionaryEntriesResult(
    IReadOnlyList<DictionaryEntrySearchItem> Items
);

public sealed record DictionaryEntrySearchItem(
    Guid Id,
    string Headword,
    string PartOfSpeech
);