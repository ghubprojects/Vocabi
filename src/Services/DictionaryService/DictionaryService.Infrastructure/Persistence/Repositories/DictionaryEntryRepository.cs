using BuildingBlocks.Domain.Abstractions;
using DictionaryService.Domain.Aggregates.DictionaryEntry;

namespace DictionaryService.Infrastructure.Persistence.Repositories;

public class DictionaryEntryRepository(DictionaryContext context) : IDictionaryEntryRepository
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