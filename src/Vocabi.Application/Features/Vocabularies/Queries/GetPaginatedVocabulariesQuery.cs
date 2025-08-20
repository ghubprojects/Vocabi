using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vocabi.Application.Common.Extensions;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Features.Vocabularies.DTOs;
using Vocabi.Domain.Aggregates.Vocabularies;

namespace Vocabi.Application.Features.Vocabularies.Queries;

public class GetPaginatedVocabulariesQuery : IRequest<PaginatedData<VocabularyDto>>
{
    public string SearchWord { get; set; } = string.Empty;
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 10;
}

public class GetPaginatedVocabEntriesQueryHandler(IMapper mapper, IVocabularyRepository vocabularyRepository) 
    : IRequestHandler<GetPaginatedVocabulariesQuery, PaginatedData<VocabularyDto>>
{
    public async Task<PaginatedData<VocabularyDto>> Handle(GetPaginatedVocabulariesQuery request, CancellationToken cancellationToken)
    {
        var query = vocabularyRepository.DbSet.AsNoTracking();

        if (!string.IsNullOrEmpty(request.SearchWord))
            query = query.Where(x => EF.Functions.ILike(x.Word, $"%{request.SearchWord}%"));

        query = query.OrderByDescending(x => x.CreatedAt);

        return await query.ProjectToPaginatedDataAsync<Vocabulary, VocabularyDto>(
            request.PageIndex,
            request.PageSize,
            mapper.ConfigurationProvider);
    }
}