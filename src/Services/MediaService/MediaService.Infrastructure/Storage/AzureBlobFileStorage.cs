using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MediaService.Application.Abstractions;
using MediaService.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace MediaService.Infrastructure.Storage;

public sealed class AzureBlobFileStorage : IFileStorage
{
    private readonly BlobContainerClient _container;

    public AzureBlobFileStorage(IOptions<StorageOptions> options)
    {
        var client = new BlobServiceClient(options.Value.ConnectionString);
        _container = client.GetBlobContainerClient(options.Value.Container);
        _container.CreateIfNotExists();
    }

    public async Task SaveAsync(
        string path,
        Stream content,
        string contentType,
        CancellationToken ct = default)
    {
        var blob = _container.GetBlobClient(path);

        await blob.UploadAsync(
            content,
            new BlobHttpHeaders { ContentType = contentType },
            cancellationToken: ct);
    }

    public async Task<Stream> GetAsync(string path, CancellationToken ct = default)
    {
        var blob = _container.GetBlobClient(path);
        var result = await blob.DownloadStreamingAsync(ct);
        return result.Value.Content;
    }

    public async Task DeleteAsync(string path, CancellationToken ct = default)
    {
        var blob = _container.GetBlobClient(path);
        await blob.DeleteIfExistsAsync(cancellationToken: ct);
    }
}
