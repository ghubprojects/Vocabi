using System.Reflection;
using Vocabi.Web.Services.Navigation;

namespace Vocabi.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddUIServices(this IServiceCollection services)
    {
        // AutoMapper configuration
        services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());

        services.AddHttpClient();

        services.AddScoped<INavigationService, NavigationService>();

        return services;
    }
}
