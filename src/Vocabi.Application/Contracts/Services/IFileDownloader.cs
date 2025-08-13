using Vocabi.Application.Common.Models;

namespace Vocabi.Application.Contracts.Services;

public interface IFileDownloader
{
    Task<Result<List<string>>> DownloadAsync(IEnumerable<string> urls, CancellationToken cancellationToken = default);
}