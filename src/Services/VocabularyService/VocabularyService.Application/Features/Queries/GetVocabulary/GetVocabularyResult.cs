using VocabularyService.Application.Features.Dtos;

namespace VocabularyService.Application.Features.Queries.GetVocabulary;

public sealed record GetVocabularyResult(
    VocabularyDetail? Vocabulary
);