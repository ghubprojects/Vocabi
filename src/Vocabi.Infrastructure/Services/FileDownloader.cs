using Microsoft.Extensions.Logging;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.Services.DownloadFile;
using Vocabi.Shared.Utils;

namespace Vocabi.Infrastructure.Services;

public class FileDownloader(HttpClient httpClient, ILogger<FileDownloader> logger) : IFileDownloader
{
    public Task<Result<DownloadedStreamFile>> DownloadAsStreamAsync(string url, CancellationToken cancellationToken) =>
        DownloadAsync(url, async u =>
        {
            var response = await httpClient.GetAsync(u, cancellationToken);
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var fileName = FileUtils.GetFileNameFromUrl(url);
            var contentType = response.Content.Headers.ContentType?.ToString();

            return new DownloadedStreamFile(stream, fileName, contentType);
        });

    public Task<Result<DownloadedBinaryFile>> DownloadAsBytesAsync(string url, CancellationToken cancellationToken) =>
        DownloadAsync(url, async u =>
        {
            var response = await httpClient.GetAsync(u, cancellationToken);
            response.EnsureSuccessStatusCode();

            var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
            var fileName = FileUtils.GetFileNameFromUrl(url);
            var contentType = response.Content.Headers.ContentType?.ToString();

            return new DownloadedBinaryFile(bytes, fileName, contentType);
        });

    public Task<Result<List<DownloadedStreamFile>>> DownloadMultipleAsStreamAsync(IEnumerable<string> urls, CancellationToken cancellationToken) =>
       DownloadMultipleAsync(urls, u => DownloadAsStreamAsync(u, cancellationToken));

    public Task<Result<List<DownloadedBinaryFile>>> DownloadMultipleAsBytesAsync(IEnumerable<string> urls, CancellationToken cancellationToken) =>
        DownloadMultipleAsync(urls, u => DownloadAsBytesAsync(u, cancellationToken));

    private async Task<Result<T>> DownloadAsync<T>(string url, Func<string, Task<T>> downloader)
    {
        if (string.IsNullOrWhiteSpace(url))
            return Result<T>.Failure("URL is null or empty.");

        try
        {
            var result = await downloader(url);
            return Result<T>.Success(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to download file from {Url}", url);
            return Result<T>.Failure($"Failed to download from {url}: {ex.Message}");
        }
    }

    private async Task<Result<List<T>>> DownloadMultipleAsync<T>(
        IEnumerable<string> urls,
        Func<string, Task<Result<T>>> downloader)
    {
        if (urls is null || !urls.Any())
            return Result<List<T>>.Failure("No URLs provided.");

        try
        {
            var tasks = urls.Select(downloader);
            var results = await Task.WhenAll(tasks);

            var successful = results
                .Where(x => x.IsSuccess && x.Data is not null)
                .Select(x => x.Data!)
                .ToList();

            if (successful.Count == 0)
                return Result<List<T>>.Failure("No files could be downloaded.");

            return Result<List<T>>.Success(successful);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while downloading multiple files.");
            return Result<List<T>>.Failure(ex.Message);
        }
    }
}