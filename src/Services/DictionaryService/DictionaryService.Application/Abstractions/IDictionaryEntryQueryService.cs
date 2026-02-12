using DictionaryService.Application.UseCases.DictionaryEntries.GetDictionaryEntry;
using DictionaryService.Application.UseCases.DictionaryEntries.SearchDictionaryEntries;

namespace DictionaryService.Application.Abstractions;

public interface IDictionaryEntryQueryService
{
    Task<IReadOnlyList<DictionaryEntrySearchItemDto>> SearchAsync(string keyword, CancellationToken cancellationToken);
    Task<DictionaryEntryDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}