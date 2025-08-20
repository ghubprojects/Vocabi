using static Vocabi.Shared.Common.Enums;

namespace Vocabi.Application.Services.Media;

public interface IMediaService
{
    MediaType MediaType { get; }
    Task<List<Guid>> DownloadAsyncWithFallback(string headword, string? url, string providerName, CancellationToken cancellationToken);
}