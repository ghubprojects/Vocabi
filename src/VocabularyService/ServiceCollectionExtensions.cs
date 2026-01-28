using BuildingBlocks.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VocabularyService.Application.Abstractions;
using VocabularyService.Infrastructure.Configuration;
using VocabularyService.Infrastructure.Persistence;
using VocabularyService.Infrastructure.Persistence.Interceptors;

namespace VocabularyService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddVocabularyServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions(configuration)
            .AddDatabase();

        services.AddSingleton(TimeProvider.System);

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

        //services.Configure<PixabaySettings>(configuration.GetSection(nameof(PixabaySettings)));

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, SoftDeleteInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DomainEventInterceptor>();

        services.AddDbContext<VocabularyContext>((provider, options) =>
        {
            var databaseOptions = provider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            var interceptors = provider.GetServices<ISaveChangesInterceptor>();

            options.AddInterceptors(interceptors)
                .UseNpgsql(databaseOptions.ConnectionString)
                .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IVocabularyWriteContext>(provider => provider.GetRequiredService<VocabularyContext>());
        services.AddScoped<IVocabularyReadContext>(provider => provider.GetRequiredService<VocabularyContext>());

        return services;
    }
}