using BuildingBlocks.Infrastructure.Persistence.Abstractions;
using Serilog;
using System.Reflection;
using Vocabi.Web.Common.Helpers;
using Vocabi.Web.Services.Navigation;

namespace WebHost;

public static class DependencyInjection
{
    public static IServiceCollection AddWebHost(this IServiceCollection services)
    {
        // AutoMapper configuration
        services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());

        services.AddHttpClient();

        services.AddScoped<INavigationService, NavigationService>();

        services.AddScoped<IActionExecutor, ActionExecutor>();

        return services;
    }

    public static async Task InitializeDatabaseAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var initializers = scope.ServiceProvider.GetServices<IDbContextInitializer>();

        foreach (var initializer in initializers)
            await initializer.InitializeAsync();
    }
}
