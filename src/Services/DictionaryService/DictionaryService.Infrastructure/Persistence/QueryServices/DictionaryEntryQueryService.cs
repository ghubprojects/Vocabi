using AutoMapper;
using AutoMapper.QueryableExtensions;
using DictionaryService.Application.Abstractions;
using DictionaryService.Application.UseCases.DictionaryEntries.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DictionaryService.Infrastructure.Persistence.QueryServices;

public sealed class DictionaryEntryQueryService(DictionaryContext context, IMapper mapper) : IDictionaryEntryQueryService
{
    public async Task<IReadOnlyList<DictionaryEntrySearchItem>> SearchAsync(string keyword, CancellationToken cancellationToken)
    {
        keyword = keyword.Trim();

        return await context.DictionaryEntries
            .AsNoTracking()
            .Where(e => EF.Functions.ILike(e.Headword, $"{keyword}%"))
            .OrderBy(e => e.Headword)
            .ProjectTo<DictionaryEntrySearchItem>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public async Task<DictionaryEntryDetail?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.DictionaryEntries
            .AsNoTracking()
            .Where(x => x.Id == id)
            .ProjectTo<DictionaryEntryDetail>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken);
    }
}