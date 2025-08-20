using AngleSharp;
using AngleSharp.Dom;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.External.Dictionary;

namespace Vocabi.Infrastructure.External.Dictionary;

public class CovietProvider : IFallbackDictionaryProvider
{
    public string ProviderName => "Coviet Dictionary";

    private readonly IBrowsingContext context;

    public CovietProvider()
    {
        var config = Configuration.Default.WithDefaultLoader();
        context = BrowsingContext.New(config);
    }

    public async Task<Result<List<string>>> LookupAsync(string word)
    {
        try
        {
            var result = await LookupMeaningsAsync(word);
            return Result<List<string>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<List<string>>.Failure(ex.Message);
        }
    }

    private async Task<IDocument> GetDocumentAsync(string word)
    {
        var url = $"https://tratu.coviet.vn/hoc-tieng-anh/tu-dien/lac-viet/A-V/{Uri.EscapeDataString(word)}.html";
        return await context.OpenAsync(url);
    }

    private async Task<List<string>> LookupMeaningsAsync(string word)
    {
        var document = await GetDocumentAsync(word);
        var meaningElements = document.QuerySelectorAll("div#divContent div.m > span");

        if (meaningElements.Length == 0)
            return Result<List<string>>.Failure($"Word '{word}' not found.").Data;

        return [.. meaningElements.Select(el => el.TextContent.Trim())];
    }
}