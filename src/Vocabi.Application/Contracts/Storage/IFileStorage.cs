using Vocabi.Application.Common.Models;

namespace Vocabi.Application.Contracts.Storage;

public interface IFileStorage
{
    Task<Result<string>> SaveAsync(Stream stream, string fileName, string? subFolder = null);
    Task<Result> DeleteAsync(string fileName, string? subFolder = null);
}