#nullable disable

namespace Vocabi.Domain.Aggregates.LookupEntries;

public class LookupEntryExample
{
    public Guid Id { get; private set; }
    public Guid LookupEntryDefinitionId { get; private set; }
    public string Text { get; private set; }
}