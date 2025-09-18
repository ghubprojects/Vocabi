using Vocabi.Application.Common.Models;

namespace Vocabi.Application.Contracts.External.Flashcards;

public interface IFlashcardService
{
    Task<Result<bool>> IsAvailableAsync();
    Task<Result<long>> ExportNoteAsync(FlashcardNote note, IEnumerable<string> mediaPaths);
    Task<Result<long[]>> ExportNotesAsync(IEnumerable<FlashcardNote> notes, IEnumerable<string> mediaPaths);
}