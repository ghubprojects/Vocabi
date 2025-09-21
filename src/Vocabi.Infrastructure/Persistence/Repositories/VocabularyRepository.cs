using Microsoft.EntityFrameworkCore;
using Vocabi.Domain.Aggregates.Vocabularies;
using Vocabi.Domain.SeedWork;

namespace Vocabi.Infrastructure.Persistence.Repositories;

public class VocabularyRepository(ApplicationDbContext context) : IVocabularyRepository
{
    public IUnitOfWork UnitOfWork => context;

    public IQueryable<Vocabulary> GetQueryableSet()
    {
        return context.Set<Vocabulary>();
    }

    public async Task<Vocabulary?> GetByIdAsync(Guid id)
    {
        return await context.Vocabularies
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<Vocabulary>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        return await context.Vocabularies
            .Where(e => ids.Contains(e.Id))
            .ToListAsync();
    }

    public async Task AddAsync(Vocabulary entity)
    {
        await context.Vocabularies.AddAsync(entity);
    }

    public void Remove(Vocabulary entity)
    {
        context.Vocabularies.Remove(entity);
    }
}