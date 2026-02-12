using BuildingBlocks.Infrastructure.Persistence.Abstractions;
using BuildingBlocks.Infrastructure.Persistence.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VocabularyService.Infrastructure.Configuration;

namespace VocabularyService.Infrastructure.Persistence.DataContext;

public class VocabularyContextInitializer(
    VocabularyContext context,
    IOptions<DatabaseOptions> options,
    IDatabaseSeeder seeder,
    ILogger<VocabularyContextInitializer> logger)
    : IDbContextInitializer
{
    private readonly DatabaseOptions _options = options.Value;

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Initializing Vocabulary DB with mode {Mode}", _options.InitMode);

        if (_options.InitMode == DatabaseInitMode.None)
        {
            logger.LogInformation("Database initialization skipped.");
            return;
        }

        var strategy = context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            switch (_options.InitMode)
            {
                case DatabaseInitMode.Migrate:
                    await context.Database.MigrateAsync(cancellationToken);
                    break;

                case DatabaseInitMode.Recreate:
                    await context.Database.EnsureDeletedAsync(cancellationToken);
                    await context.Database.MigrateAsync(cancellationToken);
                    break;

                case DatabaseInitMode.RecreateAndSeed:
                    await context.Database.EnsureDeletedAsync(cancellationToken);
                    await context.Database.MigrateAsync(cancellationToken);
                    await seeder.SeedAsync(cancellationToken);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        });

        logger.LogInformation("Database initialization completed successfully.");
    }
}