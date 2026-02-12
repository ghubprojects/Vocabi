using BuildingBlocks.Application.Models;
using VocabularyService.Application.UseCases.Vocabularies.GetVocabulary;
using VocabularyService.Application.UseCases.Vocabularies.SearchVocabularies;

namespace VocabularyService.Application.Abstractions;

public interface IVocabularyQueryService
{
    Task<PagedResult<VocabularySearchItemDto>> SearchAsync(string keyword, int pageIndex, int pageSize, CancellationToken cancellationToken);
    Task<VocabularyDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}