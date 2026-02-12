using BuildingBlocks.Application.Abstractions;

namespace DictionaryService.Application.UseCases.DictionaryEntries.GetDictionaryEntry;

public sealed record GetDictionaryEntryQuery(
    Guid Id
) : IQuery<GetDictionaryEntryResult>;