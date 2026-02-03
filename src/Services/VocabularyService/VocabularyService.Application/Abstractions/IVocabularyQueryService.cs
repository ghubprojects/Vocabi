using BuildingBlocks.Application.Models;
using VocabularyService.Application.Features.Dtos;

namespace VocabularyService.Application.Abstractions;

public interface IVocabularyQueryService
{
    Task<PagedResult<VocabularySearchItem>> SearchAsync(string keyword, int pageIndex, int pageSize, CancellationToken cancellationToken);
    Task<VocabularyDetail?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}