using Microsoft.Extensions.Logging;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.Services;
using Vocabi.Application.Contracts.Storage;
using Vocabi.Shared.Utils;

namespace Vocabi.Infrastructure.Services;

public class FileDownloader(HttpClient httpClient, IFileStorage fileStorage, ILogger<FileDownloader> logger) : IFileDownloader
{
    public async Task<Result<List<string>>> DownloadAsync(IEnumerable<string> urls, CancellationToken cancellationToken = default)
    {
        try
        {
            var tasks = urls.Select(url => DownloadSingleAsync(url, cancellationToken));
            var results = await Task.WhenAll(tasks);

            var successful = results
                .Where(path => !string.IsNullOrWhiteSpace(path))
                .ToList();

            if (successful.Count == 0)
                return Result<List<string>>.Failure($"No files could be downloaded.");

            return Result<List<string>>.Success(successful);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while downloading files");
            return Result<List<string>>.Failure(ex.Message);
        }
    }

    private async Task<string> DownloadSingleAsync(string url, CancellationToken cancellationToken)
    {
        try
        {
            await using var stream = await httpClient.GetStreamAsync(url, cancellationToken);
            var fileName = Path.GetFileName(new Uri(url).AbsolutePath);
            var contentType = FileUtils.GetContentType(fileName);

            return await fileStorage.SaveAsync(stream, fileName, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to download file from {Url}", url);
            return string.Empty;
        }
    }
}