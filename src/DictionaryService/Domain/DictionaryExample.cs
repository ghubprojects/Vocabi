using BuildingBlocks.Domain;

namespace DictionaryService.Domain;

public sealed class DictionaryExample : Entity
{
    public string Text { get; private set; } = string.Empty;

    private DictionaryExample() { }

    private DictionaryExample(string text)
    {
        Text = text;
    }

    internal static DictionaryExample Create(string text)
    {
        return new DictionaryExample(text);
    }
}