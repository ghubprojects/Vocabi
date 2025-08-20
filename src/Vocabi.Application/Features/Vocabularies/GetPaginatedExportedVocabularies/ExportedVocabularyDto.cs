using AutoMapper;
using Vocabi.Domain.Aggregates.Vocabularies;
using Vocabi.Shared.Utils;

namespace Vocabi.Application.Features.Vocabularies.GetPaginatedExportedVocabularies;

public class ExportedVocabularyDto
{
    public Guid Id { get; set; }
    public string Word { get; set; } = string.Empty;
    public string Pronunciation { get; set; } = string.Empty;
    public string PartOfSpeech { get; set; } = string.Empty;
    public string Cloze { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public string ExportedToAnkiAt { get; set; } = string.Empty;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Vocabulary, ExportedVocabularyDto>()
                .ForMember(dest => dest.Pronunciation,
                opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.Pronunciation)
                ? StringUtils.WrapWithSlash(src.Pronunciation)
                : string.Empty))
                .ForMember(dest => dest.ExportedToAnkiAt,
                opt => opt.MapFrom(src => src.Flashcard != null
                ? src.Flashcard.CreatedAt.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss")
                : string.Empty));
        }
    }
}