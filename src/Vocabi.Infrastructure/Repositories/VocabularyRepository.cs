using Microsoft.EntityFrameworkCore;
using Vocabi.Domain.Aggregates.Vocabularies;

namespace Vocabi.Infrastructure.Repositories;

public class VocabularyRepository : Repository<Vocabulary>, IVocabularyRepository
{
    public VocabularyRepository(ApplicationDbContext context) : base(context) { }

    public async Task AddAsync(Vocabulary entry)
    {
        await DbSet.AddAsync(entry);
    }

    public void Update(Vocabulary entry)
    {
        DbSet.Update(entry);
    }

    public void Remove(Vocabulary entry)
    {
        DbSet.Remove(entry);
    }

    public async Task<bool> ExistsAsync(string word)
    {
        return await DbSet.AnyAsync(e => e.Word == word);
    }
}