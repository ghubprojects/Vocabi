using BuildingBlocks.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VocabularyService.Domain.Aggregates;

namespace VocabularyService.Infrastructure.Persistence.DataContext;

public class VocabularyContext(DbContextOptions<VocabularyContext> options) : DbContext(options), IUnitOfWork
{
    internal DbSet<Vocabulary> Vocabularies { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Vocabulary");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}