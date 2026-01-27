using BuildingBlocks.Domain.Abstractions;

namespace VocabularyService.Domain.Aggregates;

public interface IVocabularyRepository : IRepository<Vocabulary>
{
    Task<Vocabulary?> GetByIdAsync(Guid id);
    Task<List<Vocabulary>> GetByIdsAsync(IEnumerable<Guid> ids);
    void Add(Vocabulary entity);
    void Remove(Vocabulary entity);
}