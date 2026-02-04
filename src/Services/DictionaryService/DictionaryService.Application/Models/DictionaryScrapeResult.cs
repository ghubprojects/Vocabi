namespace DictionaryService.Application.Models;

public sealed record DictionaryScrapeResult(
    IReadOnlyList<ScrapedDictionaryEntry> Entries
);
