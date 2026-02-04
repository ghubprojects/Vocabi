using AutoMapper;
using DictionaryService.Domain.Aggregates.DictionaryEntries.Models;

namespace DictionaryService.Application.UseCases.DictionaryEntries.Dtos;

public sealed class DictionaryEntryDetailMapping : Profile
{
    public DictionaryEntryDetailMapping()
    {
        CreateMap<DictionaryEntry, DictionaryEntryDetail>();
    }
}