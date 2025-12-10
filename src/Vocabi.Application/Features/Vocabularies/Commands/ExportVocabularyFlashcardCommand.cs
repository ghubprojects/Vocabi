using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Vocabi.Application.Contracts.External.Flashcards;
using Vocabi.Domain.Aggregates.MediaFiles;
using Vocabi.Domain.Aggregates.Vocabularies;

namespace Vocabi.Application.Features.Vocabularies.Commands;

public record ExportVocabularyFlashcardCommand(Guid VocabularyId) : IRequest<Result>;

public class ExportVocabularyFlashcardCommandHandler(
    IVocabularyRepository vocabularyRepository,
    IMediaFileRepository mediaFileRepository,
    IFlashcardService flashcardService,
    IMapper mapper,
    ILogger<ExportVocabularyFlashcardCommandHandler> logger
    ) : IRequestHandler<ExportVocabularyFlashcardCommand, Result>
{
    public async Task<Result> Handle(ExportVocabularyFlashcardCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Load vocabulary
            var vocabulary = await vocabularyRepository.GetByIdAsync(request.VocabularyId);
            
            if (vocabulary is null)
                return Result.Fail($"Vocabulary with Id '{request.VocabularyId}' not found.");

            // Validate vocabulary
            if (vocabulary.HasExportedFlashcard())
                return Result.Fail($"Vocabulary '{vocabulary.Word}' has already been exported.");

            // Prepare flashcard note
            var mediaFiles = await mediaFileRepository.GetByIdsAsync(vocabulary.MediaFiles.Select(x => x.MediaFileId));
            var flashcardNote = mapper.Map<FlashcardNote>((vocabulary, mediaFiles));
            
            vocabulary.EnsureFlashcardCreated();

            // Export flashcard
            var exportResult = await flashcardService.ExportNoteAsync(flashcardNote, mediaFiles.Select(x => x.FilePath));
            if (exportResult.IsFailed)
            {
                vocabulary.MarkFlashcardAsFailed();
                await vocabularyRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                logger.LogWarning("Failed to export vocabulary flashcard. Word={Word}, Errors={Errors}", vocabulary.Word, exportResult.Errors);
                return Result.Fail($"Failed to export vocabulary '{vocabulary.Word}'. Errors: {exportResult.Errors}");
            }

            // Mark success
            vocabulary.MarkFlashcardAsExported(exportResult.Value);
            await vocabularyRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            logger.LogInformation("Exported vocabulary flashcard successfully. Word={Word}, FlashcardId={FlashcardId}",
              vocabulary.Word, exportResult.Value);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while exporting vocabulary flashcard. VocabularyId={VocabularyId}", request.VocabularyId);
            return Result.Fail("Unexpected error while exporting vocabulary.");
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