using Vocabi.Application.Common.Models;

namespace Vocabi.Application.Contracts.External.Image;

public interface IImageProvider : IExternalProvider
{
    Task<Result<IReadOnlyList<string>>> GetAsync(string keyword, int limit = 5, CancellationToken cancellationToken = default);
}