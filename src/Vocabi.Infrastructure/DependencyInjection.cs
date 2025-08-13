using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vocabi.Domain.Aggregates.Vocabularies;
using Vocabi.Infrastructure.Persistence;
using Vocabi.Infrastructure.Persistence.Repositories;

namespace Vocabi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IVocabularyRepository, VocabularyRepository>();

        return services;
    }
}
