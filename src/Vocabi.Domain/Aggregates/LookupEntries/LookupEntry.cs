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
}