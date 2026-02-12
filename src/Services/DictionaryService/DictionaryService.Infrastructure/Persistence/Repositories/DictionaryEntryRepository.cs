using BuildingBlocks.Domain.Abstractions;
using DictionaryService.Domain.Aggregates.DictionaryEntries;
using DictionaryService.Domain.Aggregates.DictionaryEntries.Models;
using DictionaryService.Infrastructure.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;

namespace DictionaryService.Infrastructure.Persistence.Repositories;

public class DictionaryEntryRepository(DictionaryContext context) : IDictionaryEntryRepository
{
    public IUnitOfWork UnitOfWork => context;

    public async Task AddAsync(DictionaryEntry entity)
    {
        await context.DictionaryEntries.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<DictionaryEntry> entities)
    {
        await context.DictionaryEntries.AddRangeAsync(entities);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await context.DictionaryEntries.FindAsync(id);
        if (entity != null)
            context.DictionaryEntries.Remove(entity);
    }

    public async Task<bool> ExistsByHeadwordAsync(string headword)
    {
        return await context.DictionaryEntries
             .AnyAsync(e => e.Headword == headword);
    }

    public async Task<bool> ExistsByHeadwordAndPartOfSpeechAsync(string headword, string partOfSpeech)
    {
        return await context.DictionaryEntries
             .AnyAsync(e => e.Headword == headword && e.PartOfSpeech == partOfSpeech);
    }
}