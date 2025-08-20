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
    IFlashcardExporter flashcardExporter,
    IMapper mapper
    ) : IRequestHandler<ExportVocabularyFlashcardCommand, Result>
{
    public async Task<Result> Handle(ExportVocabularyFlashcardCommand request, CancellationToken cancellationToken)
    {
        var vocabulary = await vocabularyRepository.GetByIdAsync(request.VocabularyId);
        if (vocabulary is null)
            return Result.Failure("No vocabulary found to export.");

        vocabulary.AddFlashcard();
        var mediaFiles = await mediaFileRepository.GetByIdsAsync([.. vocabulary.MediaFiles.Select(x => x.Id)]);

        var flashcardNote = mapper.Map<FlashcardNote>((vocabulary, mediaFiles));
        var options = new ExportOptions("VocabiDeck", "VocabiNote");

        var exportResult = await flashcardExporter.ExportAsync(flashcardNote, options, cancellationToken);
        if (exportResult.IsFailure || exportResult.Data is null)
        {
            vocabulary.MarkFlashcardAsFailed();
            await vocabularyRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Failure($"Failed to export vocabulary flashcard: {exportResult.ErrorMessages}");
        }

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