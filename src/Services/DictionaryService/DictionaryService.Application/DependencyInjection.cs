using BuildingBlocks.Application;
using DictionaryService.Domain.Aggregates.DictionaryEntries;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DictionaryService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddDictionaryServiceApplication(this IServiceCollection services)
    {
        services.AddBuildingBlocksApplication(Assembly.GetExecutingAssembly());

        services.AddScoped<DictionaryEntryDomainService>();

        return services;
    }
}