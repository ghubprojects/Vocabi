namespace DictionaryService.Application.Features.SearchDictionaryEntries;

public sealed record DictionaryEntrySearchItem(
    Guid Id,
    string Headword,
    string PartOfSpeech
);