using BuildingBlocks.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using VocabularyService.Domain.Aggregates;

namespace VocabularyService.Application.Abstractions;

public interface IVocabularyWriteContext : IUnitOfWork
{
    DbSet<Vocabulary> Vocabularies { get; }
}