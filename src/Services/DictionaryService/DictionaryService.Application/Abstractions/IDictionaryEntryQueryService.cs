using DictionaryService.Application.Features.DictionaryEntry.Dtos;

namespace DictionaryService.Application.Abstractions;

public interface IDictionaryEntryQueryService
{
    Task<IReadOnlyList<DictionaryEntrySearchItem>> SearchAsync(string keyword, CancellationToken cancellationToken);
    Task<DictionaryEntryDetail?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}