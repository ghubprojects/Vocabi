using Vocabi.Application.Common.Models;

namespace Vocabi.Application.Contracts.Services;

public interface IFileDownloader
{
    Task<Result<Stream>> DownloadAsStreamAsync(string url);
    Task<Result<List<Stream>>> DownloadMultipleAsStreamAsync(IEnumerable<string> urls);
    Task<Result<byte[]>> DownloadAsBytesAsync(string url);
    Task<Result<List<byte[]>>> DownloadMultipleAsBytesAsync(IEnumerable<string> urls);
}