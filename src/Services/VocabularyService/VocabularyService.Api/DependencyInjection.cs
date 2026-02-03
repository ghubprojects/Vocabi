using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VocabularyService.Application;
using VocabularyService.Infrastructure;

namespace VocabularyService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddVocabularyService(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddVocabularyServiceApplication()
            .AddVocabularyServiceInfrastructure(configuration);

        return services;
    }
}