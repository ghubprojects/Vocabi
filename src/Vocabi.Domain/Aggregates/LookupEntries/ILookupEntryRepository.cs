using Vocabi.Domain.SeedWork;

namespace Vocabi.Domain.Aggregates.LookupEntries;

public interface ILookupEntryRepository : IRepository<LookupEntry>
{
    Task AddAsync(LookupEntry entity);
    Task AddRangeAsync(IEnumerable<LookupEntry> entities);
}