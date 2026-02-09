using VocabularyService.Application.UseCases.Vocabularies.Dtos;

namespace VocabularyService.Application.UseCases.Vocabularies.Queries.GetVocabulary;

public sealed record GetVocabularyResult(
    VocabularyDetail? Vocabulary
);