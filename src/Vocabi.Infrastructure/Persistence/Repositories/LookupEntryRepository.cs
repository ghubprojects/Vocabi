using Vocabi.Domain.Aggregates.LookupEntries;
using Vocabi.Domain.SeedWork;

namespace Vocabi.Infrastructure.Persistence.Repositories;

public class LookupEntryRepository(ApplicationDbContext context) : ILookupEntryRepository
{
    public IUnitOfWork UnitOfWork => context;

    public IQueryable<LookupEntry> GetQueryableSet()
    {
        return context.Set<LookupEntry>();
    }

    public async Task AddAsync(LookupEntry entity)
    {
        await context.LookupEntries.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<LookupEntry> entities)
    {
        await context.LookupEntries.AddRangeAsync(entities);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await context.LookupEntries.FindAsync(id);
        if (entity != null)
            context.LookupEntries.Remove(entity);
    }
}
