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
    private const string SourceName = "Cambridge Dictionary";

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
        if (document is null)
            return null;

        var entryElements = document.SelectElements(CambridgeDictionarySelectors.EntryBlock);
        if (!entryElements.Any())
            return null;

        var entries = ParseEntries(entryElements);

        return entries.Count > 0
            ? new DictionaryScrapeResult(entries)
            : null;
    }

    private async Task<IDocument> LoadDocumentAsync(string keyword)
    {
        var encoded = Uri.EscapeDataString(keyword.Trim().ToLowerInvariant());
        var url = $"{_options.BaseUrl}/{encoded}";
        return await _context.OpenAsync(url);
    }

    private List<ScrapedDictionaryEntry> ParseEntries(IEnumerable<IElement> entryElements)
    {
        var result = new List<ScrapedDictionaryEntry>();

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

                var scrapedEntry = new ScrapedDictionaryEntry(
                    headword,
                    partOfSpeech,
                    pronunciation,
                    SourceName,
                    definitions);

                result.Add(scrapedEntry);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to parse dictionary entry block");
            }
        }

        return result;
    }

    private static List<ScrapedDictionaryDefinition> ParseDefinitions(IElement entryElement)
    {
        var result = new List<ScrapedDictionaryDefinition>();

        var definitionElements = entryElement.SelectElements(CambridgeDictionarySelectors.DefinitionBlock);

        foreach (var definitionElement in definitionElements)
        {
            var definition = definitionElement
                .SelectText(CambridgeDictionarySelectors.Definition)
                .TrimEnd(':');

            if (string.IsNullOrWhiteSpace(definition))
                continue;

            var examples = definitionElement.SelectTexts(CambridgeDictionarySelectors.Example);

            var scrapedDefinition = new ScrapedDictionaryDefinition(definition, examples);
            result.Add(scrapedDefinition);
        }

        return result;
    }
}