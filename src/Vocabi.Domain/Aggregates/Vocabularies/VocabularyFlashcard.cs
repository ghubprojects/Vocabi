#nullable disable

using Vocabi.Domain.SeedWork;

namespace Vocabi.Domain.Aggregates.Vocabularies;

public class VocabularyFlashcard : Entity
{
    public long? NoteId { get; private set; }
    public ExportStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ExportedAt { get; private set; }
    public DateTime? LastTriedAt { get; private set; }

    private VocabularyFlashcard()
    {
        Status = ExportStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    internal static VocabularyFlashcard CreateNew()
    {
        return new();
    }

    internal void MarkAsExported(long noteId)
    {
        if (Status == ExportStatus.Pending || Status == ExportStatus.Failed)
        {
            NoteId = noteId;
            Status = ExportStatus.Completed;
            ExportedAt = DateTime.UtcNow;
        }
    }

    internal void MarkAsFailed()
    {
        if (Status == ExportStatus.Pending)
        {
            NoteId = null;
            Status = ExportStatus.Failed;
            LastTriedAt = DateTime.UtcNow;
        }
    }
}