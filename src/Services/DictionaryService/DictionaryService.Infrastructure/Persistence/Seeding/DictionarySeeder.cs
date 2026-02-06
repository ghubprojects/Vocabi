using BuildingBlocks.Infrastructure.Persistence.Abstractions;
using DictionaryService.Domain.Aggregates.DictionaryEntries;
using DictionaryService.Infrastructure.Persistence.Seeding.Dtos;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DictionaryService.Infrastructure.Persistence.Seeders;

public class DictionarySeeder(
    DictionaryContext context,
    DictionaryEntryDomainService domainService,
    ILogger<DictionarySeeder> logger)
    : IDatabaseSeeder
{
    public async Task SeedAsync()
    {
        logger.LogInformation("Seeding Dictionary data...");

        var entrySeeds = GetSeedDataFromResource<DictionaryEntrySeedDto>();

        foreach (var entrySeed in entrySeeds)
        {
            var entry = await domainService.CreateAsync(
                entrySeed.Headword,
                entrySeed.PartOfSpeech,
                entrySeed.Pronunciation,
                entrySeed.Source);

            foreach (var definitionSeed in entrySeed.Definitions)
                entry.AddDefinition(definitionSeed.Text, definitionSeed.Examples);

            context.DictionaryEntries.Add(entry);
        }

        await context.SaveChangesAsync();

        logger.LogInformation("Dictionary seeding completed.");

        context.ChangeTracker.Clear();
    }

    private static List<T> GetSeedDataFromResource<T>()
    {
        var resourceDirectoryName = $"{typeof(DictionarySeeder).Namespace}.Resources";
        var resourceName = $"{resourceDirectoryName}.{typeof(T).Name}.json";

        using var stream = typeof(DictionarySeeder).Assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Resource not found: {resourceName}");

        using var reader = new StreamReader(stream);

        return JsonSerializer.Deserialize<List<T>>(reader.ReadToEnd()) ?? [];
    }
}
