namespace DictionaryService.Application.UseCases.DictionaryEntries.Dtos;

public sealed record DictionaryEntrySearchItem(
    Guid Id,
    string Headword,
    string PartOfSpeech
);