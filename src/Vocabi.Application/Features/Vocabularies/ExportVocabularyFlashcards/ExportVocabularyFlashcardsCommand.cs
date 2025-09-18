using AutoMapper;
using FluentValidation;
using MediatR;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.External.Flashcards;
using Vocabi.Domain.Aggregates.MediaFiles;
using Vocabi.Domain.Aggregates.Vocabularies;
using Vocabi.Shared.Extensions;

namespace Vocabi.Application.Features.Vocabularies.ExportVocabularyFlashcards;

public record ExportVocabularyFlashcardsCommand(IEnumerable<Guid> VocabularyIds) : IRequest<Result>;

public class ExportVocabularyFlashcardsCommandHandler(
    IVocabularyRepository vocabularyRepository,
    IMediaFileRepository mediaFileRepository,
    IFlashcardService flashcardService,
    IMapper mapper
    ) : IRequestHandler<ExportVocabularyFlashcardsCommand, Result>
{
    public async Task<Result> Handle(ExportVocabularyFlashcardsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var vocabularies = await vocabularyRepository.GetByIdsAsync(request.VocabularyIds);

            if (vocabularies.IsNullOrEmpty())
                return Result.Failure("No vocabularies found to export.");

            var errors = new List<string>();
            foreach (var vocabulary in vocabularies)
            {
                if (vocabulary.Flashcard is not null && vocabulary.Flashcard.Status == FlashcardStatus.Exported)
                {
                    errors.Add($"Vocabulary '{vocabulary.Word}' has already been exported.");
                    continue;
                }

                var mediaFileIds = vocabulary.MediaFiles.Select(x => x.MediaFileId);
                var mediaFiles = await mediaFileRepository.GetByIdsAsync(mediaFileIds);
                var flashcardNote = mapper.Map<FlashcardNote>((vocabulary, mediaFiles));

                if (vocabulary.Flashcard is null)
                    vocabulary.AddFlashcard();

                var exportResult = await flashcardService.ExportNoteAsync(flashcardNote, mediaFiles.Select(x => x.FilePath));
                if (exportResult.IsFailure)
                {
                    vocabulary.MarkFlashcardAsFailed();
                    errors.Add($"Failed to export '{vocabulary.Word}': {exportResult.ErrorMessages}");
                    continue;
                }

                vocabulary.MarkFlashcardAsExported(exportResult.Data);
            }

            await vocabularyRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            if (errors.Count > 0)
                return Result.Failure(errors);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure("Failed to export vocabulary flashcards.");
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