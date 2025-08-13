using Vocabi.Domain.SeedWork;

namespace Vocabi.Domain.Aggregates.LookupEntries;

public interface ILookupEntryRepository : IRepository<LookupEntry>
{
    Task AddAsync(LookupEntry entity);
    Task AddRangeAsync(List<LookupEntry> entities);
    Task UpdateAsync(LookupEntry entity);
    Task DeleteAsync(Guid id);
}