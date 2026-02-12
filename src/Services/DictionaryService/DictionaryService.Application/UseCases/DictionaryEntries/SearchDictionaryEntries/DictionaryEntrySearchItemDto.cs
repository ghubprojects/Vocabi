namespace DictionaryService.Application.UseCases.DictionaryEntries.SearchDictionaryEntries;

public sealed record DictionaryEntrySearchItemDto(
    Guid Id,
    string Headword,
    string PartOfSpeech
);