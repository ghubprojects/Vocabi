namespace Vocabi.Application.Contracts.External.Dictionary;

public class DictionaryEntryModel
{
    public string Headword { get; set; } = string.Empty;
    public string PartOfSpeech { get; set; } = string.Empty;
    public string Pronunciation { get; set; } = string.Empty;
    public string AudioUrl { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public List<DictionaryDefinitionModel> Definitions { get; set; } = [];
    public List<string> Meanings { get; set; } = [];

    public string EntrySource { get; set; } = string.Empty;
    public string MeaningSource { get; set; } = string.Empty;
}