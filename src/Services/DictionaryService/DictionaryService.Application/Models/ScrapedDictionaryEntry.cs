namespace DictionaryService.Application.Models;

public sealed record ScrapedDictionaryEntry(
    string Headword,
    string PartOfSpeech,
    string Pronunciation,
    string Source,
    IReadOnlyList<ScrapedDictionaryDefinition> Definitions
);