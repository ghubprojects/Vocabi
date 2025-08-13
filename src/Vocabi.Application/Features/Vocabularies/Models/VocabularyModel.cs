namespace Vocabi.Application.Features.Vocabularies.Models;

public class VocabularyModel
{
    public Guid Id { get; set; }
    public string Word { get; set; } = string.Empty;
    public string Phonetic { get; set; } = string.Empty;
    public string PartOfSpeech { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public string Example { get; set; } = string.Empty;
    public string Cloze { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public bool IsSyncedToAnki { get; set; }
    public DateTime CreatedAt { get; set; }
}