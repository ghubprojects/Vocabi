using MediatR;
using Vocabi.Application.Common.Models;
using Vocabi.Domain.Aggregates.Vocabularies;

namespace Vocabi.Application.Features.Vocabularies.Commands;

public class CreateVocabularyCommand : IRequest<Result>
{
    public string Word { get; init; } = string.Empty;
    public string PartOfSpeech { get; init; } = string.Empty;
    public string Pronunciation { get; init; } = string.Empty;
    public string Cloze { get; init; } = string.Empty;
    public string Definition { get; init; } = string.Empty;
    public string Example { get; init; } = string.Empty;
    public string Meaning { get; init; } = string.Empty;
    public List<Guid> MediaFileIds { get; init; } = [];
}

public class CreateVocabularyCommandHandler(IVocabularyRepository vocabularyRepository) : IRequestHandler<CreateVocabularyCommand, Result>
{
    public async Task<Result> Handle(CreateVocabularyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var vocabulary = Vocabulary.CreateNew(
                request.Word,
                request.PartOfSpeech,
                request.Pronunciation,
                request.Cloze,
                request.Definition,
                request.Example,
                request.Meaning
            );
            vocabulary.AttachMediaFiles(request.MediaFileIds);

            await vocabularyRepository.AddAsync(vocabulary);
            await vocabularyRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure("Failed to create new vocabularies.");
        }
    }
}