using AutoMapper;
using AutoMapper.QueryableExtensions;
using DictionaryService.Application.Abstractions;
using DictionaryService.Application.Features.DictionaryEntry.SearchDictionaryEntries;
using DictionaryService.Application.Features.SearchDictionaryEntries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DictionaryService.Application.Features.DictionaryEntry.GetDictionaryEntry;

public sealed class GetDictionaryEntryHandler(IDictionaryReadContext context, IMapper mapper)
    : IRequestHandler<GetDictionaryEntryQuery, GetDictionaryEntryResult>
{
    public async Task<GetDictionaryEntryResult> Handle(GetDictionaryEntryQuery request, CancellationToken cancellationToken)
    {
        var item = await context.DictionaryEntries
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .ProjectTo<GetDictionaryEntryResult>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken);

        var items = await context.DictionaryEntries
            .Where(e => EF.Functions.ILike(e.Headword, $"{keyword}%"))
            .OrderBy(e => e.Headword)
            .ProjectTo<DictionaryEntrySearchItem>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new SearchDictionaryEntriesResult(items);
    }
}