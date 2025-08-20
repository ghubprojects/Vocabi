using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vocabi.Application.Common.Extensions;
using Vocabi.Application.Common.Models;
using Vocabi.Domain.Aggregates.Vocabularies;

namespace Vocabi.Application.Features.Vocabularies.GetPaginatedPendingVocabularies;

public class GetPaginatedPendingVocabulariesQuery : IRequest<PaginatedData<PendingVocabularyDto>>
{
    public string SearchWord { get; init; } = string.Empty;
    public int PageIndex { get; init; } = 0;
    public int PageSize { get; init; } = 10;
}

public class GetPaginatedPendingVocabulariesQueryHandler(
    IVocabularyRepository vocabularyRepository,
    IMapper mapper
    ) : IRequestHandler<GetPaginatedPendingVocabulariesQuery, PaginatedData<PendingVocabularyDto>>
{
    public async Task<PaginatedData<PendingVocabularyDto>> Handle(GetPaginatedPendingVocabulariesQuery request, CancellationToken cancellationToken)
    {
        var query = vocabularyRepository.DbSet
            .AsNoTracking()
            .Where(x => x.Flashcard == null || x.Flashcard.Status == FlashcardStatus.Pending);

        if (!string.IsNullOrEmpty(request.SearchWord))
            query = query.Where(x => EF.Functions.ILike(x.Word, $"%{request.SearchWord}%"));

        query = query.OrderByDescending(x => x.CreatedAt);

        return await query.ProjectToPaginatedDataAsync<Vocabulary, PendingVocabularyDto>(
            request.PageIndex,
            request.PageSize,
            mapper.ConfigurationProvider,
            cancellationToken);
    }
}