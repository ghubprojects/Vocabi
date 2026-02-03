using BuildingBlocks.Application;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace VocabularyService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddVocabularyServiceApplication(this IServiceCollection services)
    {
        services.AddBuildingBlocksApplication(Assembly.GetExecutingAssembly());

        return services;
    }
}