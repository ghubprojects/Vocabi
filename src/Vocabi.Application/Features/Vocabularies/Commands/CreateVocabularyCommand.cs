using MediatR;
using Vocabi.Application.Common.Models;
using Vocabi.Domain.Aggregates.Vocabularies;

namespace Vocabi.Application.Features.Vocabularies.Commands;

public class CreateVocabularyCommand : IRequest<Result>
{
    public string Word { get; set; } = string.Empty;
    public string PartOfSpeech { get; set; } = string.Empty;
    public string Pronunciation { get; set; } = string.Empty;
    public string Cloze { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public string Example { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public List<Guid> MediaFileIds { get; set; } = [];
}

public class CreateVocabularyCommandHandler(IVocabularyRepository vocabularyRepository) : IRequestHandler<CreateVocabularyCommand, Result>
{
    public async Task<Result> Handle(CreateVocabularyCommand request, CancellationToken cancellationToken)
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

        var isSaveSuccess = await vocabularyRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        if (!isSaveSuccess)
            return Result.Failure("Failed to save the vocabulary to the repository.");

        return Result.Success();
    }
}