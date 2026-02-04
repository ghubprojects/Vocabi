using DictionaryService.Application.UseCases.DictionaryEntries.Dtos;

namespace DictionaryService.Application.UseCases.DictionaryEntries.Queries.GetDictionaryEntry;

public sealed record GetDictionaryEntryResult(
    DictionaryEntryDetail? DictionaryEntry
);