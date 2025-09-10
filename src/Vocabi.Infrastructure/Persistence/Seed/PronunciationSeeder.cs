using System.Text.Json;
using Vocabi.Domain.Entities.Pronunciations;

namespace Vocabi.Infrastructure.Persistence.Seed;

public class PronunciationSeeder(IPronunciationRepository pronunciationRepository)
{
    public async Task SeedAsync(string seedFilePath)
    {
        if (await pronunciationRepository.IsAnyAsync())
            return;

        if (!File.Exists(seedFilePath))
            return;

        var json = await File.ReadAllTextAsync(seedFilePath);
        var data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

        if (data is null || data.Count == 0)
            return;

        var entities = data.Select(x => Pronunciation.CreateNew(x.Key, x.Value));
        await pronunciationRepository.AddRangeAsync(entities);
        await pronunciationRepository.UnitOfWork.SaveChangesAsync();
    }
}
