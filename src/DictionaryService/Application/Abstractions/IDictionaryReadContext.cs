using DictionaryService.Domain.Aggregates;

namespace DictionaryService.Application.Abstractions;

public interface IDictionaryReadContext
{
    IQueryable<DictionaryEntry> DictionaryEntries { get; }
    IQueryable<DictionarySense> DictionarySenses { get; }
    IQueryable<DictionaryExample> DictionaryExamples { get; }
}