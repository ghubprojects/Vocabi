using AutoMapper;
using Vocabi.Domain.Aggregates.Vocabularies;
using Vocabi.Shared.Utils;

namespace Vocabi.Application.Features.Vocabularies.DTOs;

public class FailedVocabularyDto
{
    public Guid Id { get; set; }
    public string Word { get; set; } = string.Empty;
    public string Pronunciation { get; set; } = string.Empty;
    public string PartOfSpeech { get; set; } = string.Empty;
    public string Cloze { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public DateTime? LastTriedAt { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Vocabulary, FailedVocabularyDto>()
                .ForMember(dest => dest.Pronunciation,
                opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.Pronunciation)
                ? FormatterUtils.WrapWithSlashes(src.Pronunciation)
                : string.Empty))
                .ForMember(dest => dest.LastTriedAt, opt => opt.MapFrom(src => src.Flashcard.LastTriedAt));
        }
    }
}