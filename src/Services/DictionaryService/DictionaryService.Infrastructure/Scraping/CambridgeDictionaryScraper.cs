using AngleSharp;
using AngleSharp.Dom;
using DictionaryService.Application.Abstractions;
using DictionaryService.Application.Models;
using DictionaryService.Infrastructure.Configurations;
using Microsoft.Extensions.Options;

namespace DictionaryService.Infrastructure.Scraping;

public class CambridgeDictionaryScraper : IDictionaryScraper
{
    private readonly DictionaryScrapingOptions _options;
    private readonly IBrowsingContext _context;

    public CambridgeDictionaryScraper(IOptions<DictionaryScrapingOptions> options)
    {
        _options = options.Value;

        var config = Configuration.Default.WithDefaultLoader();
        _context = BrowsingContext.New(config);
    }

    public Task<DictionaryScrapeResult> LookupAsync(string keyword)
    {
        throw new NotImplementedException();
    }
}
