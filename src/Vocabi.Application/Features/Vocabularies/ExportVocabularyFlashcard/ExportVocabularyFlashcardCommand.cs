using AutoMapper;
using FluentValidation;
using MediatR;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.External.Flashcards;
using Vocabi.Domain.Aggregates.MediaFiles;
using Vocabi.Domain.Aggregates.Vocabularies;
using Vocabi.Shared.Utils;

namespace Vocabi.Application.Features.Vocabularies.ExportVocabularyFlashcard;

public record ExportVocabularyFlashcardCommand(Guid VocabularyId) : IRequest<Result>;

public class ExportVocabularyFlashcardCommandHandler(
    IVocabularyRepository vocabularyRepository,
    IMediaFileRepository mediaFileRepository,
    IFlashcardService flashcardService,
    IMapper mapper
    ) : IRequestHandler<ExportVocabularyFlashcardCommand, Result>
{
    public async Task<Result> Handle(ExportVocabularyFlashcardCommand request, CancellationToken cancellationToken)
    {
        // Get vocabulary 
        var vocabulary = await vocabularyRepository.GetByIdAsync(request.VocabularyId);
        if (vocabulary is null)
            return Result.Failure("No vocabulary found to export.");
        var mediaFiles = await mediaFileRepository.GetByIdsAsync([.. vocabulary.MediaFiles.Select(x => x.MediaFileId)]);

        // Export flashcard
        var flashcardNote = mapper.Map<FlashcardNote>((vocabulary, mediaFiles));
        var options = new ExportOptions("VocabiDeck", "VocabiNote");
        var exportResult = await flashcardService.ExportAsync(flashcardNote, options, cancellationToken);
        if (exportResult.IsFailure || exportResult.Data is null)
            return Result.Failure($"Failed to export vocabulary flashcard: {exportResult.ErrorMessages}");

        // Copy media files
        var getMediaDirPathResult = await flashcardService.GetMediaDirectoryPath(cancellationToken);
        if (getMediaDirPathResult.IsFailure || getMediaDirPathResult.Data is null)
            return Result.Failure($"Failed to get Anki media directory path: {getMediaDirPathResult.ErrorMessages}");
        await FileUtils.CopyFilesAsync(
            mediaFiles.Select(x => FileUtils.GetWwwRootPath(x.FilePath)),
            getMediaDirPathResult.Data);

        // Save changes
        vocabulary.AddFlashcard();
        vocabulary.MarkFlashcardAsExported(exportResult.Data.Value);
        var isSaved = await vocabularyRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        if (!isSaved)
            return Result.Failure("Failed to export vocabulary flashcard.");

        return Result.Success();
    }
}

public class ExportVocabularyFlashcardCommandValidator : AbstractValidator<ExportVocabularyFlashcardCommand>
{
    public ExportVocabularyFlashcardCommandValidator()
    {
        RuleFor(x => x.VocabularyId)
            .NotEmpty().WithMessage("Id must not be empty.");
    }
}