#nullable disable

namespace Vocabi.Domain.Aggregates.Vocabularies;

public class VocabularyMediaFile
{
    public Guid Id { get; set; }
    public Guid VocabularyId { get; set; }
    public Guid MediaFileId { get; set; }
}