using AutoMapper;
using DictionaryService.Domain.Aggregates;

namespace DictionaryService.Application.Features.DictionaryEntry.GetDictionaryEntry;

public sealed class GetDictionaryEntryMapping : Profile
{
    public GetDictionaryEntryMapping()
    {
        CreateMap<DictionaryEntry, GetDictionaryEntryResult.DictionaryEntryDetail>();
    }
}