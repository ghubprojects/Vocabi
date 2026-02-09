using BuildingBlocks.Infrastructure.Persistence.Abstractions;
using BuildingBlocks.Infrastructure.Persistence.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VocabularyService.Infrastructure.Configuration;

namespace VocabularyService.Infrastructure.Persistence;

public class VocabularyContextInitializer(
    VocabularyContext context,
    IOptions<DatabaseOptions> options,
    IDatabaseSeeder seeder,
    ILogger<VocabularyContextInitializer> logger)
    : IDbContextInitializer
{
    private readonly DatabaseOptions _options = options.Value;

    public async Task InitializeAsync()
    {
        logger.LogInformation("Initializing Vocabulary DB with mode {Mode}", _options.InitMode);

        switch (_options.InitMode)
        {
            case DatabaseInitMode.None:
                return;

            case DatabaseInitMode.Migrate:
                await context.Database.MigrateAsync();
                break;

            case DatabaseInitMode.Recreate:
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                break;

            case DatabaseInitMode.RecreateAndSeed:
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                await seeder.SeedAsync();
                break;
        }
    }
}