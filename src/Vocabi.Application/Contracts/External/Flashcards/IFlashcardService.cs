using FluentResults;

namespace Vocabi.Application.Contracts.External.Flashcards;

public interface IFlashcardService
{
    Task<Result<bool>> IsAvailableAsync();
    Task<Result<long>> ExportNoteAsync(FlashcardNote note, IEnumerable<string> mediaPaths);
    Task<Result<long[]>> ExportNotesAsync(IEnumerable<FlashcardNote> notes, IEnumerable<string> mediaPaths);
    Task<Result> UnexportNoteAsync(long noteId);
    Task<Result> UnexportNotesAsync(IEnumerable<long> noteIds);
}