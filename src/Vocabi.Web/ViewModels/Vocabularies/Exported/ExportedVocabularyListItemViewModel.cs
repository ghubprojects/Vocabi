using AutoMapper;
using Vocabi.Application.Features.Vocabularies.DTOs;

namespace Vocabi.Web.ViewModels.Vocabularies.Exported;

public class ExportedVocabularyListItemViewModel
{
    public Guid Id { get; set; }
    public string Word { get; set; } = string.Empty;
    public string Pronunciation { get; set; } = string.Empty;
    public string PartOfSpeech { get; set; } = string.Empty;
    public string Cloze { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public string ExportedAt { get; set; } = string.Empty;

    public bool IsSelected { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ExportedVocabularyDto, ExportedVocabularyListItemViewModel>()
                .ForMember(dest => dest.ExportedAt,
                opt => opt.MapFrom(src => src.ExportedAt.HasValue 
                ? src.ExportedAt.Value.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss")
                : string.Empty));
        }
    }
}
