using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vocabi.Application.Contracts.External.Audio;
using Vocabi.Application.Contracts.External.Dictionary;
using Vocabi.Application.Contracts.External.Flashcards;
using Vocabi.Application.Contracts.External.Image;
using Vocabi.Application.Contracts.Persistence;
using Vocabi.Application.Contracts.Storage;
using Vocabi.Application.Services.Identity;
using Vocabi.Application.Services.Interfaces;
using Vocabi.Application.Services.Interfaces.DownloadFile;
using Vocabi.Domain.Aggregates.LookupEntries;
using Vocabi.Domain.Aggregates.MediaFiles;
using Vocabi.Domain.Aggregates.Pronunciations;
using Vocabi.Domain.Aggregates.Vocabularies;
using Vocabi.Infrastructure.External.Anki;
using Vocabi.Infrastructure.External.Audio;
using Vocabi.Infrastructure.External.Dictionary;
using Vocabi.Infrastructure.External.Image;
using Vocabi.Infrastructure.Persistence;
using Vocabi.Infrastructure.Persistence.Repositories;
using Vocabi.Infrastructure.Persistence.Seed;
using Vocabi.Infrastructure.Services;
using Vocabi.Infrastructure.Services.Identity;
using Vocabi.Infrastructure.Storage;

namespace Vocabi.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        services.AddSettings(configuration)
            .AddDatabase(configuration);

        // Register seeders
        services.AddScoped<PronunciationSeeder>();

        // Register repositories
        services.AddScoped<IVocabularyRepository, VocabularyRepository>();
        services.AddScoped<ILookupEntryRepository, LookupEntryRepository>();
        services.AddScoped<IMediaFileRepository, MediaFileRepository>();
        services.AddScoped<IPronunciationRepository, PronunciationRepository>();

        // Register services
        services.AddScoped<IFileDownloader, FileDownloader>();

        // Register storage
        services.AddScoped<IFileStorage, LocalFileStorage>();

        // Register external providers
        services.AddScoped<IMainDictionaryProvider, CambridgeProvider>();
        services.AddScoped<IFallbackDictionaryProvider, CovietProvider>();
        services.AddScoped<IAudioProvider, GoogleTtsProvider>();
        services.AddScoped<IImageProvider, PixabayProvider>();

        services.AddScoped<IAnkiConnectClient, AnkiConnectClient>();
        services.AddScoped<IFlashcardService, AnkiService>();

        services.AddScoped<IPerformanceRedactor, DefaultPerformanceRedactor>();

        // TODO: Enable when authentication is implemented
        //services.AddHttpContextAccessor();
        if (env.IsDevelopment())
            services.AddScoped<ICurrentUserAccessor, DevHttpContextCurrentUserAccessor>();
        else
            services.AddScoped<ICurrentUserAccessor, HttpContextCurrentUserAccessor>();

        return services;
    }

    private static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PixabaySettings>(configuration.GetSection(nameof(PixabaySettings)));
        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
        return services;
    }
}