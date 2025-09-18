using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vocabi.Application.Common.Extensions;
using Vocabi.Application.Common.Models;
using Vocabi.Domain.Aggregates.Vocabularies;

namespace Vocabi.Application.Features.Vocabularies.GetPaginatedFailedVocabularies;

public class GetPaginatedFailedVocabulariesQuery : IRequest<PaginatedData<FailedVocabularyDto>>
{
    public string SearchWord { get; init; } = string.Empty;
    public int PageIndex { get; init; } = 0;
    public int PageSize { get; init; } = 10;
}

public class GetPaginatedFailedVocabulariesQueryHandler(
    IVocabularyRepository vocabularyRepository,
    IMapper mapper
    ) : IRequestHandler<GetPaginatedFailedVocabulariesQuery, PaginatedData<FailedVocabularyDto>>
{
    public async Task<PaginatedData<FailedVocabularyDto>> Handle(GetPaginatedFailedVocabulariesQuery request, CancellationToken cancellationToken)
    {
        var query = vocabularyRepository
            .GetQueryableSet()
            .AsNoTracking()
            .Where(x => x.Flashcard != null && x.Flashcard.Status == FlashcardStatus.Failed);

        if (!string.IsNullOrEmpty(request.SearchWord))
            query = query.Where(x => EF.Functions.ILike(x.Word, $"%{request.SearchWord}%"));

        query = query.OrderByDescending(x => x.Flashcard.LastTriedAt);

        return await query.ProjectToPaginatedDataAsync<Vocabulary, FailedVocabularyDto>(
            request.PageIndex,
            request.PageSize,
            mapper.ConfigurationProvider,
            cancellationToken);
    }
}