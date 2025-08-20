using System.Text;
using System.Text.Json;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.External.Export;
using Vocabi.Domain.Aggregates.Vocabularies;

namespace Vocabi.Infrastructure.External.Export;

public class AnkiExporter(HttpClient httpClient) : IExporter
{
    public string ProviderName => "Anki";

    private const string ankiUrl = "http://127.0.0.1:8765";

    public async Task<Result> ExportAsync(IEnumerable<Vocabulary> vocabularies, CancellationToken cancellationToken = default)
    {
        if (vocabularies == null || !vocabularies.Any())
            return Result.Failure("No vocabulary items provided.");

        try
        {
            // build batch notes
            var notes = vocabularies.Select(v => new
            {
                deckName = "VocabDeck",
                modelName = "Basic",
                fields = new { Front = v.Word, Back = v.Definition },
                tags = new[] { "db-import" }
            }).ToList();

            var payload = new
            {
                action = "addNotes",
                version = 6,
                @params = new { notes }
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(ankiUrl, content, cancellationToken);
            response.EnsureSuccessStatusCode();

            var resultJson = await response.Content.ReadAsStringAsync(cancellationToken);
            using var doc = JsonDocument.Parse(resultJson);

            if (!doc.RootElement.TryGetProperty("result", out var result))
                return Result.Failure("Invalid response from AnkiConnect.");

            var failed = result.EnumerateArray()
                .Select((id, i) => (id, i))
                .Where(x => x.id.ValueKind == JsonValueKind.Null)
                .Select(x => vocabularies.ElementAt(x.i).Word)
                .ToList();

            return failed.Any()
                ? Result.Failure($"Some notes failed: {string.Join(", ", failed)}")
                : Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Export failed: {ex.Message}");
        }
    }
}