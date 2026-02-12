namespace VocabularyService.Application.UseCases.Vocabularies.GetVocabulary;

public sealed record GetVocabularyResult(
    VocabularyDetailDto? Vocabulary
);