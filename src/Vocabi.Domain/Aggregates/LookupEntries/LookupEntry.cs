#nullable disable

using Vocabi.Domain.SeedWork;

namespace Vocabi.Domain.Aggregates.LookupEntries;

public class LookupEntry : IAggregateRoot
{
    public Guid Id { get; private set; }
    public string Headword { get; private set; }
    public string PartOfSpeech { get; private set; }
    public string Pronunciation { get; private set; }

    private readonly List<LookupEntryDefinition> _definitions;
    public IReadOnlyCollection<LookupEntryDefinition> Definitions => _definitions.AsReadOnly();

    private readonly List<LookupEntryMeaning> _meanings;
    public IReadOnlyCollection<LookupEntryMeaning> Meanings => _meanings.AsReadOnly();

    private readonly List<LookupEntryMediaFile> _mediaFiles;
    public IReadOnlyCollection<LookupEntryMediaFile> MediaFiles => _mediaFiles.AsReadOnly();

    private LookupEntry()
    {
        _definitions = [];
        _meanings = [];
        _mediaFiles = [];
    }

    private LookupEntry(string headword, string partOfSpeech, string pronunciation)
    {
        Id = Guid.NewGuid();
        Headword = headword;
        PartOfSpeech = partOfSpeech;
        Pronunciation = pronunciation;

        _definitions = [];
        _meanings = [];
        _mediaFiles = [];
    }

    public static LookupEntry CreateNew(string headword, string partOfSpeech, string pronunciation)
    {
        return new LookupEntry(headword, partOfSpeech, pronunciation);
    }

    public void AddDefinition(string definition)
    {
        _definitions.Add(LookupEntryDefinition.CreateNew(Id, definition));
    }

    public void AddDefinitionWithExamples(string definition, IEnumerable<string> examples)
    {
        var newDefinition = LookupEntryDefinition.CreateNew(Id, definition);
        foreach (var example in examples)
        {
            newDefinition.AddExample(example);
        }
        _definitions.Add(newDefinition);
    }

    public void AddDefinitions(IEnumerable<string> definitions)
    {
        foreach (var definition in definitions)
        {
            AddDefinition(definition);
        }
    }

    public void AddExampleToDefinition(Guid definitionId, string example)
    {
        var definition = _definitions.FirstOrDefault(d => d.Id == definitionId)
            ?? throw new InvalidOperationException("Definition not found");

        definition.AddExample(example);
    }

    public void AddMeaning(string meaning)
    {
        _meanings.Add(LookupEntryMeaning.CreateNew(Id, meaning));
    }

    public void AddMeanings(IEnumerable<string> meanings)
    {
        foreach (var meaning in meanings)
        {
            AddMeaning(meaning);
        }
    }

    public void AttachMediaFile(Guid mediaFileId)
    {
        _mediaFiles.Add(LookupEntryMediaFile.CreateNew(Id, mediaFileId));
    }

    public void AttachMediaFiles(IEnumerable<Guid> mediaFileIds)
    {
        foreach (var id in mediaFileIds)
        {
            AttachMediaFile(id);
        }
    }
}