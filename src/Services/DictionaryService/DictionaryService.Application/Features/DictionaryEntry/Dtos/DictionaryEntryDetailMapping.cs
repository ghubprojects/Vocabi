using AutoMapper;

namespace DictionaryService.Application.Features.DictionaryEntry.Dtos;

public sealed class GetDictionaryEntryMapping : Profile
{
    public GetDictionaryEntryMapping()
    {
        CreateMap<Domain.Aggregates.DictionaryEntry.DictionaryEntry, DictionaryEntryDetail>();
    }
}