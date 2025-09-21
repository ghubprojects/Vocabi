using AutoMapper;
using MediatR;
using Vocabi.Application.Common.Models;
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
            if (saveFileResult.IsFailure)
                return Result<MediaFileDto>.Failure(saveFileResult.Errors);

            var filePath = saveFileResult.Data;
            var mediaFile = MediaFile.CreateNew(
                Path.GetFileName(filePath),
                filePath,
                FileUtils.GetContentType(filePath),
                stream.Length,
                "User upload");

            await mediaFileRepository.AddAsync(mediaFile);
            await mediaFileRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return Result<MediaFileDto>.Success(mapper.Map<MediaFileDto>(mediaFile));
        }
        catch (Exception)
        {
            return Result<MediaFileDto>.Failure("Failed to upload media file.");
        }
    }
}