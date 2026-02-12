using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuildingBlocks.Application.Models;
using BuildingBlocks.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using VocabularyService.Application.Abstractions;
using VocabularyService.Application.UseCases.Vocabularies.GetVocabulary;
using VocabularyService.Application.UseCases.Vocabularies.SearchVocabularies;
using VocabularyService.Domain.Aggregates;
using VocabularyService.Infrastructure.Persistence.DataContext;

namespace VocabularyService.Infrastructure.Persistence.QueryServices;

public sealed class VocabularyQueryService(VocabularyContext context, IMapper mapper) : IVocabularyQueryService
{
    public async Task<PagedResult<VocabularySearchItemDto>> SearchAsync(string keyword, int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        keyword = keyword.Trim();

        return await context.Vocabularies
            .AsNoTracking()
            .Where(x => EF.Functions.ILike(x.Headword, $"%{keyword}%"))
            .OrderByDescending(x => x.Audit.CreatedAt)
            .ProjectToPagedResultAsync<Vocabulary, VocabularySearchItemDto>(pageIndex, pageSize, mapper.ConfigurationProvider, cancellationToken);
    }

    public async Task<VocabularyDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Vocabularies
            .AsNoTracking()
            .Where(x => x.Id == id)
            .ProjectTo<VocabularyDetailDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken);
    }
}