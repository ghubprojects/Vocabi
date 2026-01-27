using BuildingBlocks.Infrastructure.Configuration;

namespace MediaService.Infrastructure.Configuration;

public class StorageOptions : IOptionsSection, IServiceOptions
{
    public static string ServiceName => "MediaService";
    public static string SectionName => "Storage";

    public string Container { get; init; } = string.Empty;
    public string ConnectionString { get; init; } = string.Empty;
}
