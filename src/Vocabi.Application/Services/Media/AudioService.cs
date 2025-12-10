using Vocabi.Application.Contracts.External.Audio;
using Vocabi.Application.Contracts.Storage;
using Vocabi.Application.Services.Interfaces.DownloadFile;
using Vocabi.Domain.Aggregates.MediaFiles;
using static Vocabi.Shared.Common.Enums;

namespace Vocabi.Application.Services.Media;

public class AudioService(
    IAudioProvider audioProvider,
    IFileDownloader fileDownloader,
    IFileStorage fileStorage,
    IMediaFileRepository repository
    ) : MediaServiceBase(fileDownloader, fileStorage, repository)
{
    protected override MediaType GetMediaType() => MediaType.Audio;

    protected override string GetFallbackProviderName() => audioProvider.ProviderName;

    protected override async Task<IEnumerable<string>?> GetFallbackUrlsAsync(string headword)
    {
        var result = audioProvider.Get(headword);
        return result.IsSuccess ? [result.Value] : null;
    }
}
