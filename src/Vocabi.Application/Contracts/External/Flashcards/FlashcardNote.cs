using AutoMapper;
using Vocabi.Domain.Aggregates.MediaFiles;
using Vocabi.Domain.Aggregates.Vocabularies;
using Vocabi.Shared.Utils;

namespace Vocabi.Application.Contracts.External.Flashcards;

public class FlashcardNote
{
    public string Word { get; set; } = string.Empty;
    public string PartOfSpeech { get; set; } = string.Empty;
    public string Pronunciation { get; set; } = string.Empty;
    public string Cloze { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public string Example { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty; 
    public string Audio { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<(Vocabulary vocabulary, IReadOnlyList<MediaFile> mediaFiles), FlashcardNote>()
                .ForMember(dest => dest.Word, opt => opt.MapFrom(src => src.vocabulary.Word))
                .ForMember(dest => dest.PartOfSpeech, opt => opt.MapFrom(src => src.vocabulary.PartOfSpeech))
                .ForMember(dest => dest.Pronunciation, opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.vocabulary.Pronunciation)
                ? StringUtils.WrapWithSlash(src.vocabulary.Pronunciation)
                : string.Empty))
                .ForMember(dest => dest.Cloze, opt => opt.MapFrom(src => src.vocabulary.Cloze))
                .ForMember(dest => dest.Definition, opt => opt.MapFrom(src => src.vocabulary.Definition))
                .ForMember(dest => dest.Example, opt => opt.MapFrom(src => src.vocabulary.Example))
                .ForMember(dest => dest.Meaning, opt => opt.MapFrom(src => src.vocabulary.Meaning))
                .ForMember(dest => dest.Audio, opt => opt.MapFrom(src =>
                    FormatMedia(src.mediaFiles.FirstOrDefault(m => m.ContentType.StartsWith("audio/")))))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src =>
                    FormatMedia(src.mediaFiles.FirstOrDefault(m => m.ContentType.StartsWith("image/")))));
        }
    }

    private static string FormatMedia(MediaFile file)
    {
        if (file is null)
            return string.Empty;

        if (file.ContentType.StartsWith("image/"))
            return $"<img src=\"{file.FileName}\" />";

        if (file.ContentType.StartsWith("audio/"))
            return $"[sound:{file.FileName}]";

        return string.Empty;
    }
}