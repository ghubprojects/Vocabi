using Vocabi.Domain.Aggregates.MediaFiles;

namespace Vocabi.Application.Contracts.Services.Media;

public interface IAudioService
{
    Task<List<Guid>> DownloadOrFallbackAsync(string headword, string url, string providerName, CancellationToken cancellationToken);
}