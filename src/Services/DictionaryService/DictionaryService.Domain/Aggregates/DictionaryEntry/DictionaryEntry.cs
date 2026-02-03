using BuildingBlocks.Domain;

namespace DictionaryService.Domain.Aggregates.DictionaryEntry;

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

    public static DictionaryEntry Create(string headword, string partOfSpeech, string pronunciation, string source)
    {
        return new DictionaryEntry(headword, partOfSpeech, pronunciation, source);
    }

    public void AddDefinition(string text)
    {
        var orderIndex = _definitions.Count + 1;
        _definitions.Add(DictionaryDefinition.Create(text, orderIndex));
    }

    public void AddExample(Guid definitionId, string text)
    {
        var definition = _definitions.FirstOrDefault(s => s.Id == definitionId)
            ?? throw new InvalidOperationException("Definition not found");

        definition.AddExample(text);
    }
}