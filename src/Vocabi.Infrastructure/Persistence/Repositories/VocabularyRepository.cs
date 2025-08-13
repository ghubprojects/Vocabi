using Microsoft.EntityFrameworkCore;
using Vocabi.Domain.Aggregates.Vocabularies;

namespace Vocabi.Infrastructure.Persistence.Repositories;

public class VocabularyRepository : Repository<Vocabulary>, IVocabularyRepository
{
    public VocabularyRepository(ApplicationDbContext context) : base(context) { }

    public async Task AddAsync(Vocabulary entity)
    {
        await DbSet.AddAsync(entity);
    }

    public void Update(Vocabulary entity)
    {
        DbSet.Update(entity);
    }

    public void Remove(Vocabulary entity)
    {
        DbSet.Remove(entity);
    }

    public async Task<bool> ExistsAsync(string word)
    {
        return await DbSet.AnyAsync(e => e.Word == word);
    }
}