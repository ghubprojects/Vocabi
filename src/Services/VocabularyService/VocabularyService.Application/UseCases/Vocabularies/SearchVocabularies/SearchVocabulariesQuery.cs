using BuildingBlocks.Application.Abstractions;

namespace VocabularyService.Application.UseCases.Vocabularies.SearchVocabularies;

public sealed record SearchVocabulariesQuery(
    string Keyword,
    int PageIndex,
    int PageSize
) : IQuery<SearchVocabulariesResult>;