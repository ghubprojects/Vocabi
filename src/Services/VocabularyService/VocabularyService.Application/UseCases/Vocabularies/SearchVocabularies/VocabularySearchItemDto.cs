namespace VocabularyService.Application.UseCases.Vocabularies.SearchVocabularies;

public sealed record VocabularySearchItemDto(
   Guid Id,
   string Headword,
   string PartOfSpeech,
   string Translation,
   DateTimeOffset CreatedAt
);