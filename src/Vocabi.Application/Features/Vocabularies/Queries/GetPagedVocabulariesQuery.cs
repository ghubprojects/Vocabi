using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vocabi.Application.Common.Extensions;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Features.Vocabularies.DTOs;
using Vocabi.Domain.Aggregates.Vocabularies;

namespace Vocabi.Application.Features.Vocabularies.Queries;

public class GetPagedVocabulariesQuery : IRequest<PagedResult<VocabularyDto>>
{
    public string SearchWord { get; init; } = string.Empty;
    public ExportStatus Status { get; init; } = ExportStatus.Pending;
    public int PageIndex { get; init; } = 0;
    public int PageSize { get; init; } = 10;
}

public class GetPagedVocabulariesQueryHandler(
    IVocabularyRepository vocabularyRepository,
    IMapper mapper
    ) : IRequestHandler<GetPagedVocabulariesQuery, PagedResult<VocabularyDto>>
{
    public async Task<PagedResult<VocabularyDto>> Handle(GetPagedVocabulariesQuery request, CancellationToken cancellationToken)
    {
        var query = vocabularyRepository
            .GetQueryableSet()
            .AsNoTracking();

        if (!string.IsNullOrEmpty(request.SearchWord))
            query = query.Where(x => EF.Functions.ILike(x.Word, $"%{request.SearchWord}%"));

        switch (request.Status)
        {
            case ExportStatus.Pending:
                query = query
                    .Where(x => x.Flashcard == null || x.Flashcard.Status == ExportStatus.Pending)
                    .OrderByDescending(x => x.CreatedAt);
                break;
            case
                ExportStatus.Completed:
                query = query
                    .Where(x => x.Flashcard != null && x.Flashcard.Status == ExportStatus.Completed)
                    .OrderByDescending(x => x.Flashcard.ExportedAt);
                break;
            case
                ExportStatus.Failed:
                query = query
                    .Where(x => x.Flashcard != null && x.Flashcard.Status == ExportStatus.Failed)
                    .OrderByDescending(x => x.Flashcard.LastTriedAt);
                break;
        }

        return await query.ProjectToPagedResultAsync<Vocabulary, VocabularyDto>(
            request.PageIndex,
            request.PageSize,
            mapper.ConfigurationProvider,
            cancellationToken);
    }
}