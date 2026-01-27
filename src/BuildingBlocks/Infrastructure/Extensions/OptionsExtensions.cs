using BuildingBlocks.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure.Extensions;

public static class OptionsExtensions
{
    public static IServiceCollection AddServiceOptions<T>(this IServiceCollection services, IConfiguration configuration)
        where T : class, IOptionsSection, IServiceOptions
    {
        services.Configure<T>(configuration.GetSection($"{T.ServiceName}:{T.SectionName}"));

        return services;
    }
}