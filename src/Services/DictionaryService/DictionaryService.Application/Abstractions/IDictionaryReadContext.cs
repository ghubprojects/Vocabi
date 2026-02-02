using DictionaryService.Domain.Aggregates;

namespace DictionaryService.Application.Abstractions;

public interface IDictionaryReadContext
{
    IQueryable<DictionaryEntry> DictionaryEntries { get; }
    IQueryable<DictionaryDefinition> DictionaryDefinitions { get; }
    IQueryable<DictionaryExample> DictionaryExamples { get; }
}