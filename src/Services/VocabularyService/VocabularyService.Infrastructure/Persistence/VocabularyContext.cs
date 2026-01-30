using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VocabularyService.Application.Abstractions;
using VocabularyService.Domain.Aggregates;

namespace VocabularyService.Infrastructure.Persistence;

public class VocabularyContext(DbContextOptions<VocabularyContext> options)
    : DbContext(options), IVocabularyReadContext, IVocabularyWriteContext
{
    public DbSet<Vocabulary> Vocabularies { get; private set; }
    IQueryable<Vocabulary> IVocabularyReadContext.Vocabularies => Vocabularies.AsNoTracking();

    public IQueryable<VocabularyExample> VocabularyExamples => Set<VocabularyExample>().AsNoTracking();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Vocabulary");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}