using BuildingBlocks.Domain.Abstractions;

namespace DictionaryService.Domain.Aggregates.DictionaryEntries.Rules;

public class DictionaryEntryMustBeUniqueRule(bool alreadyExists) : IBusinessRule
{
    public bool IsBroken() => alreadyExists;

    public string Message => "Dictionary entry with the same headword and part of speech already exists.";
}