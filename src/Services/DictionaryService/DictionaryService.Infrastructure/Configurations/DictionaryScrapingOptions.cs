using BuildingBlocks.Infrastructure.Configuration;

namespace DictionaryService.Infrastructure.Configurations;

public sealed class DictionaryScrapingOptions : IOptionsSection, IServiceOptions
{
    public static string ServiceName => "DictionaryService";
    public static string SectionName => "DictionaryScraping";

    public string Provider { get; init; } = string.Empty;
    public string BaseUrl { get; init; } = string.Empty;
}