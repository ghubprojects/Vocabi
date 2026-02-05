using AngleSharp;
using AngleSharp.Dom;
using DictionaryService.Application.Abstractions;
using DictionaryService.Application.Models;
using DictionaryService.Infrastructure.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DictionaryService.Infrastructure.Scraping;

public class CambridgeDictionaryScraper : IDictionaryScraper
{
    private readonly DictionaryScrapingOptions _options;
    private readonly ILogger<CambridgeDictionaryScraper> _logger;

    private readonly IBrowsingContext _context;

    public CambridgeDictionaryScraper(IOptions<DictionaryScrapingOptions> options, ILogger<CambridgeDictionaryScraper> logger)
    {
        _options = options.Value;
        _logger = logger;

        var config = Configuration.Default.WithDefaultLoader();
        _context = BrowsingContext.New(config);
    }

    public async Task<DictionaryScrapeResult?> LookupAsync(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return null;

        var document = await LoadDocumentAsync(keyword);

        var entryElements = document.SelectElements(CambridgeDictionarySelectors.Entry);
        if (!entryElements.Any())
            return null;

        var entries = ParseEntries(entryElements);
        if (entries.Count == 0)
            return null;

        return entries.Count == 0
            ? null
            : new DictionaryScrapeResult(entries);
    }

    private async Task<IDocument> LoadDocumentAsync(string keyword)
    {
        var encoded = Uri.EscapeDataString(keyword.Trim().ToLowerInvariant());
        var url = $"{_options.BaseUrl}/{encoded}";
        return await _context.OpenAsync(url);
    }

    private List<ScrapedDictionaryEntry> ParseEntries(IEnumerable<IElement> entryElements)
    {
        var results = new List<ScrapedDictionaryEntry>();

        foreach (var entryElement in entryElements)
        {
            try
            {
                var headword = entryElement.SelectText(CambridgeDictionarySelectors.Headword);
                var partOfSpeech = entryElement.SelectText(CambridgeDictionarySelectors.PartOfSpeech);
                var pronunciation = entryElement.SelectText(CambridgeDictionarySelectors.Pronunciation);

                var definitions = ParseDefinitions(entryElement);
                if (definitions.Count == 0)
                    continue;

                results.Add(
                    new ScrapedDictionaryEntry(
                        headword,
                        phonetic,
                        definitions));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to parse dictionary entry block");
            }
        }

        return results;
    }

    private static List<ScrapedDictionaryDefinition> ParseDefinitions(IElement entry)
    {
        var results = new List<ScrapedDictionaryDefinition>();

        foreach (var posHeader in posHeaders)
        {
            var defBlocks = posHeader
                .ParentElement?
                .QueryAll(CambridgeDictionarySelectors.DefinitionBlocks)
                ?? Enumerable.Empty<IElement>();

            foreach (var defBlock in defBlocks)
            {
                var definition = defBlock
                    .QueryFirst(CambridgeDictionarySelectors.DefinitionText)
                    ?.TextContent
                    .Trim();

                if (string.IsNullOrWhiteSpace(definition))
                    continue;

                var examples = defBlock
                    .QueryAll(CambridgeDictionarySelectors.Examples)
                    .Select(x => x.TextContent.Trim())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToList();

                results.Add(
                    new ScrapedDictionaryDefinition(
                        definition,
                        examples));
            }
        }

        return results;
    }
}