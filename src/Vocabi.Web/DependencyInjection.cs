using Vocabi.Web.Services.Navigation;

namespace Vocabi.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddUIServices(this IServiceCollection services)
    {
        services.AddHttpClient();

        services.AddScoped<INavigationService, NavigationService>();
        
        return services;
    }
}
