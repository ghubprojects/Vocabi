#nullable disable

namespace Vocabi.Domain.Aggregates.LookupEntries;

public class LookupEntryMeaning
{
    public Guid Id { get; private set; }
    public Guid LookupEntryId { get; private set; }
    public string Text { get; private set; }

    private LookupEntryMeaning() { }

    private LookupEntryMeaning(Guid lookupEntryId, string text)
    {
        Id = Guid.NewGuid();
        LookupEntryId = lookupEntryId;
        Text = text;
    }

    public static LookupEntryMeaning CreateNew(Guid lookupEntryId, string text)
    {
        return new LookupEntryMeaning(lookupEntryId, text);
    }
}