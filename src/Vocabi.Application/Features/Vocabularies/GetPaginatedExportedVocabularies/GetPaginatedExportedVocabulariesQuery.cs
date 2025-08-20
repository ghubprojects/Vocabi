using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vocabi.Application.Common.Extensions;
using Vocabi.Application.Common.Models;
using Vocabi.Domain.Aggregates.Vocabularies;

namespace Vocabi.Application.Features.Vocabularies.GetPaginatedExportedVocabularies;

public class GetPaginatedExportedVocabulariesQuery : IRequest<PaginatedData<ExportedVocabularyDto>>
{
    public string SearchWord { get; init; } = string.Empty;
    public int PageIndex { get; init; } = 0;
    public int PageSize { get; init; } = 10;
}

public class GetPaginatedExportedVocabulariesQueryHandler(
    IVocabularyRepository vocabularyRepository,
    IMapper mapper
    ) : IRequestHandler<GetPaginatedExportedVocabulariesQuery, PaginatedData<ExportedVocabularyDto>>
{
    public async Task<PaginatedData<ExportedVocabularyDto>> Handle(GetPaginatedExportedVocabulariesQuery request, CancellationToken cancellationToken)
    {
        var query = vocabularyRepository.DbSet
            .AsNoTracking()
            .Where(x => x.Flashcard != null && x.Flashcard.Status == FlashcardStatus.Exported);

        if (!string.IsNullOrEmpty(request.SearchWord))
            query = query.Where(x => EF.Functions.ILike(x.Word, $"%{request.SearchWord}%"));

        query = query.OrderByDescending(x => x.Flashcard.CreatedAt);

        return await query.ProjectToPaginatedDataAsync<Vocabulary, ExportedVocabularyDto>(
            request.PageIndex,
            request.PageSize,
            mapper.ConfigurationProvider,
            cancellationToken);
    }
}