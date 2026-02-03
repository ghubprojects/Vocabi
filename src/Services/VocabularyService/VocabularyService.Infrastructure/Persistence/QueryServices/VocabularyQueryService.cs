using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuildingBlocks.Application.Models;
using BuildingBlocks.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using VocabularyService.Application.Abstractions;
using VocabularyService.Application.Features.Dtos;
using VocabularyService.Domain.Aggregates;

namespace VocabularyService.Infrastructure.Persistence.QueryServices;

public sealed class VocabularyQueryService(VocabularyContext context, IMapper mapper) : IVocabularyQueryService
{
    public async Task<PagedResult<VocabularySearchItem>> SearchAsync(string keyword, int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        keyword = keyword.Trim();

        return await context.Vocabularies
            .AsNoTracking()
            .Where(x => EF.Functions.ILike(x.Word, $"%{keyword}%"))
            .OrderByDescending(x => x.Audit.CreatedAt)
            .ProjectToPagedResultAsync<Vocabulary, VocabularySearchItem>(pageIndex, pageSize, mapper.ConfigurationProvider, cancellationToken);
    }

    public async Task<VocabularyDetail?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Vocabularies
            .AsNoTracking()
            .Where(x => x.Id == id)
            .ProjectTo<VocabularyDetail>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken);
    }
}

