#nullable disable

using Vocabi.Domain.SeedWork;

namespace Vocabi.Domain.Aggregates.Vocabularies;

public class Vocabulary : IAggregateRoot
{
    public Guid Id { get; private set; }
    public string Word { get; private set; }
    public string PartOfSpeech { get; private set; }
    public string Pronunciation { get; private set; }
    public string Cloze { get; private set; }
    public string Definition { get; private set; }
    public string Example { get; private set; }
    public string Meaning { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Media files
    private readonly List<VocabularyMediaFile> _mediaFiles;
    public IReadOnlyCollection<VocabularyMediaFile> MediaFiles => _mediaFiles.AsReadOnly();

    // Flashcards
    public VocabularyFlashcard Flashcard { get; private set; }

    private Vocabulary()
    {
        _mediaFiles = [];
    }

    private Vocabulary(string word, string partOfSpeech, string pronunciation, string cloze, string definition, string example, string meaning)
    {
        Id = Guid.NewGuid();
        Word = word;
        PartOfSpeech = partOfSpeech;
        Pronunciation = pronunciation;
        Cloze = cloze;
        Definition = definition;
        Example = example;
        Meaning = meaning;
        CreatedAt = DateTime.UtcNow;

        _mediaFiles = [];
    }

    public static Vocabulary CreateNew(string word, string partOfSpeech, string pronunciation, string cloze, string definition, string example, string meaning)
    {
        return new Vocabulary(word, partOfSpeech, pronunciation, cloze, definition, example, meaning);
    }

    public void AttachMediaFile(Guid mediaFileId)
    {
        _mediaFiles.Add(VocabularyMediaFile.CreateNew(Id, mediaFileId));
    }

    public void AttachMediaFiles(IEnumerable<Guid> mediaFileIds)
    {
        foreach (var id in mediaFileIds)
        {
            AttachMediaFile(id);
        }
    }

    public void AddFlashcard()
    {
        Flashcard = VocabularyFlashcard.CreateNew(Id);
    }

    public void MarkFlashcardAsExported(long noteId)
    {
        Flashcard.MarkAsExported(noteId);
    }

    public void UnmarkFlashcardAsExported()
    {
        Flashcard.MarkAsPending();
    }
}