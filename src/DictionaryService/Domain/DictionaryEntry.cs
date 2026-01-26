using BuildingBlocks.Domain;

namespace DictionaryService.Domain;

public sealed class DictionaryEntry : AggregateRoot
{
    public string Headword { get; private set; } = string.Empty;
    public string PartOfSpeech { get; private set; } = string.Empty;
    public string Pronunciation { get; private set; } = string.Empty;
    public string Source { get; private set; } = string.Empty;

    private readonly List<DictionarySense> _senses = [];
    public IReadOnlyCollection<DictionarySense> Senses => _senses.AsReadOnly();

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

    public void AddSense(string definition)
    {
        var orderIndex = _senses.Count + 1;
        _senses.Add(DictionarySense.Create(definition, orderIndex));
    }

    public void AddExampleToSense(Guid senseId, string text)
    {
        var sense = _senses.FirstOrDefault(s => s.Id == senseId)
            ?? throw new InvalidOperationException("Sense not found");

        sense.AddExample(text);
    }
}