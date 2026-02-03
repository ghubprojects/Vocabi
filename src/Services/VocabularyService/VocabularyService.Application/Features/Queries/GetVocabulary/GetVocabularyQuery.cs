using BuildingBlocks.Application.Abstractions;

namespace VocabularyService.Application.Features.Queries.GetVocabulary;

public sealed record GetVocabularyQuery(
    Guid Id
) : IQuery<GetVocabularyResult>;