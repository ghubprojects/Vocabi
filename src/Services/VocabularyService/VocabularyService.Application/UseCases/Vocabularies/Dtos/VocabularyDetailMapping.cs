using AutoMapper;

namespace VocabularyService.Application.UseCases.Vocabularies.Dtos;

public sealed class VocabularyDetailMapping : Profile
{
    public VocabularyDetailMapping()
    {
        CreateMap<Domain.Aggregates.Vocabulary, VocabularyDetail>();
    }
}
