using AutoMapper;
using MediatR;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.External.Audio;
using Vocabi.Application.Contracts.External.Dictionary;
using Vocabi.Application.Contracts.External.Image;
using Vocabi.Application.Features.Vocabularies.DTOs;
using Vocabi.Application.Services;
using Vocabi.Domain.Aggregates.LookupEntries;

namespace Vocabi.Application.Features.Vocabularies.Queries;

public class LookupWordQuery : IRequest<Result<VocabularyUpsertDto>>
{
    public string Word { get; set; } = string.Empty;
}

public class LookupWordQueryHandler(
    IMapper mapper,
    ILookupEntryRepository lookupEntryRepository,
    IDictionaryProvider dictionaryProvider,
    IImageProvider imageProvider,
    IAudioProvider audioProvider,
    MediaDownloadService imageDownloadService)
    : IRequestHandler<LookupWordQuery, Result<VocabularyUpsertDto>>
{
    public async Task<Result<VocabularyUpsertDto>> Handle(LookupWordQuery request, CancellationToken cancellationToken)
    {
        var dictionaryLookupResult = await dictionaryProvider.LookupAsync(request.Word);
        if (dictionaryLookupResult.IsFailure)
            return Result<VocabularyUpsertDto>.Failure([.. dictionaryLookupResult.Errors]);

        var dictionaryLookupResultData = dictionaryLookupResult.Data;
        var lookupEntry = LookupEntry.CreateNew(
            dictionaryLookupResultData.Headword,
            dictionaryLookupResultData.PartOfSpeech,
            dictionaryLookupResultData.Pronunciation
        );

        // Nếu thiếu audio → gọi GoogleTTS
        if (!string.IsNullOrEmpty(dictionaryLookupResultData.AudioUrl))
        {
            await imageDownloadService.DownloadAndSaveAsync([dictionaryLookupResultData.AudioUrl], dictionaryProvider.ProviderName, cancellationToken);
        }
        else
        {
            var audioUrl = await audioProvider.GetAsync(dictionaryLookupResultData.Headword);
            dictionaryLookupResult.MediaFiles.Add(new DictionaryLookupResult.MediaFileModel
            {
                Url = audioUrl,
                ContentType = "audio/mpeg",
                SourceName = "GoogleTTS"
            });
        }

        // Nếu thiếu image → gọi Pixabay
        if (!string.IsNullOrEmpty(dictionaryLookupResultData.ImageUrl))
        {
            var imageUrl = await imageProvider.GetImageAsync(dictionaryLookupResult.Headword);
            dictionaryLookupResult.MediaFiles.Add(new DictionaryLookupResult.MediaFileModel
            {
                Url = imageUrl,
                ContentType = "image/jpeg",
                SourceName = "Pixabay"
            });
        }

    }
}