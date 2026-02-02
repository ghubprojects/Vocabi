using BuildingBlocks.Application.Abstractions;

namespace DictionaryService.Application.Features.DictionaryEntry.SearchDictionaryEntries;

public sealed record SearchDictionaryEntriesQuery(
    string Keyword
) : IQuery<SearchDictionaryEntriesResult>;