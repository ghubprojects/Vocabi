using Vocabi.Application.Contracts.Services;
using Vocabi.Domain.Aggregates.MediaFiles;
using Vocabi.Shared.Utils;

namespace Vocabi.Application.Services.Media;

public abstract class MediaServiceBase(IFileDownloader fileDownloader, IMediaFileRepository repository) : IMediaService
{
    public MediaType MediaType => GetMediaType();

    public async Task<List<Guid>> DownloadOrFallbackAsync(string headword, string? url, string providerName, CancellationToken cancellationToken)
    {
        var urls = !string.IsNullOrEmpty(url) ? [url] : await GetFallbackUrlsAsync(headword);

        if (urls is null || !urls.Any())
            return [];

        var files = await fileDownloader.DownloadAsync(urls, cancellationToken);
        if (files.IsFailure)
            return [];

        var mediaFiles = files.Data.Select(path => MediaFile.CreateNew(
            Path.GetFileName(path),
            path,
            FileUtils.GetContentType(path),
            MediaType,
            new FileInfo(path).Length,
            MediaSourceCategory.External,
            providerName)).ToList();

        await repository.AddRangeAsync(mediaFiles);
        return [.. mediaFiles.Select(m => m.Id)];
    }

    protected abstract MediaType GetMediaType();
    protected abstract Task<IEnumerable<string>?> GetFallbackUrlsAsync(string headword);
}