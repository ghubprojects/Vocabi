using BuildingBlocks.Application.Abstractions;

namespace DictionaryService.Application.Features.DictionaryEntry.GetDictionaryEntry;

public sealed record GetDictionaryEntryQuery(
    Guid Id
) : IQuery<GetDictionaryEntryResult>;