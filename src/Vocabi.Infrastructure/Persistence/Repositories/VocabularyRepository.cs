using Microsoft.EntityFrameworkCore;
using Vocabi.Domain.Aggregates.Vocabularies;

namespace Vocabi.Infrastructure.Persistence.Repositories;

public class VocabularyRepository(ApplicationDbContext context) : Repository<Vocabulary>(context), IVocabularyRepository
{
    public async Task<Vocabulary?> GetByIdAsync(Guid id)
    {
        return await DbSet
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IReadOnlyList<Vocabulary>> GetByIdsAsync(IReadOnlyCollection<Guid> ids)
    {
        return await DbSet
            .Where(e => ids.Contains(e.Id))
            .ToListAsync();
    }

    public async Task AddAsync(Vocabulary entity)
    {
        await DbSet.AddAsync(entity);
    }

    public void Remove(Vocabulary entity)
    {
        DbSet.Remove(entity);
    }
}