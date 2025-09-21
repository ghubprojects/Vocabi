using AutoMapper;
using Vocabi.Application.Features.Vocabularies.DTOs;

namespace Vocabi.Web.ViewModels.Vocabularies.Failed;

public class FailedVocabularyListItemViewModel
{
    public Guid Id { get; set; }
    public string Word { get; set; } = string.Empty;
    public string Pronunciation { get; set; } = string.Empty;
    public string PartOfSpeech { get; set; } = string.Empty;
    public string Cloze { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public string LastTriedAt { get; set; } = string.Empty;

    public bool IsSelected { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<FailedVocabularyDto, FailedVocabularyListItemViewModel>()
                .ForMember(dest => dest.LastTriedAt,
                opt => opt.MapFrom(src => src.LastTriedAt.HasValue
                ? src.LastTriedAt.Value.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss")
                : string.Empty));
            ;
        }
    }
}
