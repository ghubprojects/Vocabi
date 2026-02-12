using BuildingBlocks.Application.Models;

namespace VocabularyService.Application.UseCases.Vocabularies.SearchVocabularies;

public sealed record SearchVocabulariesResult(
    PagedResult<VocabularySearchItemDto> Vocabularies
);