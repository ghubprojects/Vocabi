using BuildingBlocks.Application.Behaviors;
using BuildingBlocks.Infrastructure.Extensions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationBehaviors(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(GlobalExceptionBehavior<,>));

          // Configure settings
        services.Configure<PerformanceSettings>(configuration.GetSection("PerformanceSettings"));

        return services;
    }

      private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServiceOptions<DatabaseOptions>(configuration);

        //services.Configure<PixabaySettings>(configuration.GetSection(nameof(PixabaySettings)));

        return services;
    }
}
