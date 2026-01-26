using Vocabi.Domain.SeedWork;

namespace Vocabi.Domain.Aggregates.Vocabularies;

public class Vocabulary : Entity, IAggregateRoot
{
    public Guid Id { get; private set; }
    public string Word { get; private set; } = string.Empty;
    public string? PartOfSpeech { get; private set; }
    public string? Pronunciation { get; private set; }
    public string? Cloze { get; private set; }
    public string? Definition { get; private set; }
    public string? Example { get; private set; }
    public string? Meaning { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private readonly List<VocabularyMediaFile> _mediaFiles = [];
    public IReadOnlyCollection<VocabularyMediaFile> MediaFiles => _mediaFiles.AsReadOnly();

    public VocabularyFlashcard? Flashcard { get; private set; }

    private Vocabulary() { }

    private Vocabulary(
        string word, 
        string partOfSpeech, 
        string pronunciation, 
        string cloze, 
        string definition, 
        string example, 
        string meaning)
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
    }

    public static Vocabulary Create(
       string word,
       string partOfSpeech,
       string pronunciation,
       string cloze,
       string definition,
       string example,
       string meaning,
       IEnumerable<Guid>? mediaFileIds = null)
    {
        var vocabulary = new Vocabulary(
            word,
            partOfSpeech,
            pronunciation,
            cloze,
            definition,
            example,
            meaning);

        if (mediaFileIds != null)
            vocabulary.AttachMediaFiles(mediaFileIds);

        return vocabulary;
    }

    private void AttachMediaFiles(IEnumerable<Guid> mediaFileIds)
    {
        foreach (var mediaFileId in mediaFileIds)
            _mediaFiles.Add(VocabularyMediaFile.Create(Id, mediaFileId));
    }

    public void EnsureFlashcardCreated()
    {
        if (Flashcard is null)
            AddFlashcard();
    }

    private void AddFlashcard()
    {
        Flashcard = VocabularyFlashcard.Create();
    }

    public void MarkFlashcardAsExported(long noteId)
    {
        if (Flashcard is null)
            AddFlashcard();

        Flashcard.MarkAsExported(noteId);
    }

    public void MarkFlashcardAsFailed()
    {
        if (Flashcard is null)
            AddFlashcard();

        Flashcard.MarkAsFailed();
    }

    public void RemoveFlashcard()
    {
        Flashcard = null;
    }

    public bool HasFlashcard()
        => Flashcard is not null;

    public bool HasExportedFlashcard()
        => Flashcard is { Status: ExportStatus.Completed, NoteId: not null };

    public bool HasFailedFlashcard()
        => Flashcard is { Status: ExportStatus.Failed };

    public bool HasPendingFlashcard()
        => Flashcard is { Status: ExportStatus.Pending };
}