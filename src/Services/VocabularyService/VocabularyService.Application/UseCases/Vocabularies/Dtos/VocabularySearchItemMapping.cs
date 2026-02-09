using AutoMapper;

namespace VocabularyService.Application.UseCases.Vocabularies.Dtos;

public sealed class VocabularySearchItemMapping : Profile
{
    public VocabularySearchItemMapping()
    {
        CreateMap<Domain.Aggregates.Vocabulary, VocabularySearchItem>();
    }
}
