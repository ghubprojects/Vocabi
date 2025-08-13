using Vocabi.Application.Contracts.External.Audio;
using Vocabi.Application.Contracts.Services;
using Vocabi.Domain.Aggregates.MediaFiles;

namespace Vocabi.Application.Services.Media;

public class AudioService(
    IAudioProvider audioProvider,
    IFileDownloader fileDownloader,
    IMediaFileRepository repository
    ) : MediaServiceBase(fileDownloader, repository)
{
    protected override MediaType GetMediaType() => MediaType.Audio;

    protected override async Task<IEnumerable<string>?> GetFallbackUrlsAsync(string headword)
    {
        var result = await audioProvider.GetAsync(headword);
        return result.IsSuccess ? [result.Data] : null;
    }
}
