using MediaService.Application.Abstractions;

namespace MediaService.Application.Services;

public sealed class MediaAppService(IFileStorage storage)
{
    public Task UploadAsync(
        string path,
        Stream stream,
        string contentType,
        CancellationToken ct)
        => storage.SaveAsync(path, stream, contentType, ct);

    public Task<Stream> GetAsync(string path, CancellationToken ct)
        => storage.GetAsync(path, ct);

    public Task DeleteAsync(string path, CancellationToken ct)
        => storage.DeleteAsync(path, ct);
}

// TODO: Use Command / Query

//[HttpPost]
//    public async Task<IActionResult> Upload(
//        [FromQuery] string path,
//        IFormFile file,
//        CancellationToken ct)
//    {
//        await using var stream = file.OpenReadStream();

//        await _service.UploadAsync(
//            path,
//            stream,
//            file.ContentType,
//            ct);

//        return Ok();
//    }

//    [HttpGet]
//    public async Task<IActionResult> Get(
//        [FromQuery] string path,
//        CancellationToken ct)
//    {
//        var stream = await _service.GetAsync(path, ct);
//        return File(stream, "application/octet-stream");
//    }

//    [HttpDelete]
//    public async Task<IActionResult> Delete(
//        [FromQuery] string path,
//        CancellationToken ct)
//    {
//        await _service.DeleteAsync(path, ct);
//        return NoContent();
//    }