using BuildingBlocks.Application.Abstractions;

namespace DictionaryService.Application.UseCases.DictionaryEntries.SearchDictionaryEntries;

public sealed record SearchDictionaryEntriesQuery(
    string Keyword
) : IQuery<SearchDictionaryEntriesResult>;