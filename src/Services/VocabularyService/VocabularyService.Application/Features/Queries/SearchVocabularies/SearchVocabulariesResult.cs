using BuildingBlocks.Application.Models;
using VocabularyService.Application.Features.Dtos;

namespace VocabularyService.Application.Features.Queries.SearchVocabularies;

public sealed record SearchVocabulariesResult(
    PagedResult<VocabularySearchItem> Vocabularies
);