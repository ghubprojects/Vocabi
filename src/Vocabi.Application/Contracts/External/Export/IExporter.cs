using Vocabi.Application.Common.Models;
using Vocabi.Domain.Aggregates.Vocabularies;

namespace Vocabi.Application.Contracts.External.Export;

public interface IExporter : IExternalProvider
{
    Task<Result> ExportAsync(IEnumerable<Vocabulary> vocabularies, CancellationToken cancellationToken = default);
}