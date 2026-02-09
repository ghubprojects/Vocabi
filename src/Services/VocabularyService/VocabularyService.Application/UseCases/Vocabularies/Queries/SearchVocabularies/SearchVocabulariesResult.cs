using BuildingBlocks.Application.Models;
using VocabularyService.Application.UseCases.Vocabularies.Dtos;

namespace VocabularyService.Application.UseCases.Vocabularies.Queries.SearchVocabularies;

public sealed record SearchVocabulariesResult(
    PagedResult<VocabularySearchItem> Vocabularies
);