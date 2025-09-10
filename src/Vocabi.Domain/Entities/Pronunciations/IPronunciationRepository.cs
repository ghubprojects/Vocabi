using Vocabi.Domain.SeedWork;

namespace Vocabi.Domain.Entities.Pronunciations;

public interface IPronunciationRepository : IRepository<Pronunciation>
{
    Task<Pronunciation?> GetAsync(string word);
    Task<bool> IsAnyAsync();
    Task AddAsync(Pronunciation entity);
    Task AddRangeAsync(IEnumerable<Pronunciation> entities);
}