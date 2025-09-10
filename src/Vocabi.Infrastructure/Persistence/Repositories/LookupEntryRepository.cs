using Vocabi.Domain.Aggregates.LookupEntries;

namespace Vocabi.Infrastructure.Persistence.Repositories;

public class LookupEntryRepository(ApplicationDbContext context) : Repository<LookupEntry>(context), ILookupEntryRepository
{
    public async Task AddAsync(LookupEntry entity)
    {
        await DbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<LookupEntry> entities)
    {
        await DbSet.AddRangeAsync(entities);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await DbSet.FindAsync(id);
        if (entity != null)
        {
            DbSet.Remove(entity);
        }
    }
}
