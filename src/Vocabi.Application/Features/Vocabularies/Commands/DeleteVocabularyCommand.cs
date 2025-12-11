using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Vocabi.Application.Contracts.External.Flashcards;
using Vocabi.Domain.Aggregates.MediaFiles;
using Vocabi.Domain.Aggregates.Vocabularies;

namespace Vocabi.Application.Features.Vocabularies.Commands;


public record DeleteVocabularyCommand(Guid VocabularyId) : IRequest<Result>;

public class DeleteVocabularyCommandHandler(
    IVocabularyRepository vocabularyRepository,
    IMediaFileRepository mediaFileRepository,
    IFlashcardService flashcardService,
    ILogger<DeleteVocabularyCommandHandler> logger
    ) : IRequestHandler<DeleteVocabularyCommand, Result>
{
    public Task<Result> Handle(DeleteVocabularyCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}