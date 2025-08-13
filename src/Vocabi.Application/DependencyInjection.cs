using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
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

        // Register services
        services.AddScoped<IMediaService, AudioService>();
        services.AddScoped<IMediaService, ImageService>();
        services.AddScoped<MediaDownloadService>();
        services.AddScoped<DictionaryLookupService>();

        return services;
    }
}
