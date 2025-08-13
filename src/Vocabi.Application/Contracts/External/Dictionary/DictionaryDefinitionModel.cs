namespace Vocabi.Application.Contracts.External.Dictionary;

public class DictionaryDefinitionModel
{
    public string Text { get; init; } = string.Empty;
    public List<string> Examples { get; init; } = [];
}