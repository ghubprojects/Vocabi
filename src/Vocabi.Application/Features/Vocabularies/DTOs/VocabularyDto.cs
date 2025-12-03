using AutoMapper;
using Vocabi.Application.Features.MediaFiles.DTOs;
using Vocabi.Domain.Aggregates.Vocabularies;
using Vocabi.Shared.Utils;

namespace Vocabi.Application.Features.Vocabularies.DTOs;

public class VocabularyDto
{
    public Guid Id { get; set; }
    public string Word { get; set; } = string.Empty;
    public string Pronunciation { get; set; } = string.Empty;
    public string PartOfSpeech { get; set; } = string.Empty;
    public string Cloze { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public string Example { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;

    public MediaFileDto AudioFile { get; set; } = new();
    public MediaFileDto ImageFile { get; set; } = new();

    public DateTime CreatedAt { get; set; }
    public DateTime? ExportedAt { get; set; }
    public DateTime? LastTriedAt { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Vocabulary, VocabularyDto>()
                .ForMember(dest => dest.Pronunciation, opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.Pronunciation)
                    ? FormatterUtils.WrapWithSlashes(src.Pronunciation)
                    : string.Empty))
                .ForMember(dest => dest.ExportedAt, opt => opt.MapFrom(src => src.Flashcard.ExportedAt))
                .ForMember(dest => dest.LastTriedAt, opt => opt.MapFrom(src => src.Flashcard.LastTriedAt));
        }
    }
}