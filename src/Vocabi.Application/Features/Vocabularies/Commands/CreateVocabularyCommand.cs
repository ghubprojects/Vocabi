using FluentResults;
using MediatR;
using Vocabi.Domain.Aggregates.Vocabularies;
using Vocabi.Shared.Utils;

namespace Vocabi.Application.Features.Vocabularies.Commands;

public record CreateVocabularyCommand(
    string Word,
    string PartOfSpeech,
    string Pronunciation,
    string Cloze,
    string Definition,
    string Example,
    string Meaning,
    Guid AudioFileId,
    Guid ImageFileId
) : IRequest<Result>;

public class CreateVocabularyCommandHandler(IVocabularyRepository vocabularyRepository)
    : IRequestHandler<CreateVocabularyCommand, Result>
{
    public async Task<Result> Handle(CreateVocabularyCommand request, CancellationToken cancellationToken)
    {
        var vocabulary = Vocabulary.Create(
            request.Word,
            request.PartOfSpeech,
            FormatterUtils.TrimSlashes(request.Pronunciation),
            request.Cloze,
            request.Definition,
            request.Example,
            request.Meaning,
            [request.AudioFileId, request.ImageFileId]
        );
        vocabularyRepository.Add(vocabulary);

        await vocabularyRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return Result.Ok();
    }
}