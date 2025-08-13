#nullable disable

namespace Vocabi.Domain.Aggregates.Vocabularies;

public class Vocabulary
{
    public Guid Id { get; private set; }
    public string Word { get; private set; }
    public string PartOfSpeech { get; private set; }
    public string Pronunciation { get; private set; }
    public string Cloze { get; private set; }
    public string Definition { get; private set; }
    public string Example { get; private set; }
    public string Meaning { get; private set; }
    public bool IsSyncedToAnki { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private readonly List<VocabularyMediaFile> _mediaFiles;
    public IReadOnlyCollection<VocabularyMediaFile> MediaFiles => _mediaFiles.AsReadOnly();

    private Vocabulary()
    {
        _mediaFiles = [];
    }

    private Vocabulary(string word, string partOfSpeech, string pronunciation, string cloze, string definition, string example, string meaning, bool isSyncedToAnki)
    {
        Id = Guid.NewGuid();
        Word = word;
        PartOfSpeech = partOfSpeech;
        Pronunciation = pronunciation;
        Cloze = cloze;
        Definition = definition;
        Example = example;
        Meaning = meaning;
        IsSyncedToAnki = isSyncedToAnki;
        CreatedAt = DateTime.UtcNow;

        _mediaFiles = [];
    }

    public static Vocabulary CreateNew(string word, string partOfSpeech, string pronunciation, string cloze, string definition, string example, string meaning, bool isSyncedToAnki)
    {
        return new Vocabulary(word, partOfSpeech, pronunciation, cloze, definition, example, meaning, isSyncedToAnki);
    }
}