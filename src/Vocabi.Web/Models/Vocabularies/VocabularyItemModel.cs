using AutoMapper;
using Vocabi.Application.Features.Vocabularies.DTOs;

namespace Vocabi.Web.Models.Vocabularies;

public class VocabularyItemModel
{
    public Guid Id { get; set; }
    public string Word { get; set; } = string.Empty;
    public string Pronunciation { get; set; } = string.Empty;
    public string PartOfSpeech { get; set; } = string.Empty;
    public string Cloze { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;

    public string CreatedAt { get; set; } = string.Empty;
    public string ExportedAt { get; set; } = string.Empty;
    public string LastTriedAt { get; set; } = string.Empty;

    public bool IsSelected { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<VocabularyDto, VocabularyItemModel>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => FormatDateTime(src.ExportedAt)))
                .ForMember(dest => dest.ExportedAt, opt => opt.MapFrom(src => FormatDateTime(src.ExportedAt)))
                .ForMember(dest => dest.LastTriedAt, opt => opt.MapFrom(src => FormatDateTime(src.LastTriedAt)));
        }

        private static string FormatDateTime(DateTime? dt)
            => dt.HasValue ? dt.Value.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss") : string.Empty;
    }
}
