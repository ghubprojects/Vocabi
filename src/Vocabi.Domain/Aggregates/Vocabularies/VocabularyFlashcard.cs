#nullable disable

using Vocabi.Domain.SeedWork;

namespace Vocabi.Domain.Aggregates.Vocabularies;

public class VocabularyFlashcard : Entity
{
    public long? NoteId { get; private set; }
    public FlashcardStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ExportedAt { get; private set; }
    public DateTime? LastTriedAt { get; private set; }

    private VocabularyFlashcard()
    {
        Status = FlashcardStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    internal static VocabularyFlashcard CreateNew()
    {
        return new();
    }

    internal void MarkAsExported(long noteId)
    {
        if (Status == FlashcardStatus.Pending || Status == FlashcardStatus.Failed)
        {
            NoteId = noteId;
            Status = FlashcardStatus.Exported;
            ExportedAt = DateTime.UtcNow;
        }
    }

    internal void MarkAsFailed()
    {
        if (Status == FlashcardStatus.Pending)
        {
            NoteId = null;
            Status = FlashcardStatus.Failed;
            LastTriedAt = DateTime.UtcNow;
        }
    }
}