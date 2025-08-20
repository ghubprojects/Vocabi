using System.ComponentModel.DataAnnotations;
using Vocabi.Application.Features.LookupEntries.DTOs;

namespace Vocabi.Web.Models;

public class VocabularyFormModel
{
    [Required(ErrorMessage = "Word must not be empty.")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Word must be between 1 and 200 characters long.")]
    [RegularExpression(@"^[a-zA-Z\s\-'\.]+$", ErrorMessage = "Word can only contain letters, spaces, hyphens, apostrophes, and periods.")]
    public string Word { get; set; } = string.Empty;

    [Required(ErrorMessage = "Part of speech must not be empty.")]
    [StringLength(50, ErrorMessage = "Part of speech must not exceed 50 characters.")]
    public string PartOfSpeech { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Pronunciation must not exceed 100 characters.")]
    [RegularExpression(@"^\/[^\/]*\/\s*$|^$", ErrorMessage = "Pronunciation must be in the format /pronunciation/ or left blank.")]
    public string Pronunciation { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Cloze must not exceed 200 characters.")]
    public string Cloze { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Definition must not exceed 2000 characters.")]
    public string Definition { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Example must not exceed 1000 characters.")]
    public string Example { get; set; } = string.Empty;

    [Required(ErrorMessage = "Meaning must not be empty.")]
    [StringLength(1000, MinimumLength = 2, ErrorMessage = "Meaning must be between 2 and 1000 characters long.")]
    public string Meaning { get; set; } = string.Empty;

    public static VocabularyFormModel FromEntry(LookupEntryDto entry)
    {
        return new VocabularyFormModel
        {
            Word = entry.Headword,
            PartOfSpeech = entry.PartOfSpeech,
            Pronunciation = entry.Pronunciation,
            Cloze = string.Empty,
            Definition = entry.Definitions.FirstOrDefault()?.Text ?? string.Empty,
            Example = entry.Definitions.FirstOrDefault()?.Examples.FirstOrDefault() ?? string.Empty,
            Meaning = entry.Meanings.FirstOrDefault() ?? string.Empty,
        };
    }
}