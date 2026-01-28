using DictionaryService.Application.Abstractions;
using MediatR;

namespace DictionaryService.Application.Features.SearchDictionaryEntries;

public sealed class SearchDictionaryEntriesHandler(IDictionaryReadContext context) : IRequestHandler<SearchDictionaryEntriesQuery, SearchDictionaryEntriesResult>
{
    public async Task<SearchDictionaryEntriesResult> Handle(SearchDictionaryEntriesQuery request, CancellationToken cancellationToken)
    {
        var keyword = request.Keyword.Trim();

        var items = await _db.DictionaryEntries
            .AsNoTracking()
            .Where(e =>
                e.Headword.StartsWith(keyword)) // prefix search (fast, index-friendly)
            .OrderBy(e => e.Headword)
            .Take(request.Limit)
            .Select(e => new DictionaryEntrySearchItem(
                e.Id,
                e.Headword,
                e.PartOfSpeech,
                e.Pronunciation
            ))
            .ToListAsync(ct);

        return new SearchDictionaryEntriesResult(items);
    }
}
