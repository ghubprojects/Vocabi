namespace VocabularyService.Infrastructure.Persistence.Seeding.Dtos;

internal sealed record VocabularySeedDto(
    string Word,
    string PartOfSpeech,
    string Pronunciation,
    string Cloze,
    string Definition,
    string Translation,
    List<string> Examples
);