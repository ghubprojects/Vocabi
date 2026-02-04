using BuildingBlocks.Application.Abstractions;

namespace DictionaryService.Application.UseCases.DictionaryEntries.Queries.SearchDictionaryEntries;

public sealed record SearchDictionaryEntriesQuery(
    string Keyword
) : IQuery<SearchDictionaryEntriesResult>;