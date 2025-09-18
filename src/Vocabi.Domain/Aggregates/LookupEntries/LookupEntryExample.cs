#nullable disable

using Vocabi.Domain.SeedWork;

namespace Vocabi.Domain.Aggregates.LookupEntries;

public class LookupEntryExample : Entity
{
    public Guid Id { get; private set; }
    public Guid LookupEntryDefinitionId { get; private set; }
    public string Text { get; private set; }

    private LookupEntryExample() { }

    private LookupEntryExample(Guid lookupEntryDefinitionId, string text)
    {
        Id = Guid.NewGuid();
        LookupEntryDefinitionId = lookupEntryDefinitionId;
        Text = text;
    }

    public static LookupEntryExample CreateNew(Guid lookupEntryDefinitionId, string text)
    {
        return new LookupEntryExample(lookupEntryDefinitionId, text);
    }
}