using FluentResults;

namespace Vocabi.Application.Contracts.External.Image;

public interface IImageProvider : IExternalProvider
{
    Task<Result<IReadOnlyList<string>>> GetAsync(string keyword, CancellationToken cancellationToken = default);
}