using Vocabi.Domain.SeedWork;

namespace Vocabi.Domain.Aggregates.Vocabularies;

public interface IVocabularyRepository : IRepository<Vocabulary>
{
    Task<Vocabulary?> GetByIdAsync(Guid id);
    Task<List<Vocabulary>> GetByIdsAsync(IEnumerable<Guid> ids);
    Task AddAsync(Vocabulary entry);
}