using Microsoft.EntityFrameworkCore;
using Vocabi.Domain.Aggregates.Pronunciations;
using Vocabi.Domain.SeedWork;

namespace Vocabi.Infrastructure.Persistence.Repositories;

public class PronunciationRepository(AppDbContext context) : IPronunciationRepository
{
    public IUnitOfWork UnitOfWork => context;

    public IQueryable<Pronunciation> GetQueryableSet()
    {
        return context.Set<Pronunciation>();
    }

    public async Task<Pronunciation?> GetAsync(string word)
    {
        return await context.Pronunciations
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Word == word);
    }
    public async Task<bool> IsAnyAsync()
    {
        return await context.Pronunciations.AnyAsync();
    }

    public async Task AddAsync(Pronunciation entity)
    {
        await context.Pronunciations.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<Pronunciation> entities)
    {
        await context.Pronunciations.AddRangeAsync(entities);
    }
}
