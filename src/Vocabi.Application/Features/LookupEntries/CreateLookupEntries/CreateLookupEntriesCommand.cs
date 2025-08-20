using MediatR;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Services;
using Vocabi.Application.Services.Media;
using Vocabi.Domain.Aggregates.LookupEntries;
using static Vocabi.Shared.Common.Enums;

namespace Vocabi.Application.Features.LookupEntries.CreateLookupEntries;

public class CreateLookupEntriesCommand : IRequest<Result<IReadOnlyList<Guid>>>
{
    public string Word { get; init; } = string.Empty;
}

public class LookupWordCommandHandler(
    DictionaryLookupService dictionaryLookupService,
    MediaDownloadService mediaDownloadService,
    ILookupEntryRepository lookupEntryRepository
    ) : IRequestHandler<CreateLookupEntriesCommand, Result<IReadOnlyList<Guid>>>
{
    public async Task<Result<IReadOnlyList<Guid>>> Handle(CreateLookupEntriesCommand request, CancellationToken cancellationToken)
    {
        // Lookup the word using the dictionaries
        var dictionaryLookupResult = await dictionaryLookupService.LookupAsync(request.Word);
        if (dictionaryLookupResult.IsFailure)
            return Result<IReadOnlyList<Guid>>.Failure(dictionaryLookupResult.Errors);

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
                mediaLookup[data.Headword] = [.. mediaFileIds];
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
        var isSaved = await lookupEntryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        if (!isSaved)
            return Result<IReadOnlyList<Guid>>.Failure("Failed to create new lookup entries.");

        return Result<IReadOnlyList<Guid>>.Success([.. lookupEntries.Select(e => e.Id)]);
    }
}