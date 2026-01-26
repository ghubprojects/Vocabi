using BuildingBlocks.Domain;

namespace DictionaryService.Domain;

public sealed class DictionarySense : Entity
{
    public string Definition { get; private set; } = string.Empty;
    public int OrderIndex { get; private set; }

    private readonly List<DictionaryExample> _examples = [];
    public IReadOnlyCollection<DictionaryExample> Examples => _examples.AsReadOnly();

    private DictionarySense() { }

    private DictionarySense(string definition, int orderIndex)
    {
        Definition = definition;
        OrderIndex = orderIndex;
    }

    internal static DictionarySense Create(string definition, int orderIndex)
    {
        return new DictionarySense(definition, orderIndex);
    }

    internal void AddExample(string text)
    {
        _examples.Add(DictionaryExample.Create(text));
    }
}