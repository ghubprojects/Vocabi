using BuildingBlocks.Domain;

namespace DictionaryService.Domain.Aggregates.DictionaryEntries.Models;

public sealed class DictionaryDefinition : Entity
{
    public string Text { get; private set; } = string.Empty;
    public int OrderIndex { get; private set; }

    private readonly List<DictionaryExample> _examples = [];
    public IReadOnlyCollection<DictionaryExample> Examples => _examples.AsReadOnly();

    private DictionaryDefinition() { }

    private DictionaryDefinition(string definition, int orderIndex)
    {
        Text = definition;
        OrderIndex = orderIndex;
    }

    internal static DictionaryDefinition Create(string definition, int orderIndex)
    {
        return new DictionaryDefinition(definition, orderIndex);
    }

    internal void AddExample(string text)
    {
        _examples.Add(DictionaryExample.Create(text));
    }
}