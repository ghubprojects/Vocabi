using DictionaryService.Application;
using DictionaryService.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DictionaryService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddDictionaryService(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDictionaryServiceApplication()
            .AddDictionaryServiceInfrastructure(configuration);

        return services;
    }
}