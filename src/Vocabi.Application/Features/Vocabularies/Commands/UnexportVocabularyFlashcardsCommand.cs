using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.External.Flashcards;
using Vocabi.Domain.Aggregates.Vocabularies;
using Vocabi.Shared.Extensions;

namespace Vocabi.Application.Features.Vocabularies.Commands;

public record UnexportVocabularyFlashcardsCommand(IEnumerable<Guid> VocabularyIds) : IRequest<Result>;

public class UnexportVocabularyFlashcardsCommandHandler(
    IVocabularyRepository vocabularyRepository,
    IFlashcardService flashcardService,
    ILogger<UnexportVocabularyFlashcardsCommandHandler> logger
    ) : IRequestHandler<UnexportVocabularyFlashcardsCommand, Result>
{
    public async Task<Result> Handle(UnexportVocabularyFlashcardsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Load vocabularies
            var vocabularies = await vocabularyRepository.GetByIdsAsync(request.VocabularyIds);

            if (vocabularies.IsNullOrEmpty())
                return Result.Failure("No vocabularies found to unexport.");

            // Pick only exported ones
            var exportedVocabs = vocabularies
                .Where(x => x.HasExportedFlashcard())
                .ToList();

            if (exportedVocabs.Count == 0)
                return Result.Failure("None of the selected vocabularies have been exported.");

            // Unexport flashcards
            var noteIds = exportedVocabs.Select(v => v.Flashcard.NoteId!.Value).ToList();
            var unexportResult = await flashcardService.UnexportNotesAsync(noteIds);
            if (unexportResult.IsFailure)
            {
                logger.LogWarning("Failed to unexport multiple vocabularies. Errors={Errors}", unexportResult.ErrorMessages);
                return Result.Failure($"Failed to unexport vocabularies. Errors: {unexportResult.ErrorMessages}");
            }

            // Update entities
            exportedVocabs.ForEach(x => x.RemoveFlashcard());
            await vocabularyRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            logger.LogInformation("Successfully unexported {Count} vocabulary flashcards.", exportedVocabs.Count);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while unexporting multiple vocabulary flashcards.");
            return Result.Failure("Unexpected error while unexporting vocabularies.");
        }
    }
}

public class UnexportVocabularyFlashcardsCommandValidator : AbstractValidator<UnexportVocabularyFlashcardsCommand>
{
    public UnexportVocabularyFlashcardsCommandValidator()
    {
        RuleFor(x => x.VocabularyIds)
            .NotNull().WithMessage("Ids must not be null.")
            .Must(ids => ids.Any()).WithMessage("At least one Id is required.")
            .ForEach(idRule => idRule.NotEmpty().WithMessage("Id must not be empty."));
    }
}