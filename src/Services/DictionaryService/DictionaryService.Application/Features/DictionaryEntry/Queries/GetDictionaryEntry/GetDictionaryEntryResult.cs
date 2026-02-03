using DictionaryService.Application.Features.DictionaryEntry.Dtos;

namespace DictionaryService.Application.Features.DictionaryEntry.Queries.GetDictionaryEntry;

public sealed record GetDictionaryEntryResult(
    DictionaryEntryDetail? DictionaryEntry
);