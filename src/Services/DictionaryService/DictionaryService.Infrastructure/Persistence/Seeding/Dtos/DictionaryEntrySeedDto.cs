namespace DictionaryService.Infrastructure.Persistence.Seeding.Dtos;

internal sealed record DictionaryEntrySeedDto(
    string Headword,
    string PartOfSpeech,
    string Pronunciation,
    string Source,
    List<DictionaryDefinitionSeedDto> Definitions
);