#nullable disable

namespace Vocabi.Domain.Aggregates.Vocabularies;

public class VocabularyMediaFile
{
    public Guid Id { get; private set; }
    public Guid VocabularyId { get; private set; }
    public Guid MediaFileId { get; private set; }

    private VocabularyMediaFile() { }

    private VocabularyMediaFile(Guid vocabularyId, Guid mediaFileId)
    {
        Id = Guid.NewGuid();
        VocabularyId = vocabularyId;
        MediaFileId = mediaFileId;
    }

    internal static VocabularyMediaFile CreateNew(Guid vocabularyId, Guid mediaFileId)
    {
        return new VocabularyMediaFile(vocabularyId, mediaFileId);
    }
}