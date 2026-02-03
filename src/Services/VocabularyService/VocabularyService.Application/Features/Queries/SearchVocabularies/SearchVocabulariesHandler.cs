using BuildingBlocks.Application.Abstractions;
using VocabularyService.Application.Abstractions;

namespace VocabularyService.Application.Features.Queries.SearchVocabularies;

public sealed class SearchVocabulariesHandler(IVocabularyQueryService queryService)
    : IQueryHandler<SearchVocabulariesQuery, SearchVocabulariesResult>
{
    public async Task<SearchVocabulariesResult> Handle(SearchVocabulariesQuery request, CancellationToken cancellationToken)
    {
        var vocabularies = await queryService.SearchAsync(
            request.Keyword,
            request.PageIndex,
            request.PageSize,
            cancellationToken);

        return new SearchVocabulariesResult(vocabularies);
    }
}