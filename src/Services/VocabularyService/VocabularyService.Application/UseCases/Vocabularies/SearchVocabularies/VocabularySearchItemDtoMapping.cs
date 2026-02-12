using AutoMapper;
using VocabularyService.Domain.Aggregates;

namespace VocabularyService.Application.UseCases.Vocabularies.SearchVocabularies;

public sealed class VocabularySearchItemDtoMapping : Profile
{
    public VocabularySearchItemDtoMapping()
    {
        CreateMap<Vocabulary, VocabularySearchItemDto>();
    }
}