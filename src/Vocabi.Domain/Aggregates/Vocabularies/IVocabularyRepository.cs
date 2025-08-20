using Vocabi.Domain.SeedWork;

namespace Vocabi.Domain.Aggregates.Vocabularies;

public interface IVocabularyRepository : IRepository<Vocabulary>
{
    Task<Vocabulary?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Vocabulary>> GetByIdsAsync(IReadOnlyCollection<Guid> ids);
    Task AddAsync(Vocabulary entry);
}