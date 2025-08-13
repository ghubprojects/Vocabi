using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vocabi.Application.Common.Extensions;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Features.Vocabularies.Models;
using Vocabi.Domain.Aggregates.Vocabularies;

namespace Vocabi.Application.Features.Vocabularies.Queries;

public class GetPaginatedVocabEntriesQuery : IRequest<PaginatedData<VocabularyModel>>
{
    public string SearchWord { get; set; } = string.Empty;
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 10;
}

public class GetPaginatedVocabEntriesQueryHandler(IMapper mapper, IVocabularyRepository vocabEntryRepository)
    : IRequestHandler<GetPaginatedVocabEntriesQuery, PaginatedData<VocabularyModel>>
{
    public async Task<PaginatedData<VocabularyModel>> Handle(GetPaginatedVocabEntriesQuery request, CancellationToken cancellationToken)
    {
        var query = vocabEntryRepository.DbSet.AsNoTracking();

        if (!string.IsNullOrEmpty(request.SearchWord))
            query = query.Where(x => EF.Functions.ILike(x.Word, $"%{request.SearchWord}%"));

        query = query.OrderByDescending(x => x.CreatedAt);

        return await query.ProjectToPaginatedDataAsync<Vocabulary, VocabularyModel>(
            request.PageIndex,
            request.PageSize,
            mapper.ConfigurationProvider);
    }
}