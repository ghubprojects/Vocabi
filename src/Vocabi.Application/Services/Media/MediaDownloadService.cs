using static Vocabi.Shared.Common.Enums;

namespace Vocabi.Application.Services.Media;

public class MediaDownloadService(IEnumerable<IMediaService> mediaServices)
{
    public async Task<IReadOnlyList<Guid>> DownloadAllMediaAsync(string headword, IReadOnlyDictionary<MediaType, string?> urls, string providerName, CancellationToken cancellationToken)
    {
        var tasks = mediaServices.Select(service => service.DownloadAsyncWithFallback(
            headword,
            urls.GetValueOrDefault(service.MediaType),
            providerName,
            cancellationToken));

        var results = await Task.WhenAll(tasks);
        return [.. results.SelectMany(r => r)];
    }
}