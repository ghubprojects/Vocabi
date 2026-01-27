using BuildingBlocks.Application.Abstractions;
using DictionaryService.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace DictionaryService.Application.Abstractions;

public interface IDictionaryWriteContext : IUnitOfWork
{
    DbSet<DictionaryEntry> DictionaryEntries { get; }
}