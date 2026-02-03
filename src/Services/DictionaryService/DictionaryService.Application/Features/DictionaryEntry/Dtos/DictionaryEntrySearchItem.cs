namespace DictionaryService.Application.Features.DictionaryEntry.Dtos;

public sealed record DictionaryEntrySearchItem(
    Guid Id,
    string Headword,
    string PartOfSpeech
);