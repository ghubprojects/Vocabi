using DictionaryService.Domain.Aggregates;

namespace DictionaryService.Application.Abstractions;

public interface IDictionaryReadContext
{
    IQueryable<DictionaryEntry> DictionaryEntries { get; }
    IQueryable<DictionaryDefinition> DictionarySenses { get; }
    IQueryable<DictionaryExample> DictionaryExamples { get; }
}