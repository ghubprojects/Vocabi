using Vocabi.Domain.Aggregates.MediaFiles;

namespace Vocabi.Application.Services.Media;

public interface IMediaService
{
    MediaType MediaType { get; }
    Task<List<Guid>> DownloadOrFallbackAsync(string headword, string? url, string providerName, CancellationToken cancellationToken);
}
