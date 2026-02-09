using BuildingBlocks.Infrastructure.Extensions;
using BuildingBlocks.Infrastructure.Persistence.Abstractions;
using DictionaryService.Application.Abstractions;
using DictionaryService.Domain.Aggregates.DictionaryEntries;
using DictionaryService.Infrastructure.Configurations;
using DictionaryService.Infrastructure.Persistence;
using DictionaryService.Infrastructure.Persistence.Interceptors;
using DictionaryService.Infrastructure.Persistence.QueryServices;
using DictionaryService.Infrastructure.Persistence.Repositories;
using DictionaryService.Infrastructure.Persistence.Seeding;
using DictionaryService.Infrastructure.Scraping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DictionaryService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDictionaryServiceInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions(configuration);

        services.AddDatabase();

        services.AddScoped<IDictionaryEntryQueryService, DictionaryEntryQueryService>();
        services.AddScoped<IDictionaryEntryRepository, DictionaryEntryRepository>();

        services.AddScoped<IDictionaryScraper, CambridgeDictionaryScraper>();

        //// Register seeders
        //services.AddScoped<PronunciationSeeder>();

        //// Register repositories
        //services.AddScoped<IVocabularyRepository, VocabularyRepository>();
        //services.AddScoped<ILookupEntryRepository, LookupEntryRepository>();
        //services.AddScoped<IMediaFileRepository, MediaFileRepository>();
        //services.AddScoped<IPronunciationRepository, PronunciationRepository>();

        //// Register services
        //services.AddScoped<IFileDownloader, FileDownloader>();

        //// Register storage
        //services.AddScoped<IFileStorage, LocalFileStorage>();

        //// Register external providers
        //services.AddScoped<IMainDictionaryProvider, CambridgeProvider>();
        //services.AddScoped<IFallbackDictionaryProvider, CovietProvider>();
        //services.AddScoped<IAudioProvider, GoogleTtsProvider>();
        //services.AddScoped<IImageProvider, PixabayProvider>();

        //services.AddScoped<IAnkiConnectClient, AnkiConnectClient>();
        //services.AddScoped<IFlashcardService, AnkiService>();

        //services.AddScoped<IPerformanceRedactor, DefaultPerformanceRedactor>();

        //// TODO: Enable when authentication is implemented
        ////services.AddHttpContextAccessor();
        //if (env.IsDevelopment())
        //    services.AddScoped<ICurrentUserAccessor, DevHttpContextCurrentUserAccessor>();
        //else
        //    services.AddScoped<ICurrentUserAccessor, HttpContextCurrentUserAccessor>();

        return services;
    }

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServiceOptions<DatabaseOptions>(configuration);
        services.AddServiceOptions<DictionaryScrapingOptions>(configuration);

        //services.Configure<PixabaySettings>(configuration.GetSection(nameof(PixabaySettings)));

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddScoped<ISaveChangesInterceptor, DomainEventInterceptor>();

        services.AddDbContext<DictionaryContext>((provider, options) =>
        {
            var databaseOptions = provider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            var interceptors = provider.GetServices<ISaveChangesInterceptor>();

            options.AddInterceptors(interceptors)
                .UseNpgsql(databaseOptions.ConnectionString)
                .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IDatabaseSeeder, DictionarySeeder>();
        services.AddScoped<IDbContextInitializer, DictionaryContextInitializer>();

        return services;
    }
}