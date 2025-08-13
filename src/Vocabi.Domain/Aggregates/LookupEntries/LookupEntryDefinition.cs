#nullable disable

namespace Vocabi.Domain.Aggregates.LookupEntries;

public class LookupEntryDefinition
{
    public Guid Id { get; private set; }
    public Guid LookupEntryId { get; private set; }
    public string Text { get; private set; }

    private readonly List<LookupEntryExample> _examples;
    public IReadOnlyCollection<LookupEntryExample> Examples => _examples.AsReadOnly();

    private LookupEntryDefinition()
    {
        _examples = [];
    }

    private LookupEntryDefinition(Guid lookupEntryId, string text)
    {
        Id = Guid.NewGuid();
        LookupEntryId = lookupEntryId;
        Text = text;

        _examples = [];
    }

    public static LookupEntryDefinition CreateNew(Guid lookupEntryId, string text)
    {
        return new LookupEntryDefinition(lookupEntryId, text);
    }

    public void AddExample(string example)
    {
        _examples.Add(LookupEntryExample.CreateNew(Id, example));
    }
}