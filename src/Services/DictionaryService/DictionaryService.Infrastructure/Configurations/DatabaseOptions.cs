using BuildingBlocks.Infrastructure.Configuration;
using BuildingBlocks.Infrastructure.Persistence.Enums;

namespace DictionaryService.Infrastructure.Configurations;

public sealed class DatabaseOptions : IOptionsSection, IServiceOptions
{
    public static string ServiceName => "DictionaryService";
    public static string SectionName => "Database";

    public string Provider { get; init; } = string.Empty;
    public string ConnectionString { get; init; } = string.Empty;
    public DatabaseInitMode InitMode { get; set; } = DatabaseInitMode.None;
}