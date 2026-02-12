using BuildingBlocks.Infrastructure.Persistence.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using VocabularyService.Domain.Aggregates;
using VocabularyService.Infrastructure.Persistence.DataContext;
using VocabularyService.Infrastructure.Persistence.Seeding.Dtos;

namespace VocabularyService.Infrastructure.Persistence.Seeding;

public class VocabularySeeder(
    VocabularyContext context,
    ILogger<VocabularySeeder> logger)
    : IDatabaseSeeder
{
    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task SeedAsync()
    {
        logger.LogInformation("Seeding Vocabulary data...");

        var seeds = GetSeedDataFromResource<VocabularySeedDto>();

        foreach (var seed in seeds)
        {
            var vocabulary = Vocabulary.Create(
                seed.Word,
                seed.PartOfSpeech,
                seed.Pronunciation,
                seed.Cloze,
                seed.Definition,
                seed.Translation);

            foreach (var example in seed.Examples)
                vocabulary.AddExample(example);

            context.Vocabularies.Add(vocabulary);
        }

        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        logger.LogInformation("Vocabulary seeding completed.");
    }

    private static List<T> GetSeedDataFromResource<T>()
    {
        var resourceDirectoryName = $"{typeof(VocabularySeeder).Namespace}.Resources";

        var typeName = typeof(T).Name;
        if (typeName.EndsWith("SeedDto"))
            typeName = typeName[..^"SeedDto".Length];

        var resourceName = $"{resourceDirectoryName}.{typeName}.json";

        using var stream = typeof(VocabularySeeder).Assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Resource not found: {resourceName}");

        using var reader = new StreamReader(stream);

        return JsonSerializer.Deserialize<List<T>>(reader.ReadToEnd(), jsonOptions) ?? [];
    }
}