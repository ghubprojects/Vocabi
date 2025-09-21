using AutoMapper;
using Vocabi.Application.Features.Vocabularies.DTOs;

namespace Vocabi.Web.ViewModels.Vocabularies.Pending;

public class PendingVocabularyListItemViewModel
{
    public Guid Id { get; set; }
    public string Word { get; set; } = string.Empty;
    public string Pronunciation { get; set; } = string.Empty;
    public string PartOfSpeech { get; set; } = string.Empty;
    public string Cloze { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public bool IsSelected { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<PendingVocabularyDto, PendingVocabularyListItemViewModel>();
        }
    }
}
