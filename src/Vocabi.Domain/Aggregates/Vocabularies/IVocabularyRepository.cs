using Vocabi.Domain.SeedWork;

namespace Vocabi.Domain.Aggregates.Vocabularies;

public interface IVocabularyRepository : IRepository<Vocabulary>
{
    Task AddAsync(Vocabulary entry);
    void Update(Vocabulary entry);
    void Remove(Vocabulary entry);
    Task<bool> ExistsAsync(string word);
}