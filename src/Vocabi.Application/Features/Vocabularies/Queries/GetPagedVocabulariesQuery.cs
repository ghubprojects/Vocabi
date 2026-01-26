using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vocabi.Application.Common.Extensions;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Features.Vocabularies.DTOs;
using Vocabi.Domain.Aggregates.Vocabularies;

namespace Vocabi.Application.Features.Vocabularies.Queries;

public record GetPagedVocabulariesQuery(
    string SearchWord,
    ExportStatus Status,
    int PageIndex,
    int PageSize
) : IRequest<Result<PagedData<VocabularyDto>>>;


public class GetPagedVocabulariesQueryHandler(
    IVocabularyRepository vocabularyRepository,
    IMapper mapper
    ) : IRequestHandler<GetPagedVocabulariesQuery, Result<PagedData<VocabularyDto>>>
{
    public async Task<Result<PagedData<VocabularyDto>>> Handle(GetPagedVocabulariesQuery request, CancellationToken cancellationToken)
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

        var pagedData = await query.ProjectToPagedResultAsync<Vocabulary, VocabularyDto>(
            request.PageIndex,
            request.PageSize,
            mapper.ConfigurationProvider,
            cancellationToken);

        return Result.Ok(pagedData);
    }
}