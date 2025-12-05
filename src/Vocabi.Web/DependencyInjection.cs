using Serilog;
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

        // Serilog
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "MyApp")
            .WriteTo.Seq("http://seq:5341")
            .CreateLogger();
        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());

        services.AddScoped<INavigationService, NavigationService>();

        return services;
    }
}
