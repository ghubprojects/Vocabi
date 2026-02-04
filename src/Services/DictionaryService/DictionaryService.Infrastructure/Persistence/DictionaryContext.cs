using BuildingBlocks.Domain.Abstractions;
using DictionaryService.Domain.Aggregates.DictionaryEntries.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DictionaryService.Infrastructure.Persistence;

public class DictionaryContext(DbContextOptions<DictionaryContext> options) : DbContext(options), IUnitOfWork
{
    internal DbSet<DictionaryEntry> DictionaryEntries { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Dictionary");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}