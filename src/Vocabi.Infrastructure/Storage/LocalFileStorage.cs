using Microsoft.Extensions.Logging;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.Storage;
using Vocabi.Shared.Utils;

namespace Vocabi.Infrastructure.Storage;

public class LocalFileStorage : IFileStorage
{
    private readonly string _rootPath;
    private readonly ILogger<LocalFileStorage> _logger;

    public LocalFileStorage(ILogger<LocalFileStorage> logger)
    {
        _rootPath = FileUtils.GetUploadPath();
        _logger = logger;

        if (!Directory.Exists(_rootPath))
            Directory.CreateDirectory(_rootPath);
    }

    public async Task<Result<string>> SaveAsync(Stream stream, string fileName, string? subFolder = null)
    {
        try
        {
            var folder = EnsureFolder(subFolder);
            var uniqueFileName = FileUtils.GenerateSafeFileName(fileName);
            var filePath = Path.Combine(folder, uniqueFileName);

            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await stream.CopyToAsync(fileStream);

            var relativeFilePath = Path.Combine("uploads", subFolder ?? string.Empty, uniqueFileName);
            return Result<string>.Success(relativeFilePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save file {FileName}", fileName);
            return Result<string>.Failure(ex.Message);
        }
    }

    public async Task<Result> DeleteAsync(string fileName, string? subFolder = null)
    {
        try
        {
            var folder = EnsureFolder(subFolder);
            var filePath = Path.Combine(folder, fileName);

            if (!File.Exists(filePath))
                return await Result.FailureAsync($"File not found: {filePath}");

            File.Delete(filePath);
            return await Result.SuccessAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete file {FileName}", fileName);
            return await Result.FailureAsync(ex.Message);
        }
    }

    private string EnsureFolder(string? subFolder)
    {
        var folder = _rootPath;

        if (!string.IsNullOrWhiteSpace(subFolder))
            folder = Path.Combine(_rootPath, subFolder);

        Directory.CreateDirectory(folder);
        return folder;
    }
}