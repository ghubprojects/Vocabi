#nullable disable

using Vocabi.Domain.SeedWork;

namespace Vocabi.Domain.Aggregates.LookupEntries;

public class LookupEntryMediaFile : Entity
{
    public Guid Id { get; private set; }
    public Guid LookupEntryId { get; private set; }
    public Guid MediaFileId { get; private set; }

    private LookupEntryMediaFile() { }

    private LookupEntryMediaFile(Guid lookupEntryId, Guid mediaFileId)
    {
        Id = Guid.NewGuid();
        LookupEntryId = lookupEntryId;
        MediaFileId = mediaFileId;
    }

    public static LookupEntryMediaFile CreateNew(Guid lookupEntryId, Guid mediaFileId)
    {
        return new LookupEntryMediaFile(lookupEntryId, mediaFileId);
    }
}