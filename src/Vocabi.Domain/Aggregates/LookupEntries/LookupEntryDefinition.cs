#nullable disable

namespace Vocabi.Domain.Aggregates.LookupEntries;

public class LookupEntryDefinition
{
    public Guid Id { get; private set; }
    public Guid LookupEntryId { get; private set; }
    public string Text { get; private set; }

    private readonly List<LookupEntryExample> _examples;
    public IReadOnlyCollection<LookupEntryExample> Examples => _examples.AsReadOnly();
}