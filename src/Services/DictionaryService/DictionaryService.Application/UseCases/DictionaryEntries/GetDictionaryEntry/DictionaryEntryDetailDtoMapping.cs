using AutoMapper;
using DictionaryService.Domain.Aggregates.DictionaryEntries.Models;

namespace DictionaryService.Application.UseCases.DictionaryEntries.GetDictionaryEntry;

public sealed class DictionaryEntryDetailDtoMapping : Profile
{
    public DictionaryEntryDetailDtoMapping()
    {
        CreateMap<DictionaryEntry, DictionaryEntryDetailDto>();
    }
}