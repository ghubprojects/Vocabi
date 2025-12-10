using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Vocabi.Application.Behaviors;
using Vocabi.Application.Common.Configurations;
using Vocabi.Application.Services;
using Vocabi.Application.Services.Media;

namespace Vocabi.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // MediatR configuration
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // AutoMapper configuration
        services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());

        // Register pipeline behaviors 
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(GlobalExceptionBehavior<,>));

        // Configure settings
        services.Configure<PerformanceSettings>(configuration.GetSection("PerformanceSettings"));

        // Register services
        services.AddScoped<IMediaService, AudioService>();
        services.AddScoped<IMediaService, ImageService>();
        services.AddScoped<MediaDownloadService>();
        services.AddScoped<DictionaryLookupService>();

        return services;
    }
}
