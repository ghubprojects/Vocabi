using BuildingBlocks.Application.Abstractions;

namespace VocabularyService.Application.UseCases.Vocabularies.GetVocabulary;

public sealed record GetVocabularyQuery(
    Guid Id
) : IQuery<GetVocabularyResult>;