using AutoMapper;

namespace VocabularyService.Application.UseCases.Vocabularies.GetVocabulary;

public sealed class VocabularyDetailDtoMapping : Profile
{
    public VocabularyDetailDtoMapping()
    {
        CreateMap<Domain.Aggregates.Vocabulary, VocabularyDetailDto>();
    }
}
