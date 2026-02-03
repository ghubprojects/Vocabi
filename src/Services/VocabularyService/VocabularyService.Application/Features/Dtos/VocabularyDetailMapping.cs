using AutoMapper;

namespace VocabularyService.Application.Features.Dtos;

public sealed class VocabularyDetailMapping : Profile
{
    public VocabularyDetailMapping()
    {
        CreateMap<Domain.Aggregates.Vocabulary, VocabularyDetail>();
    }
}
