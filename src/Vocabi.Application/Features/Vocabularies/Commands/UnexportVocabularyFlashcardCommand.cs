using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.External.Flashcards;
using Vocabi.Domain.Aggregates.Vocabularies;

namespace Vocabi.Application.Features.Vocabularies.Commands;

public record UnexportVocabularyFlashcardCommand(Guid VocabularyId) : IRequest<Result>;

public class UnexportVocabularyFlashcardCommandHandler(
    IVocabularyRepository vocabularyRepository,
    IFlashcardService flashcardService,
    ILogger<UnexportVocabularyFlashcardCommandHandler> logger
    ) : IRequestHandler<UnexportVocabularyFlashcardCommand, Result>
{
    public async Task<Result> Handle(UnexportVocabularyFlashcardCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Load vocabulary
            var vocabulary = await vocabularyRepository.GetByIdAsync(request.VocabularyId);

            if (vocabulary is null)
                return Result.Failure($"Vocabulary with Id '{request.VocabularyId}' not found.");

            // Validate vocabulary
            if (!vocabulary.HasExportedFlashcard())
                return Result.Failure($"Vocabulary '{vocabulary.Word}' has not been exported yet.");

            // Unexport flashcard
            var unexportResult = await flashcardService.UnexportNoteAsync(vocabulary.Flashcard.NoteId!.Value);
            if (unexportResult.IsFailure)
            {
                logger.LogWarning("Failed to unexport vocabulary flashcard. Word={Word}, Errors={Errors}", vocabulary.Word, unexportResult.ErrorMessages);
                return Result.Failure($"Failed to unexport vocabulary '{vocabulary.Word}'. Errors: {unexportResult.ErrorMessages}");
            }

            // Update entity
            vocabulary.RemoveFlashcard();
            await vocabularyRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            logger.LogInformation("Successfully unexported flashcard. Word={Word}", vocabulary.Word);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while unexporting vocabulary flashcard. VocabularyId={VocabularyId}", request.VocabularyId);
            return Result.Failure("Unexpected error while unexporting vocabulary.");
        }
    }
}

public class UnexportVocabularyFlashcardCommandValidator : AbstractValidator<UnexportVocabularyFlashcardCommand>
{
    public UnexportVocabularyFlashcardCommandValidator()
    {
        RuleFor(x => x.VocabularyId)
            .NotEmpty().WithMessage("Id must not be empty.");
    }
}
