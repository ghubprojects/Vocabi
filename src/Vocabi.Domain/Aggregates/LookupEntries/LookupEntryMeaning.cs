#nullable disable

namespace Vocabi.Domain.Aggregates.LookupEntries;

public class LookupEntryMeaning
{
    public Guid Id { get; private set; }
    public Guid LookupEntryId { get; private set; }
    public string Text { get; private set; }
}