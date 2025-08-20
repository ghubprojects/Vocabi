using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Vocabi.Domain.Aggregates.LookupEntries;
using Vocabi.Domain.Aggregates.MediaFiles;
using Vocabi.Domain.Aggregates.Vocabularies;
using Vocabi.Domain.SeedWork;

namespace Vocabi.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Vocabulary> Vocabularies { get; set; }
    public DbSet<LookupEntry> LookupEntries { get; set; }
    public DbSet<MediaFile> MediaFiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        // TODO: Dispatch domain events before saving changes
        await base.SaveChangesAsync(cancellationToken);
        return true;
    }
}