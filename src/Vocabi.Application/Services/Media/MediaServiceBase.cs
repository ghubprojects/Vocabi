using Vocabi.Application.Contracts.Services;
using Vocabi.Application.Contracts.Storage;
using Vocabi.Domain.Aggregates.MediaFiles;
using Vocabi.Shared.Utils;

namespace Vocabi.Application.Services.Media;

public abstract class MediaServiceBase(IFileDownloader fileDownloader, IFileStorage fileStorage, IMediaFileRepository repository) : IMediaService
{
    public MediaType MediaType => GetMediaType();

    public async Task<List<Guid>> DownloadOrFallbackAsync(string headword, string? url, string providerName, CancellationToken cancellationToken)
    {
        var urls = !string.IsNullOrEmpty(url) ? [url] : await GetFallbackUrlsAsync(headword);
        var sourceName = !string.IsNullOrEmpty(url) ? providerName : GetFallbackProviderName();

        if (urls is null || !urls.Any())
            return [];

        var downloadResult = await fileDownloader.DownloadMultipleAsStreamAsync(urls);
        if (downloadResult.IsFailure)
            return [];

        var mediaFiles = await Task.WhenAll(downloadResult.Data.Select(async stream =>
        {
            await using (stream)
            {
                var saveFileResult = await fileStorage.SaveAsync(stream, Path.GetRandomFileName());
                if (saveFileResult.IsFailure)
                    return null; // or handle failure differently

                var filePath = saveFileResult.Data;

                return MediaFile.CreateNew(
                    Path.GetFileName(filePath),
                    filePath,
                    FileUtils.GetContentType(filePath),
                    stream.Length,
                    MediaSourceCategory.External,
                    sourceName);
            }
        }));


        await repository.AddRangeAsync(mediaFiles);
        return [.. mediaFiles.Select(m => m.Id)];
    }

    protected abstract MediaType GetMediaType();
    protected abstract string GetFallbackProviderName();
    protected abstract Task<IEnumerable<string>?> GetFallbackUrlsAsync(string headword);
}