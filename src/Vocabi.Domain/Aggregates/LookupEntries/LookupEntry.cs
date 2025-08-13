#nullable disable

namespace Vocabi.Domain.Aggregates.LookupEntries;

public class LookupEntry
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

    public void AddMeaning(string meaning)
    {
        _meanings.Add(LookupEntryMeaning.CreateNew(Id, meaning));
    }

    public void AddMediaFile(Guid mediaFileId)
    {
        _mediaFiles.Add(LookupEntryMediaFile.CreateNew(Id, mediaFileId));
    }
}