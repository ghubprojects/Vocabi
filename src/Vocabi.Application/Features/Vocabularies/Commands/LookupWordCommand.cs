using MediatR;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Services;
using Vocabi.Application.Services.Media;
using Vocabi.Domain.Aggregates.LookupEntries;
using Vocabi.Domain.Aggregates.MediaFiles;

namespace Vocabi.Application.Features.Vocabularies.Commands;

public class LookupWordCommand : IRequest<Result>
{
    public string Word { get; set; } = string.Empty;
}

public class LookupWordCommandHandler(
    ILookupEntryRepository lookupEntryRepository,
    DictionaryLookupService dictionaryLookupService,
    MediaDownloadService mediaDownloadService
    ) : IRequestHandler<LookupWordCommand, Result>
{
    public async Task<Result> Handle(LookupWordCommand request, CancellationToken cancellationToken)
    {
        // Lookup the word using the dictionaries
        var dictionaryLookupResult = await dictionaryLookupService.LookupAsync(request.Word);
        if (dictionaryLookupResult.IsFailure)
            return Result.Failure([.. dictionaryLookupResult.Errors]);

        // Handle data from the dictionary lookup
        var dictionaryEntryModels = dictionaryLookupResult.Data;
        var lookupEntries = new List<LookupEntry>();

        foreach (var data in dictionaryEntryModels)
        {
            var lookupEntry = LookupEntry.CreateNew(
                data.Headword,
                data.PartOfSpeech,
                data.Pronunciation);

            // Add meanings and definitions
            lookupEntry.AddMeanings(data.Meanings);
            foreach (var definition in data.Definitions)
                lookupEntry.AddDefinitionWithExamples(definition.Text, definition.Examples);

            // Download and add media files
            var urls = new Dictionary<MediaType, string?>
            {
                { MediaType.Audio, data.AudioUrl },
                { MediaType.Image, data.ImageUrl }
            };
            var mediaFileIds = await mediaDownloadService.DownloadAllMediaAsync(data.Headword, urls, data.EntrySource, cancellationToken);
            lookupEntry.AttachMediaFiles(mediaFileIds);

            // Add the lookup entry to the list
            lookupEntries.Add(lookupEntry);
        }

        // Save the lookup entries to the repository
        await lookupEntryRepository.AddRangeAsync(lookupEntries);
        var isSaveSuccess = await lookupEntryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        // TODO: Mapping to return DTOs

        return isSaveSuccess
            ? Result.Success()
            : Result.Failure("Failed to save the lookup entry to the repository.");
    }
}