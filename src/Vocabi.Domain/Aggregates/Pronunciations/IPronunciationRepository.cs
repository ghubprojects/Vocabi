using Vocabi.Domain.SeedWork;

namespace Vocabi.Domain.Aggregates.Pronunciations;

public interface IPronunciationRepository : IRepository<Pronunciation>
{
    Task<Pronunciation?> GetAsync(string word);
    Task<bool> IsAnyAsync();
    Task AddAsync(Pronunciation entity);
    Task AddRangeAsync(IEnumerable<Pronunciation> entities);
}