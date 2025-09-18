using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Vocabi.Domain.Aggregates.LookupEntries;
using Vocabi.Domain.Aggregates.MediaFiles;
using Vocabi.Domain.Aggregates.Pronunciations;
using Vocabi.Domain.Aggregates.Vocabularies;
using Vocabi.Domain.SeedWork;
using Vocabi.Infrastructure.Persistence.Extensions;

namespace Vocabi.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator) : DbContext(options), IUnitOfWork
{
    public DbSet<Vocabulary> Vocabularies { get; set; }
    public DbSet<LookupEntry> LookupEntries { get; set; }
    public DbSet<MediaFile> MediaFiles { get; set; }
    public DbSet<Pronunciation> Pronunciations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await mediator.DispatchDomainEventsAsync(this);
        await base.SaveChangesAsync(cancellationToken);
        return true;
    }
}