using BuildingBlocks.Domain.Abstractions;

namespace DictionaryService.Domain.Aggregates.DictionaryEntry.Rules;

public class DictionaryEntryMustBeUniqueRule(bool isUnique) : IBusinessRule
{
    public bool IsBroken() => !isUnique;

    public string Message => "Dictionary entry with the same headword and part of speech already exists.";
}

