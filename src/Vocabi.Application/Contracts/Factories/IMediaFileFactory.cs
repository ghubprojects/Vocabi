using Vocabi.Domain.Aggregates.MediaFiles;

namespace Vocabi.Application.Contracts.Factories;

public interface IMediaFileFactory
{
    MediaFile FromFilePath(string filePath, MediaSourceCategory sourceCategory, string sourceName);
}
