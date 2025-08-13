using Microsoft.Extensions.Configuration;
using Vocabi.Application.Contracts.Storage;

namespace Vocabi.Infrastructure.Storage;

public class LocalFileStorage : IFileStorage
{
    private readonly string _basePath;

    public LocalFileStorage()
    {
        _basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        if (!Directory.Exists(_basePath))
            Directory.CreateDirectory(_basePath);
    }

    public async Task<string> SaveAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        var safeFileName = Path.GetFileName(fileName); // tránh path injection
        var uniqueName = $"{Guid.NewGuid()}_{safeFileName}";
        var fullPath = Path.Combine(_basePath, uniqueName);

        await using var output = File.Create(fullPath);
        await fileStream.CopyToAsync(output, cancellationToken);

        return fullPath; // hoặc return relative path nếu muốn
    }

    public Task DeleteAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (File.Exists(filePath))
            File.Delete(filePath);

        return Task.CompletedTask;
    }
}