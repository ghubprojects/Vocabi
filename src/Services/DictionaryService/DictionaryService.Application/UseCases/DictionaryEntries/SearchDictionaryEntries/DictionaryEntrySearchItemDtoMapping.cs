using AutoMapper;
using DictionaryService.Domain.Aggregates.DictionaryEntries.Models;

namespace DictionaryService.Application.UseCases.DictionaryEntries.SearchDictionaryEntries;

public sealed class DictionaryEntrySearchItemDtoMapping : Profile
{
    public DictionaryEntrySearchItemDtoMapping()
    {
        CreateMap<DictionaryEntry, DictionaryEntrySearchItemDto>();
    }
}