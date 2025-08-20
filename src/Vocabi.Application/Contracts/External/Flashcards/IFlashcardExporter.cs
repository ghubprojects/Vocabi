using Vocabi.Application.Common.Models;

namespace Vocabi.Application.Contracts.External.Flashcards;

public interface IFlashcardExporter
{
    Task<Result<long?>> ExportAsync(FlashcardNote note, ExportOptions options, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<long>>> ExportAsync(IEnumerable<FlashcardNote> notes, ExportOptions options, CancellationToken cancellationToken = default);
}