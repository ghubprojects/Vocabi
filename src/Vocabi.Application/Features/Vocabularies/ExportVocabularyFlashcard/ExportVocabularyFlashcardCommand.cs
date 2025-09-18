using AutoMapper;
using FluentValidation;
using MediatR;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.External.Flashcards;
using Vocabi.Domain.Aggregates.MediaFiles;
using Vocabi.Domain.Aggregates.Vocabularies;

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
        try
        {
            // Get vocabulary
            var vocabulary = await vocabularyRepository.GetByIdAsync(request.VocabularyId);

            if (vocabulary is null)
                return Result.Failure("No vocabulary found to export.");

            if (vocabulary.Flashcard is not null && vocabulary.Flashcard.Status == FlashcardStatus.Exported)
                return Result.Failure($"Vocabulary '{vocabulary.Word}' has already been exported.");

            // Export
            var mediaFileIds = vocabulary.MediaFiles.Select(x => x.MediaFileId);
            var mediaFiles = await mediaFileRepository.GetByIdsAsync(mediaFileIds);
            var flashcardNote = mapper.Map<FlashcardNote>((vocabulary, mediaFiles));

            if (vocabulary.Flashcard is null)
                vocabulary.AddFlashcard();

            var exportResult = await flashcardService.ExportNoteAsync(flashcardNote, mediaFiles.Select(x => x.FilePath));
            if (exportResult.IsFailure)
            {
                vocabulary.MarkFlashcardAsFailed();
                await vocabularyRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                return Result.Failure($"Failed to export '{vocabulary.Word}': {exportResult.ErrorMessages}");
            }

            vocabulary.MarkFlashcardAsExported(exportResult.Data);
            await vocabularyRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure("Failed to export vocabulary flashcard.");
        }
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