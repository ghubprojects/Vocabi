using BuildingBlocks.Application.Abstractions;
using DictionaryService.Application.Abstractions;

namespace DictionaryService.Application.UseCases.DictionaryEntries.Queries.SearchDictionaryEntries;

public sealed class SearchDictionaryEntriesHandler(IDictionaryEntryQueryService queryService)
    : IQueryHandler<SearchDictionaryEntriesQuery, SearchDictionaryEntriesResult>
{
    public async Task<SearchDictionaryEntriesResult> Handle(SearchDictionaryEntriesQuery request, CancellationToken cancellationToken)
    {
        var items = await queryService.SearchAsync(request.Keyword, cancellationToken);

        return new SearchDictionaryEntriesResult(items);
    }
}