using BuildingBlocks.Application.Abstractions;
using DictionaryService.Application.Abstractions;
using DictionaryService.Domain.Aggregates.DictionaryEntries;
using FluentResults;

namespace DictionaryService.Application.UseCases.DictionaryEntries.Commands.LookupDictionary;

public sealed class LookupDictionaryHandler(
    IDictionaryEntryRepository repository,
    IDictionaryScraper scraper,
    DictionaryEntryDomainService domainService)
    : ICommandHandler<LookupDictionaryCommand, Result>
{
    public async Task<Result> Handle(LookupDictionaryCommand request, CancellationToken cancellationToken)
    {
        var keyword = request.Keyword.Trim().ToLowerInvariant();

        var exists = await repository.ExistsByHeadwordAsync(keyword);
        if (exists)
            return Result.Fail("Dictionary entry with the same headword already exists.");

        var scrapeResult = await scraper.LookupAsync(keyword);
        if (scrapeResult is null)
            return Result.Fail("The search term does not match any dictionary entries.");

        foreach (var scrapedEntry in scrapeResult.Entries)
        {
            var entry = await domainService.CreateAsync(
                scrapedEntry.Headword,
                scrapedEntry.PartOfSpeech,
                scrapedEntry.Pronunciation,
                scrapedEntry.Source);

            foreach (var definition in scrapedEntry.Definitions)
                entry.AddDefinition(definition.Text, definition.Examples);

            await repository.AddAsync(entry);
        }

        await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}