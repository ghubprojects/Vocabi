using Vocabi.Application.Contracts.External.Image;
using Vocabi.Application.Contracts.Storage;
using Vocabi.Application.Services.Interfaces.DownloadFile;
using Vocabi.Domain.Aggregates.MediaFiles;
using static Vocabi.Shared.Common.Enums;

namespace Vocabi.Application.Services.Media;

public class ImageService(
    IImageProvider imageProvider,
    IFileDownloader fileDownloader,
    IFileStorage fileStorage,
    IMediaFileRepository repository
    ) : MediaServiceBase(fileDownloader, fileStorage, repository)
{
    protected override MediaType GetMediaType() => MediaType.Image;

    protected override string GetFallbackProviderName() => imageProvider.ProviderName;

    protected override async Task<IEnumerable<string>?> GetFallbackUrlsAsync(string headword)
    {
        var result = await imageProvider.GetAsync(headword);
        return result.IsSuccess ? result.Value : null;
    }
}
