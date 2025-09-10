#nullable disable

namespace Vocabi.Domain.Aggregates.Vocabularies;

public class VocabularyFlashcard
{
    public long? NoteId { get; private set; }
    public FlashcardStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private VocabularyFlashcard() { }

    private VocabularyFlashcard(Guid vocabularyId)
    {
        Status = FlashcardStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    internal static VocabularyFlashcard CreateNew(Guid vocabularyId)
    {
        return new VocabularyFlashcard(vocabularyId);
    }

    internal void MarkAsPending()
    {
        if (Status == FlashcardStatus.Exported)
        {
            NoteId = null;
            Status = FlashcardStatus.Pending;
        }
    }

    internal void MarkAsExported(long noteId)
    {
        if (Status == FlashcardStatus.Pending)
        {
            NoteId = noteId;
            Status = FlashcardStatus.Exported;
        }
    }
}