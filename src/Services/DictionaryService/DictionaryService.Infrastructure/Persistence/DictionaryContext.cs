using DictionaryService.Application.Abstractions;
using DictionaryService.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DictionaryService.Infrastructure.Persistence;

public class DictionaryContext(DbContextOptions<DictionaryContext> options)
    : DbContext(options), IDictionaryReadContext, IDictionaryWriteContext
{
    public DbSet<DictionaryEntry> DictionaryEntries { get; private set; }
    IQueryable<DictionaryEntry> IDictionaryReadContext.DictionaryEntries => DictionaryEntries.AsNoTracking();

    public IQueryable<DictionaryDefinition> DictionarySenses => Set<DictionaryDefinition>().AsNoTracking();
    public IQueryable<DictionaryExample> DictionaryExamples => Set<DictionaryExample>().AsNoTracking();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Dictionary");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}