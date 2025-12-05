using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Vocabi.Application.Behaviors;
using Vocabi.Application.Services;
using Vocabi.Application.Services.Media;

namespace Vocabi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // MediatR configuration
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // AutoMapper configuration
        services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());

        // Mediator behaviors
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionMappingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>)); // transaction for commands

        // Register services
        services.AddScoped<IMediaService, AudioService>();
        services.AddScoped<IMediaService, ImageService>();
        services.AddScoped<MediaDownloadService>();
        services.AddScoped<DictionaryLookupService>();

        return services;
    }
}
