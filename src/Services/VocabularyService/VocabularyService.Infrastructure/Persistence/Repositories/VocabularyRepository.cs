using BuildingBlocks.Domain.Abstractions;
using VocabularyService.Domain.Aggregates;
using VocabularyService.Infrastructure.Persistence.DataContext;

namespace VocabularyService.Infrastructure.Persistence.Repositories;

public class VocabularyRepository(VocabularyContext context) : IVocabularyRepository
{
    public IUnitOfWork UnitOfWork => context;

    public void Add(Vocabulary entity)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(Vocabulary entity)
    {
        await context.Vocabularies.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<Vocabulary> entities)
    {
        await context.Vocabularies.AddRangeAsync(entities);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await context.Vocabularies.FindAsync(id);
        if (entity != null)
            context.Vocabularies.Remove(entity);
    }

    public Task<Vocabulary?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Vocabulary>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        throw new NotImplementedException();
    }

    public void Remove(Vocabulary entity)
    {
        throw new NotImplementedException();
    }
}