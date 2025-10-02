using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vocabi.Application.Contracts.External.Audio;
using Vocabi.Application.Contracts.External.Dictionary;
using Vocabi.Application.Contracts.External.Flashcards;
using Vocabi.Application.Contracts.External.Image;
using Vocabi.Application.Contracts.Services.DownloadFile;
using Vocabi.Application.Contracts.Storage;
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
using Vocabi.Infrastructure.Storage;

namespace Vocabi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext configuration
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

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

        return services;
    }
}