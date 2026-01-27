using BuildingBlocks.Application.Abstractions;
using DictionaryService.Application.Abstractions;
using DictionaryService.Domain.Aggregates;

namespace DictionaryService.Infrastructure.Repositories;

public class DictionaryEntryRepository(IDictionaryWriteContext context) : IDictionaryEntryRepository
{
    public IUnitOfWork UnitOfWork => context;

    public async Task AddAsync(DictionaryEntry entity)
    {
        await context.DictionaryEntries.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<DictionaryEntry> entities)
    {
        await context.DictionaryEntries.AddRangeAsync(entities);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await context.DictionaryEntries.FindAsync(id);
        if (entity != null)
            context.DictionaryEntries.Remove(entity);
    }
}