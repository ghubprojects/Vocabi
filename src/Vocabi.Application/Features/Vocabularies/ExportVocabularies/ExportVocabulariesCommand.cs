using MediatR;
using Vocabi.Application.Common.Models;

namespace Vocabi.Application.Features.Vocabularies.ExportVocabularies;

public class ExportVocabulariesCommand : IRequest<Result>
{
}

public class ExportVocabulariesToAnkiCommandHandler : IRequestHandler<ExportVocabulariesCommand, Result>
{
    public Task<Result> Handle(ExportVocabulariesCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
