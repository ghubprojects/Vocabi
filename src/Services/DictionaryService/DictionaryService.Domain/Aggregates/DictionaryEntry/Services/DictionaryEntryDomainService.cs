using BuildingBlocks.Domain;

namespace DictionaryService.Domain.Aggregates.DictionaryEntry.Services;

public sealed class DictionaryEntryDomainService(IDictionaryEntryRepository repository) 
    : DomainService, IDictionaryEntryDomainService
{
    public DictionaryEntry Create(
        string headword,
        string partOfSpeech,
        string pronunciation,
        string source)
    {
        CheckRule(new DictionaryEntryMustBeUniqueRule(
            repository, headword, partOfSpeech));

        return DictionaryEntry.Create(
            headword, partOfSpeech, pronunciation, source);
    }
}
