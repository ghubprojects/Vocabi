using Vocabi.Application.Contracts.Services.DownloadFile;
using Vocabi.Application.Contracts.Storage;
using Vocabi.Domain.Aggregates.MediaFiles;
using Vocabi.Shared.Extensions;
using Vocabi.Shared.Utils;
using static Vocabi.Shared.Common.Enums;

namespace Vocabi.Application.Services.Media;

public abstract class MediaServiceBase(IFileDownloader fileDownloader, IFileStorage fileStorage, IMediaFileRepository repository) : IMediaService
{
    public MediaType MediaType => GetMediaType();

    public async Task<List<Guid>> DownloadAsyncWithFallback(string headword, string? initialUrl, string initialProvider, CancellationToken cancellationToken)
    {
        var urls = !string.IsNullOrWhiteSpace(initialUrl) ? [initialUrl] : await GetFallbackUrlsAsync(headword);
        if (urls.IsNullOrEmpty())
            return [];

        var providerName = !string.IsNullOrWhiteSpace(initialUrl) ? initialProvider : GetFallbackProviderName();

        var downloadResult = await fileDownloader.DownloadMultipleAsStreamAsync(urls, cancellationToken);
        if (downloadResult.IsFailure)
            return [];

        var mediaFiles = new List<MediaFile>();
        var downloadedStreamFiles = downloadResult.Data;
        foreach (var file in downloadedStreamFiles)
        {
            var stream = file.Content;
            await using (stream)
            {
                var saveResult = await fileStorage.SaveAsync(stream, file.FileName);
                if (saveResult.IsFailure)
                    continue;

                var savedFilePath = saveResult.Data;
                var mediaFile = MediaFile.CreateNew(
                    Path.GetFileName(savedFilePath),
                    savedFilePath,
                    FileUtils.GetContentType(savedFilePath),
                    stream.Length,
                    providerName);

                mediaFiles.Add(mediaFile);
            }
        }

        if (mediaFiles.Count == 0)
            return [];

        await repository.AddRangeAsync(mediaFiles);
        return [.. mediaFiles.Select(m => m.Id)];
    }

    protected abstract MediaType GetMediaType();
    protected abstract string GetFallbackProviderName();
    protected abstract Task<IEnumerable<string>?> GetFallbackUrlsAsync(string headword);
}