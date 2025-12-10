using FluentResults;

namespace Vocabi.Application.Services.Interfaces.DownloadFile;

public interface IFileDownloader
{
    Task<Result<DownloadedStreamFile>> DownloadAsStreamAsync(string url, CancellationToken cancellationToken);
    Task<Result<List<DownloadedStreamFile>>> DownloadMultipleAsStreamAsync(IEnumerable<string> urls, CancellationToken cancellationToken);
    Task<Result<DownloadedBinaryFile>> DownloadAsBytesAsync(string url, CancellationToken cancellationToken);
    Task<Result<List<DownloadedBinaryFile>>> DownloadMultipleAsBytesAsync(IEnumerable<string> urls, CancellationToken cancellationToken);
}