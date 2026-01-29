namespace DictionaryService.Application.Features.DictionaryEntry.SearchDictionaryEntries;

public sealed record SearchDictionaryEntriesResult
{
    public IReadOnlyList<DictionaryEntrySearchItem> Items { get; init; } = [];

    public sealed record DictionaryEntrySearchItem(
        Guid Id,
        string Headword,
        string PartOfSpeech
    );
}