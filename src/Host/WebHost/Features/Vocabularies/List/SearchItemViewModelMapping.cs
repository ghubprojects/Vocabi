using AutoMapper;
using VocabularyService.Application.UseCases.Vocabularies.SearchVocabularies;

namespace WebHost.Features.Vocabularies.List;

public sealed class SearchItemViewModelMapping : Profile
{
    public SearchItemViewModelMapping()
    {
        CreateMap<VocabularySearchItemDto, SearchItemViewModel>();
    }
}