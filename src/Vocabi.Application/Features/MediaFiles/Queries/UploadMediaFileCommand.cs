using AutoMapper;
using FluentResults;
using MediatR;
using Vocabi.Application.Contracts.Storage;
using Vocabi.Application.Features.MediaFiles.DTOs;
using Vocabi.Domain.Aggregates.MediaFiles;
using Vocabi.Shared.Utils;

namespace Vocabi.Application.Features.MediaFiles.Queries;

public class UploadMediaFileCommand : IRequest<Result<MediaFileDto>>
{
    public Stream Stream { get; set; } = default!;
    public string Filename { get; set; } = string.Empty;
}

public class UploadMediaFileCommandHandler(
    IMediaFileRepository mediaFileRepository,
    IFileStorage fileStorage,
    IMapper mapper
    ) : IRequestHandler<UploadMediaFileCommand, Result<MediaFileDto>>
{
    public async Task<Result<MediaFileDto>> Handle(UploadMediaFileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await using var stream = request.Stream;

            var saveFileResult = await fileStorage.SaveAsync(stream, request.Filename);
            if (saveFileResult.IsFailed)
                return Result.Fail(saveFileResult.Errors);

            var filePath = saveFileResult.Value;
            var mediaFile = MediaFile.CreateNew(
                Path.GetFileName(filePath),
                filePath,
                FileUtils.GetContentType(filePath),
                stream.Length,
                "User upload");

            await mediaFileRepository.AddAsync(mediaFile);
            await mediaFileRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return Result.Ok(mapper.Map<MediaFileDto>(mediaFile));
        }
        catch (Exception)
        {
            return Result.Fail("Failed to upload media file.");
        }
    }
}