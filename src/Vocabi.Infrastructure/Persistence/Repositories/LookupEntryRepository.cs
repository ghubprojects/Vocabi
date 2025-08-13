using Vocabi.Domain.Aggregates.LookupEntries;

namespace Vocabi.Infrastructure.Persistence.Repositories;

public class LookupEntryRepository: Repository<LookupEntry>, ILookupEntryRepository
{
    public LookupEntryRepository(ApplicationDbContext context) : base(context) { }

    public async Task AddAsync(LookupEntry entity)
    {
        await DbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(List<LookupEntry> entities)
    {
        await DbSet.AddRangeAsync(entities);
    }

    public async Task UpdateAsync(LookupEntry entity)
    {
        DbSet.Update(entity);
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
