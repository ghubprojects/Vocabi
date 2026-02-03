using AutoMapper;

namespace VocabularyService.Application.Features.Dtos;

public sealed class VocabularySearchItemMapping : Profile
{
    public VocabularySearchItemMapping()
    {
        CreateMap<Domain.Aggregates.Vocabulary, VocabularySearchItem>();
    }
}
