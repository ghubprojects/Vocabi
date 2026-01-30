using AutoMapper;
using System.ComponentModel.DataAnnotations;
using Vocabi.Application.Features.LookupEntries.DTOs;
using Vocabi.Application.Features.Vocabularies.DTOs;
using Vocabi.Shared.Utils;
using Vocabi.Web.ViewModels.MediaFiles;

namespace Vocabi.Web.Models.Vocabularies;

<<<<<<<< HEAD:src/Vocabi.Web/Models/Vocabularies/VocabularyDetailViewModel.cs
public class VocabularyDetailViewModel
========
public class VocabularyFormModel
>>>>>>>> 9842cd63dc353721663e65e43b087c5b9f598e26:src/Vocabi.Web/Models/Vocabularies/VocabularyFormModel.cs
{
    public Guid Id { get; set; }

    private string _word = string.Empty;
    private string _cloze = string.Empty;

    [Required(ErrorMessage = "Word must not be empty.")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Word must be between 1 and 200 characters long.")]
    [RegularExpression(@"^[a-zA-Z\s\-'\.]+$", ErrorMessage = "Word can only contain letters, spaces, hyphens, apostrophes, and periods.")]
    public string Word
    {
        get => _word;
        set
        {
            if (_word != value)
            {
                _word = value.Trim().ToLower();
                Cloze = StringUtils.Mask(_word);
            }
        }
    }

    [StringLength(200, ErrorMessage = "Cloze must not exceed 200 characters.")]
    public string Cloze
    {
        get => _cloze;
        set => _cloze = value;
    }

    [Required(ErrorMessage = "Part of speech must not be empty.")]
    [StringLength(50, ErrorMessage = "Part of speech must not exceed 50 characters.")]
    public string PartOfSpeech { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Pronunciation must not exceed 100 characters.")]
    [RegularExpression(@"^\/[^\/]*\/\s*$|^$", ErrorMessage = "Pronunciation must be in the format /pronunciation/ or left blank.")]
    public string Pronunciation { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Definition must not exceed 2000 characters.")]
    public string Definition { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Example must not exceed 1000 characters.")]
    public string Example { get; set; } = string.Empty;

    [Required(ErrorMessage = "Meaning must not be empty.")]
    [StringLength(1000, MinimumLength = 2, ErrorMessage = "Meaning must be between 2 and 1000 characters long.")]
    public string Meaning { get; set; } = string.Empty;

    public string CreatedAt { get; set; } = string.Empty;
    public string ExportedAt { get; set; } = string.Empty;
    public string LastTriedAt { get; set; } = string.Empty;

    public MediaFileViewModel AudioFile { get; set; } = new MediaFileViewModel();
    public MediaFileViewModel ImageFile { get; set; } = new MediaFileViewModel();

    public bool IsSelected { get; set; }

    public bool IsDeleting { get; set; }
    public bool IsExporting { get; set; }
    public bool IsRemovingExport { get; set; }
    public bool IsRetryingExport { get; set; }

    public void UpdateFromEntry(LookupEntryDto entry)
    {
        Word = entry.Headword;
        PartOfSpeech = entry.PartOfSpeech;
        Pronunciation = FormatterUtils.WrapWithSlashes(entry.Pronunciation);
        Definition = entry.Definitions.FirstOrDefault()?.Text ?? string.Empty;
        Example = entry.Definitions.FirstOrDefault()?.Examples.FirstOrDefault() ?? string.Empty;
        Meaning = entry.Meanings.FirstOrDefault() ?? string.Empty;
    }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<VocabularyDto, VocabularyDetailViewModel>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => FormatDateTime(src.ExportedAt)))
                .ForMember(dest => dest.ExportedAt, opt => opt.MapFrom(src => FormatDateTime(src.ExportedAt)))
                .ForMember(dest => dest.LastTriedAt, opt => opt.MapFrom(src => FormatDateTime(src.LastTriedAt)));
        }

        private static string FormatDateTime(DateTime? dt)
            => dt.HasValue ? dt.Value.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss") : string.Empty;
    }
}