using BuildingBlocks.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VocabularyService.Domain.Aggregates;

namespace VocabularyService.Infrastructure.Persistence;

public class VocabularyDbContext(DbContextOptions<VocabularyDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Vocabulary> Vocabularies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Vocabulary");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}