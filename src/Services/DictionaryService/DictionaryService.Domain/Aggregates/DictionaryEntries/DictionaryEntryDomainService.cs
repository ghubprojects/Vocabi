using BuildingBlocks.Domain;
using DictionaryService.Domain.Aggregates.DictionaryEntries.Models;
using DictionaryService.Domain.Aggregates.DictionaryEntries.Rules;

namespace DictionaryService.Domain.Aggregates.DictionaryEntries;

public sealed class DictionaryEntryDomainService(IDictionaryEntryRepository repository) : DomainService
{
    public async Task<DictionaryEntry> CreateAsync(string headword, string partOfSpeech, string pronunciation, string source)
    {
        var exists = await repository.ExistsByHeadwordAndPartOfSpeechAsync(headword, partOfSpeech);

        CheckRule(new DictionaryEntryMustBeUniqueRule(exists));

        return DictionaryEntry.Create(headword, partOfSpeech, pronunciation, source);
    }
}