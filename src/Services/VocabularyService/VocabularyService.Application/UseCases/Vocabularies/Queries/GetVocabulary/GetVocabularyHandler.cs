using BuildingBlocks.Application.Abstractions;
using VocabularyService.Application.Abstractions;

namespace VocabularyService.Application.UseCases.Vocabularies.Queries.GetVocabulary;

public sealed class GetVocabularyHandler(IVocabularyQueryService queryService)
    : IQueryHandler<GetVocabularyQuery, GetVocabularyResult>
{
    public async Task<GetVocabularyResult> Handle(GetVocabularyQuery request, CancellationToken cancellationToken)
    {
        var vocabulary = await queryService.GetByIdAsync(request.Id, cancellationToken);

        return new GetVocabularyResult(vocabulary);
    }
}