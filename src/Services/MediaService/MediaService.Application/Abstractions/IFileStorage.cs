namespace MediaService.Application.Abstractions;

public interface IFileStorage
{
    Task SaveAsync(
        string path,
        Stream content,
        string contentType,
        CancellationToken ct = default);

    Task<Stream> GetAsync(
        string path,
        CancellationToken ct = default);

    Task DeleteAsync(
        string path,
        CancellationToken ct = default);
}
