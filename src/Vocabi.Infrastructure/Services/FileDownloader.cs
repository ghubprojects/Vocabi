using Microsoft.Extensions.Logging;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.Services;

namespace Vocabi.Infrastructure.Services;

public class FileDownloader(HttpClient httpClient, ILogger<FileDownloader> logger) : IFileDownloader
{
    public Task<Result<Stream>> DownloadAsStreamAsync(string url) =>
        DownloadAsync(url, async u =>
        {
            var response = await httpClient.GetAsync(u, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        });

    public Task<Result<List<Stream>>> DownloadMultipleAsStreamAsync(IEnumerable<string> urls) =>
        DownloadMultipleAsync(urls, DownloadAsStreamAsync);

    public Task<Result<byte[]>> DownloadAsBytesAsync(string url) =>
        DownloadAsync(url, httpClient.GetByteArrayAsync);

    public Task<Result<List<byte[]>>> DownloadMultipleAsBytesAsync(IEnumerable<string> urls) =>
        DownloadMultipleAsync(urls, DownloadAsBytesAsync);

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
            logger.LogWarning(ex, "Failed to download file from {Url}", url);
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