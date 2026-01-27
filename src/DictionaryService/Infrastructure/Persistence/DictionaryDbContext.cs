using BuildingBlocks.Application.Abstractions;
using DictionaryService.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DictionaryService.Infrastructure.Persistence;

public class DictionaryDbContext(DbContextOptions<DictionaryDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<DictionaryEntry> DictionaryEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Dictionary");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}