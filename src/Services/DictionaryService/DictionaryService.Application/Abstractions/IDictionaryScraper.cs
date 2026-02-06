using DictionaryService.Application.Models;

namespace DictionaryService.Application.Abstractions;

public interface IDictionaryScraper
{
    Task<DictionaryScrapeResult?> LookupAsync(string keyword);
}