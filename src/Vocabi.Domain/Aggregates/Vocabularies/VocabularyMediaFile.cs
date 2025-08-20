#nullable disable

namespace Vocabi.Domain.Aggregates.Vocabularies;

public class VocabularyMediaFile
{
    public Guid Id { get; set; }
    public Guid VocabularyId { get; set; }
    public Guid MediaFileId { get; set; }

    private VocabularyMediaFile() { }

    private VocabularyMediaFile(Guid vocabularyId, Guid mediaFileId)
    {
        Id = Guid.NewGuid();
        VocabularyId = vocabularyId;
        MediaFileId = mediaFileId;
    }

    public static VocabularyMediaFile CreateNew(Guid vocabularyId, Guid mediaFileId)
    {
        return new VocabularyMediaFile(vocabularyId, mediaFileId);
    }
}