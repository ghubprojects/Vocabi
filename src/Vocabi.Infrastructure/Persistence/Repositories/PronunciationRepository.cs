using Microsoft.EntityFrameworkCore;
using Vocabi.Domain.Entities.Pronunciations;

namespace Vocabi.Infrastructure.Persistence.Repositories;

public class PronunciationRepository(ApplicationDbContext context) : Repository<Pronunciation>(context), IPronunciationRepository
{
    public async Task<Pronunciation?> GetAsync(string word)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Word == word);
    }
    public async Task<bool> IsAnyAsync()
    {
        return await DbSet.AnyAsync();
    }

    public async Task AddAsync(Pronunciation entity)
    {
        await DbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<Pronunciation> entities)
    {
        await DbSet.AddRangeAsync(entities);
    }
}
