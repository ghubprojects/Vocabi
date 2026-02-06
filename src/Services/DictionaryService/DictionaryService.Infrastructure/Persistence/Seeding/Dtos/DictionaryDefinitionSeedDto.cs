namespace DictionaryService.Infrastructure.Persistence.Seeding.Dtos;

internal sealed record DictionaryDefinitionSeedDto(
    string Text,
    List<string> Examples
);