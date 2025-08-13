using Vocabi.Application.Contracts.Storage;
using Vocabi.Domain.Aggregates.MediaFiles;

namespace Vocabi.Application.Services;

public class MediaDownloadService(
    HttpClient httpClient,
    IFileStorage fileStorage,
    IMediaFileRepository mediaRepo)
{

    /// <summary>
    /// Tải và lưu nhiều ảnh song song để tăng tốc.
    /// </summary>
    public async Task<List<Guid>> DownloadAndSaveAsync(
        IEnumerable<string> urls,
        string sourceName,
        CancellationToken cancellationToken = default)
    {
        var tasks = urls.Select(async url =>
        {
            try
            {
                using var stream = await httpClient.GetStreamAsync(url, cancellationToken);

                var fileName = Path.GetFileName(new Uri(url).AbsolutePath);
                var contentType = GetContentType(fileName);

                var path = await fileStorage.SaveAsync(stream, fileName, contentType, cancellationToken);

                var media = MediaFile.CreateNew(
                    Path.GetFileName(path),
                    path,
                    contentType,
                    MediaType.Image,
                    new FileInfo(path).Length,
                    MediaSourceCategory.External,
                    sourceName);

                await mediaRepo.AddAsync(media, cancellationToken);
                return media.Id;
            }
            catch
            {
                // Có thể log lỗi từng ảnh để không dừng toàn bộ
                return Guid.Empty;
            }
        });

        var results = await Task.WhenAll(tasks);
        return results.Where(id => id != Guid.Empty).ToList();
    }

    private static string GetContentType(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        return ext switch
        {
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            _ => "image/jpeg"
        };
    }
}