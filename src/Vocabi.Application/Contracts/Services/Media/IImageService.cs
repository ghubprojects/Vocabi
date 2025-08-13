using Vocabi.Domain.Aggregates.MediaFiles;

namespace Vocabi.Application.Contracts.Services.Media;

public interface IImageService
{
    Task<List<Guid>> DownloadOrFallbackAsync(string headword, string url, string providerName, CancellationToken cancellationToken);
}