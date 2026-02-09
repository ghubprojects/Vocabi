using BuildingBlocks.Application.Abstractions;

namespace VocabularyService.Application.UseCases.Vocabularies.Queries.GetVocabulary;

public sealed record GetVocabularyQuery(
    Guid Id
) : IQuery<GetVocabularyResult>;