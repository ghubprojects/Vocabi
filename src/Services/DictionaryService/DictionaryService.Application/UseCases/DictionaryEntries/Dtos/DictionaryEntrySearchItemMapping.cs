using AutoMapper;
using DictionaryService.Domain.Aggregates.DictionaryEntries.Models;

namespace DictionaryService.Application.UseCases.DictionaryEntries.Dtos;

public sealed class DictionaryEntrySearchItemMapping : Profile
{
    public DictionaryEntrySearchItemMapping()
    {
        CreateMap<DictionaryEntry, DictionaryEntrySearchItem>();
    }
}