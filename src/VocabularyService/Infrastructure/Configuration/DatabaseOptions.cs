using BuildingBlocks.Infrastructure.Configuration;

namespace VocabularyService.Infrastructure.Configuration;

public sealed class DatabaseOptions : IOptionsSection, IServiceOptions
{
    public static string ServiceName => "VocabularyService";
    public static string SectionName => "Database";

    public string Provider { get; init; } = string.Empty;
    public string ConnectionString { get; init; } = string.Empty;
}