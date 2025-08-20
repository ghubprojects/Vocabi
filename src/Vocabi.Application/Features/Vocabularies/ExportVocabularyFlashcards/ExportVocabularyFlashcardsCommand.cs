using FluentValidation;
using MediatR;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.External.Flashcards;
using Vocabi.Domain.Aggregates.Vocabularies;
using Vocabi.Shared.Extensions;

namespace Vocabi.Application.Features.Vocabularies.ExportVocabularyFlashcards;

public record ExportVocabularyFlashcardsCommand(IReadOnlyCollection<Guid> VocabularyIds) : IRequest<Result>;

public class ExportVocabularyFlashcardsCommandHandler(
    IVocabularyRepository vocabularyRepository,
    IFlashcardExporter flashcardPublisher
    ) : IRequestHandler<ExportVocabularyFlashcardsCommand, Result>
{
    public async Task<Result> Handle(ExportVocabularyFlashcardsCommand request, CancellationToken cancellationToken)
    {
        var vocabularies = await vocabularyRepository.GetByIdsAsync(request.VocabularyIds);
        if (vocabularies.IsNullOrEmpty())
            return Result.Failure("No vocabularies found to export.");

        return Result.Success();
    }
}

public class ExportVocabularyFlashcardsCommandValidator : AbstractValidator<ExportVocabularyFlashcardsCommand>
{
    public ExportVocabularyFlashcardsCommandValidator()
    {
        RuleFor(x => x.VocabularyIds)
            .NotNull().WithMessage("Ids must not be null.")
            .Must(ids => ids.Count > 0).WithMessage("At least one Id is required.")
            .ForEach(idRule => idRule.NotEmpty().WithMessage("Id must not be empty."));
    }
}