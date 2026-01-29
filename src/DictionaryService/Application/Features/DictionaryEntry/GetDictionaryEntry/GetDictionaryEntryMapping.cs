using AutoMapper;

namespace DictionaryService.Application.Features.DictionaryEntry.GetDictionaryEntry;

public sealed class GetDictionaryEntryMapping : Profile
{
    public GetDictionaryEntryMapping()
    {
        CreateMap<Domain.Aggregates.DictionaryEntry, GetDictionaryEntryResult.DictionaryEntryDetail>();
    }
}