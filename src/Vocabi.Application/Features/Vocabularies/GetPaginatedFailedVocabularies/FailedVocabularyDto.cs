using AutoMapper;
using Vocabi.Domain.Aggregates.Vocabularies;
using Vocabi.Shared.Utils;

namespace Vocabi.Application.Features.Vocabularies.GetPaginatedFailedVocabularies;

public class FailedVocabularyDto
{
    public Guid Id { get; set; }
    public string Word { get; set; } = string.Empty;
    public string Pronunciation { get; set; } = string.Empty;
    public string PartOfSpeech { get; set; } = string.Empty;
    public string Cloze { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public string LastTriedAt { get; set; } = string.Empty;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Vocabulary, FailedVocabularyDto>()
                .ForMember(dest => dest.Pronunciation,
                opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.Pronunciation)
                ? FormatterUtils.WrapWithSlashes(src.Pronunciation)
                : string.Empty))
                .ForMember(dest => dest.LastTriedAt,
                opt => opt.MapFrom(src => src.Flashcard != null && src.Flashcard.LastTriedAt.HasValue
                ? src.Flashcard.LastTriedAt.Value.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss")
                : string.Empty));
        }
    }
}