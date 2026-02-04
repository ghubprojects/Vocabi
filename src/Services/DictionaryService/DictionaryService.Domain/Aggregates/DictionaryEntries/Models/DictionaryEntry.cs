using BuildingBlocks.Domain;

namespace DictionaryService.Domain.Aggregates.DictionaryEntries.Models;

public sealed class DictionaryEntry : AggregateRoot
{
    public string Headword { get; private set; } = string.Empty;
    public string PartOfSpeech { get; private set; } = string.Empty;
    public string Pronunciation { get; private set; } = string.Empty;
    public string Source { get; private set; } = string.Empty;

    private readonly List<DictionaryDefinition> _definitions = [];
    public IReadOnlyCollection<DictionaryDefinition> Definitions => _definitions.AsReadOnly();

    private DictionaryEntry() { }

    private DictionaryEntry(string headword, string partOfSpeech, string pronunciation, string source)
    {
        Headword = headword;
        PartOfSpeech = partOfSpeech;
        Pronunciation = pronunciation;
        Source = source;
    }

    internal static DictionaryEntry Create(string headword, string partOfSpeech, string pronunciation, string source)
    {
        return new DictionaryEntry(headword, partOfSpeech, pronunciation, source);
    }

    public void AddDefinition(string text, IEnumerable<string> examples)
    {
        var orderIndex = _definitions.Count + 1;
        var definition = DictionaryDefinition.Create(text, orderIndex);

        foreach (var example in examples)
            definition.AddExample(example);

        _definitions.Add(definition);
    }
}