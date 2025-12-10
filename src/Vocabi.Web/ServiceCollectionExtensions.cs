using Serilog;
using System.Reflection;
using Vocabi.Web.Services.Navigation;

namespace Vocabi.Web;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        // AutoMapper configuration
        services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());

        services.AddHttpClient();

        services.AddScoped<INavigationService, NavigationService>();

        return services;
    }
}
