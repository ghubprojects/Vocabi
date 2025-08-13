#nullable disable

namespace Vocabi.Domain.Aggregates.LookupEntries;

public class LookupEntryMediaFile
{
    public Guid Id { get; private set; }
    public Guid LookupEntryId { get; private set; }
    public Guid MediaFileId { get; private set; }
}