using Vocabi.Domain.Aggregates.MediaFiles;

namespace Vocabi.Application.Services.Media;

public class MediaDownloadService(IEnumerable<IMediaService> mediaServices)
{
    public async Task<List<Guid>> DownloadAllMediaAsync(string headword, Dictionary<MediaType, string?> urls, string providerName, CancellationToken cancellationToken)
    {
        var tasks = mediaServices.Select(service => service.DownloadOrFallbackAsync(
            headword, 
            urls.GetValueOrDefault(service.MediaType), 
            providerName,
            cancellationToken));

        var results = await Task.WhenAll(tasks);
        return [.. results.SelectMany(r => r)];
    }
}