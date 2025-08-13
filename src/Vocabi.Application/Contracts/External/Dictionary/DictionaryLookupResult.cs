namespace Vocabi.Application.Contracts.External.Dictionary;

public class DictionaryLookupResult
{
    public string Headword { get; init; } = string.Empty;
    public string PartOfSpeech { get; init; } = string.Empty;
    public string Pronunciation { get; init; } = string.Empty;
    public string AudioUrl { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;

    public List<DefinitionModel> Definitions { get; init; } = [];
    public List<MeaningModel> Meanings { get; init; } = [];

    public class DefinitionModel
    {
        public string Text { get; init; } = string.Empty;
        public List<string> Examples { get; init; } = [];
    }

    public class MeaningModel
    {
        public string Text { get; init; } = string.Empty;
        public string Example { get; init; } = string.Empty;
    }
}
