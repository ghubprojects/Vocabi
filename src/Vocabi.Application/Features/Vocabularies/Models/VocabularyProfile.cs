using AutoMapper;
using Vocabi.Domain.Aggregates.Vocabularies;

namespace Vocabi.Application.Features.Vocabularies.Models;

public class VocabularyProfile : Profile
{
    public VocabularyProfile()
    {
        CreateMap<Vocabulary, VocabularyModel>();
    }
}