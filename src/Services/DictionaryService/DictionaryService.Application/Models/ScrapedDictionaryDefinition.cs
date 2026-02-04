namespace DictionaryService.Application.Models;

public sealed record ScrapedDictionaryDefinition(
    string Text,
    IReadOnlyList<string> Examples
);