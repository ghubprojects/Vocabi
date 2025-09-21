using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.External.Flashcards;
using Vocabi.Domain.Aggregates.MediaFiles;
using Vocabi.Domain.Aggregates.Vocabularies;
using Vocabi.Shared.Extensions;

namespace Vocabi.Application.Features.Vocabularies.Commands;

public record ExportVocabularyFlashcardsCommand(IEnumerable<Guid> VocabularyIds) : IRequest<Result>;

public class ExportVocabularyFlashcardsCommandHandler(
    IVocabularyRepository vocabularyRepository,
    IMediaFileRepository mediaFileRepository,
    IFlashcardService flashcardService,
    IMapper mapper,
    ILogger<ExportVocabularyFlashcardsCommandHandler> logger
    ) : IRequestHandler<ExportVocabularyFlashcardsCommand, Result>
{
    public async Task<Result> Handle(ExportVocabularyFlashcardsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Load vocabularies
            var vocabularies = await vocabularyRepository.GetByIdsAsync(request.VocabularyIds);
            
            if (vocabularies.IsNullOrEmpty())
                return Result.Failure("No vocabularies found to export.");

            // Validate vocabularies
            var validVocabs = vocabularies
               .Where(x => !x.HasExportedFlashcard())
               .ToList();

            if (validVocabs.Count == 0)
                return Result.Failure("All selected vocabularies have already been exported.");

            // Prepare flashcard note
            var allMediaFileIds = validVocabs.SelectMany(v => v.MediaFiles.Select(m => m.MediaFileId)).Distinct();
            var allMediaFiles = await mediaFileRepository.GetByIdsAsync(allMediaFileIds);

            var flashcardNotes = new List<FlashcardNote>();
            var mediaPaths = new List<string>();

            foreach (var vocab in validVocabs)
            {
                var vocabMediaFiles = allMediaFiles.Where(m => vocab.MediaFiles.Any(vm => vm.MediaFileId == m.Id)).ToList();
                var note = mapper.Map<FlashcardNote>((vocab, vocabMediaFiles));
                flashcardNotes.Add(note);
                mediaPaths.AddRange(vocabMediaFiles.Select(m => m.FilePath));

                vocab.EnsureFlashcardCreated();
            }

            // Export flashcards
            var exportResult = await flashcardService.ExportNotesAsync(flashcardNotes, mediaPaths.Distinct());
            if (exportResult.IsFailure)
            {
                validVocabs.ForEach(x => x.MarkFlashcardAsFailed());
                await vocabularyRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                logger.LogWarning("Failed to export multiple vocabulary flashcards. Errors={Errors}", exportResult.ErrorMessages);
                return Result.Failure($"Failed to export vocabularies. Errors: {exportResult.ErrorMessages}");
            }

            // Mark success
            var noteIds = exportResult.Data;
            for (int i = 0; i < validVocabs.Count; i++)
            {
                if (i < noteIds.Length)
                    validVocabs[i].MarkFlashcardAsExported(noteIds[i]);
                else
                    validVocabs[i].MarkFlashcardAsFailed();
            }
            await vocabularyRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            logger.LogInformation("Successfully exported {Count} vocabulary flashcards.", noteIds.Length);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while exporting multiple vocabulary flashcards.");
            return Result.Failure("Unexpected error while exporting vocabularies.");
        }
    }
}

public class ExportVocabularyFlashcardsCommandValidator : AbstractValidator<ExportVocabularyFlashcardsCommand>
{
    public ExportVocabularyFlashcardsCommandValidator()
    {
        RuleFor(x => x.VocabularyIds)
            .NotNull().WithMessage("Ids must not be null.")
            .Must(ids => ids.Any()).WithMessage("At least one Id is required.")
            .ForEach(idRule => idRule.NotEmpty().WithMessage("Id must not be empty."));
    }
}