using AutoMapper;
using AutoMapper.QueryableExtensions;
using DictionaryService.Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DictionaryService.Application.Features.SearchDictionaryEntries;

public sealed class SearchDictionaryEntriesHandler(IDictionaryReadContext context, IMapper mapper)
    : IRequestHandler<SearchDictionaryEntriesQuery, SearchDictionaryEntriesResult>
{
    public async Task<SearchDictionaryEntriesResult> Handle(SearchDictionaryEntriesQuery request, CancellationToken cancellationToken)
    {
        var keyword = request.Keyword.Trim();

        var items = await context.DictionaryEntries
            .Where(e => EF.Functions.ILike(e.Headword, $"{keyword}%"))
            .OrderBy(e => e.Headword)
            .ProjectTo<DictionaryEntrySearchItem>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new SearchDictionaryEntriesResult(items);
    }
}