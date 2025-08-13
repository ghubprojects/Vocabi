using AngleSharp;
using AngleSharp.Dom;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.External.Dictionary;

namespace Vocabi.Infrastructure.External.Dictionary;

public class CambridgeProvider : IMainDictionaryProvider
{
    public string ProviderName => "Cambridge Dictionary";

    private readonly IBrowsingContext _context;

    public CambridgeProvider()
    {
        var config = Configuration.Default.WithDefaultLoader();
        _context = BrowsingContext.New(config);
    }

    public async Task<Result<List<DictionaryEntryModel>>> LookupAsync(string word)
    {
        try
        {
            var entries = await LookupEntriesAsync(word);
            var result = await EnrichWithMeaningAsync(entries, word);
            return Result<List<DictionaryEntryModel>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<List<DictionaryEntryModel>>.Failure(ex.Message);
        }
    }

    private async Task<IDocument> GetDocumentAsync(string word, string langPath)
    {
        var url = $"https://dictionary.cambridge.org/dictionary/{langPath}/{Uri.EscapeDataString(word)}";
        return await _context.OpenAsync(url);
    }

    private async Task<List<DictionaryEntryModel>> LookupEntriesAsync(string word)
    {
        // For phrases, return a fallback entry
        var isPhrase = word.Contains(' ');
        if (isPhrase)
        {
            return [new DictionaryEntryModel
            {
                Headword = word,
                PartOfSpeech = "phrase",
                Pronunciation = await BuildPhrasePronunciationAsync(word),
            }];
        }

        // For single words, fetch the entries from the Cambridge Dictionary
        var document = await GetDocumentAsync(word, "english");
        var entryElements = document.QuerySelectorAll("div.pr.dictionary[data-id='cald4'] div.pr.entry-body__el");

        if (entryElements.Length == 0)
            return Result<List<DictionaryEntryModel>>.Failure($"Word '{word}' not found.").Data;

        var entries = new List<DictionaryEntryModel>();
        foreach (var entryElement in entryElements)
        {
            var entryHeaderElement = entryElement.QuerySelector("div.pos-header");
            var pronunciationElement = entryHeaderElement?.QuerySelector("span.us.dpron-i");
            var entryBodyElement = entryElement.QuerySelector("div.pos-body");

            var baseUrl = "https://dictionary.cambridge.org";
            var audioSourceElement = pronunciationElement?.QuerySelector("audio > source[type='audio/mpeg']");
            var imageElement = entryBodyElement?.QuerySelector("amp-img.dimg_i");

            entries.Add(new DictionaryEntryModel
            {
                Headword = entryHeaderElement?.QuerySelector("span.hw")?.TextContent.Trim() ?? "",
                PartOfSpeech = entryHeaderElement?.QuerySelector("span.pos")?.TextContent.Trim() ?? "",
                Pronunciation = pronunciationElement?.QuerySelector("span.ipa.dipa")?.TextContent.Trim() ?? "",
                AudioUrl = audioSourceElement is not null ? $"{baseUrl}{audioSourceElement?.GetAttribute("src")}" : "",
                ImageUrl = imageElement is not null ? $"{baseUrl}{imageElement?.GetAttribute("src")}" : "",
                Definitions = ParseDefinitions(entryBodyElement),
                EntrySource = ProviderName,
            });
        }
        return entries;
    }

    private static List<DictionaryDefinitionModel> ParseDefinitions(IElement? entryBodyElement)
    {
        var definitionElements = entryBodyElement?.QuerySelectorAll("div.sense-body.dsense_b > div.def-block.ddef_block");
        if (definitionElements is null || definitionElements.Length == 0)
            return [];

        return [.. definitionElements.Select(definitionElement => new DictionaryDefinitionModel
        {
            Text = definitionElement.QuerySelector("div.def.ddef_d.db")?.TextContent.Trim() ?? "",
            Examples = [.. definitionElement.QuerySelectorAll("div.examp.dexamp > span.eg.deg").Select(e => e.TextContent.Trim())]
        })];
    }

    private async Task<string> BuildPhrasePronunciationAsync(string phrase)
    {
        var words = phrase.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var pronunciations = new List<string>();
        foreach (var word in words)
        {
            var pronunciation = await GetPronunciationAsync(word);
            pronunciations.Add(pronunciation);
        }

        return string.Join(" ", pronunciations);
    }

    private async Task<string> GetPronunciationAsync(string word)
    {
        var document = await GetDocumentAsync(word, "english");
        var firstEntryElement = document.QuerySelector("div.pr.dictionary[data-id='cald4'] div.pr.entry-body__el");
        var entryHeaderElement = firstEntryElement?.QuerySelector("div.pos-header");
        var pronunciationElement = entryHeaderElement?.QuerySelector("span.us.dpron-i");
        return pronunciationElement?.QuerySelector("span.ipa.dipa")?.TextContent.Trim() ?? "";
    }

    private async Task<List<DictionaryEntryModel>> EnrichWithMeaningAsync(List<DictionaryEntryModel> entries, string word)
    {
        var document = await GetDocumentAsync(word, "english-vietnamese");
        var entryElements = document.QuerySelectorAll("div.entry-body span.link.dlink");

        foreach (var entryElement in entryElements)
        {
            var entryHeaderElement = entryElement.QuerySelector("div.di-head");
            var headword = entryHeaderElement?.QuerySelector(".di-title")?.TextContent.Trim() ?? "";
            var partOfSpeech = entryHeaderElement?.QuerySelector("span.di-info span.pos.dpos")?.TextContent.Trim() ?? "";

            var entry = entries
                .FirstOrDefault(e => e.Headword.Equals(headword, StringComparison.OrdinalIgnoreCase)
                && e.PartOfSpeech.Equals(partOfSpeech, StringComparison.OrdinalIgnoreCase));

            if (entry is null)
                continue;

            var entryBodyElement = entryElement.QuerySelector("div.di-body");
            var meanings = entryBodyElement
                ?.QuerySelectorAll("div.sense-body.dsense_b > div.def-block.ddef_block div.def-body > span.trans.dtrans")
                .Select(element => element.TextContent.Trim() ?? "")
                .ToList();
            entry.Meanings = meanings ?? [];
            entry.MeaningSource = ProviderName;
        }
        return entries;
    }
}