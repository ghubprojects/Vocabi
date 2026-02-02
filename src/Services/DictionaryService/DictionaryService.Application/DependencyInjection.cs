using BuildingBlocks.Application;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DictionaryService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddDictionaryServiceApplication(this IServiceCollection services)
    {
        services.AddBuildingBlocksApplication(Assembly.GetExecutingAssembly());

        return services;
    }
}