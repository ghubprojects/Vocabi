using BuildingBlocks.Domain.Abstractions;
using DictionaryService.Domain.Aggregates.DictionaryEntries.Models;

namespace DictionaryService.Domain.Aggregates.DictionaryEntries;

public interface IDictionaryEntryRepository : IRepository<DictionaryEntry>
{
    Task AddAsync(DictionaryEntry entity);
    Task AddRangeAsync(IEnumerable<DictionaryEntry> entities);

    Task<bool> ExistsByHeadwordAsync(string headword);
    Task<bool> ExistsByHeadwordAndPartOfSpeechAsync(string headword, string partOfSpeech);
}