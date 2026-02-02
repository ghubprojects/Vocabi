using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Infrastructure.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddBuildingBlocksInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUser, DevCurrentUserService>();

        return services;
    }
}