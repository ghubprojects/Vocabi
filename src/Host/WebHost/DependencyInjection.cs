using BuildingBlocks.Infrastructure.Persistence.Abstractions;
using System.Reflection;
using WebHost.Common.Helpers;
using WebHost.Features.Vocabularies;

namespace WebHost;

public static class DependencyInjection
{
	public static IServiceCollection AddWebHost(this IServiceCollection services)
	{
		services.AddNavigations();

		services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());

		services.AddHttpClient();

		services.AddScoped<IActionExecutor, ActionExecutor>();

		return services;
	}

	public static IServiceCollection AddNavigations(this IServiceCollection services)
	{
		services.AddScoped<VocabularyNavigation>();

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
