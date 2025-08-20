using AutoMapper;
using Vocabi.Domain.Aggregates.Vocabularies;

namespace Vocabi.Application.Features.Vocabularies.GetPaginatedVocabularies;

public class VocabularyDto
{
    public Guid Id { get; set; }
    public string Word { get; set; } = string.Empty;
    public string Pronunciation { get; set; } = string.Empty;
    public string PartOfSpeech { get; set; } = string.Empty;
    public string Cloze { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public bool IsSyncedToAnki { get; set; }
    public DateTime CreatedAt { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Vocabulary, VocabularyDto>();
        }
    }
}