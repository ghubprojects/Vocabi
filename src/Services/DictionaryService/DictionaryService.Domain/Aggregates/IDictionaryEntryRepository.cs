using BuildingBlocks.Domain.Abstractions;

namespace DictionaryService.Domain.Aggregates;

public interface IDictionaryEntryRepository : IRepository<DictionaryEntry>
{
    Task AddAsync(DictionaryEntry entity);
    Task AddRangeAsync(IEnumerable<DictionaryEntry> entities);
}