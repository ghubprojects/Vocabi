namespace Vocabi.Application.Contracts.Storage;

public interface IFileStorage
{
    Task<string> SaveAsync(Stream fileStream, string fileName, CancellationToken cancellationToken);
    Task DeleteAsync(string filePath, CancellationToken cancellationToken);
}