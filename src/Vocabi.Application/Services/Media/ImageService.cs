using Vocabi.Application.Contracts.External.Image;
using Vocabi.Application.Contracts.Services;
using Vocabi.Domain.Aggregates.MediaFiles;

namespace Vocabi.Application.Services.Media;

public class ImageService(
    IImageProvider imageProvider,
    IFileDownloader fileDownloader,
    IMediaFileRepository repository
    ) : MediaServiceBase(fileDownloader, repository)
{
    protected override MediaType GetMediaType() => MediaType.Image;

    protected override async Task<IEnumerable<string>?> GetFallbackUrlsAsync(string headword)
    {
        var result = await imageProvider.GetAsync(headword);
        return result.IsSuccess ? result.Data : null;
    }
}
