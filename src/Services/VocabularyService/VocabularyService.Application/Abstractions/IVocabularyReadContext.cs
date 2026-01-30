using VocabularyService.Domain.Aggregates;

namespace VocabularyService.Application.Abstractions;

public interface IVocabularyReadContext
{
    IQueryable<Vocabulary> Vocabularies { get; }
    IQueryable<VocabularyExample> VocabularyExamples { get; }
}