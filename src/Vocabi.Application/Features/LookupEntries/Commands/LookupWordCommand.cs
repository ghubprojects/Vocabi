using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Features.LookupEntries.DTOs;
using Vocabi.Application.Services;
using Vocabi.Application.Services.Media;
using Vocabi.Domain.Aggregates.LookupEntries;
using Vocabi.Domain.Aggregates.MediaFiles;

namespace Vocabi.Application.Features.LookupEntries.Commands;

public class LookupWordCommand : IRequest<Result<List<LookupEntryDto>>>
{
    public string Word { get; set; } = string.Empty;
}

public class LookupWordCommandHandler(
    DictionaryLookupService dictionaryLookupService,
    MediaDownloadService mediaDownloadService,
    ILookupEntryRepository lookupEntryRepository,
    IMapper mapper,
    ILogger<LookupWordCommandHandler> logger
    ) : IRequestHandler<LookupWordCommand, Result<List<LookupEntryDto>>>
{
    public async Task<Result<List<LookupEntryDto>>> Handle(LookupWordCommand request, CancellationToken cancellationToken)
    {
        // Lookup the word using the dictionaries
        var dictionaryLookupResult = await dictionaryLookupService.LookupAsync(request.Word);
        if (dictionaryLookupResult.IsFailure)
            return Result<List<LookupEntryDto>>.Failure(dictionaryLookupResult.Errors);

        // Handle data from the dictionary lookup
        var dictionaryEntryModels = dictionaryLookupResult.Data;
        var mediaLookup = new Dictionary<string, List<Guid>>();
        var lookupEntries = new List<LookupEntry>();

        foreach (var data in dictionaryEntryModels)
        {
            if (!mediaLookup.ContainsKey(data.Headword))
            {
                var urls = new Dictionary<MediaType, string?> { { MediaType.Audio, data.AudioUrl }, { MediaType.Image, data.ImageUrl } };
                var mediaFileIds = await mediaDownloadService.DownloadAllMediaAsync(data.Headword, urls, data.EntrySource, cancellationToken);
                mediaLookup[data.Headword] = mediaFileIds;
            }

            var lookupEntry = LookupEntry.CreateNew(
                data.Headword,
                data.PartOfSpeech,
                data.Pronunciation);

            lookupEntry.AddMeanings(data.Meanings);
            foreach (var definition in data.Definitions)
                lookupEntry.AddDefinitionWithExamples(definition.Text, definition.Examples);

            lookupEntry.AttachMediaFiles(mediaLookup[data.Headword]);

            lookupEntries.Add(lookupEntry);
        }

        // Save the lookup entries to the repository
        await lookupEntryRepository.AddRangeAsync(lookupEntries);
        var isSaveSuccess = await lookupEntryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        if (!isSaveSuccess)
            return Result<List<LookupEntryDto>>.Failure("Failed to save the lookup entry to the repository.");

        return Result<List<LookupEntryDto>>.Success(mapper.Map<List<LookupEntryDto>>(lookupEntries));
    }
}