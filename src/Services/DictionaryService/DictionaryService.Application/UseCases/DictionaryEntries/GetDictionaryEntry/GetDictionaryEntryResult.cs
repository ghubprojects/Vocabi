namespace DictionaryService.Application.UseCases.DictionaryEntries.GetDictionaryEntry;

public sealed record GetDictionaryEntryResult(
    DictionaryEntryDetailDto? DictionaryEntry
);