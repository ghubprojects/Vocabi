using BuildingBlocks.Infrastructure.Extensions;
using MediaService.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MediaService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddVocabularyServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions(configuration);

        return services;
    }

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServiceOptions<StorageOptions>(configuration);

        //services.Configure<PixabaySettings>(configuration.GetSection(nameof(PixabaySettings)));

        return services;
    }
}
