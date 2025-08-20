using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.External.Flashcards;

namespace Vocabi.Infrastructure.External.Flashcards;

public class AnkiConnectExporter(IAnkiConnectClient ankiConnectClient) : IFlashcardExporter
{
    public async Task<Result<long?>> ExportAsync(FlashcardNote note, ExportOptions options, CancellationToken cancellationToken = default)
    {
        var parameters = new
        {
            notes = new[]
            {
                new
                {
                    deckName = options.DeckName,
                    modelName = options.ModelName,
                    fields = new
                    {
                        note.Word,
                        note.PartOfSpeech,
                        note.Pronunciation,
                        note.Cloze,
                        note.Definition,
                        note.Example,
                        note.Meaning,
                        note.Audio,
                        note.Image
                    }
                }
            }
        };

        var response = await ankiConnectClient.InvokeAsync<IReadOnlyList<long>>("addNotes", parameters, cancellationToken);
        if (response.Error is not null)
            return Result<long?>.Failure(response.Error);

        return Result<long?>.Success(response.Result![0]);
    }

    public async Task<Result<IReadOnlyList<long>>> ExportAsync(IEnumerable<FlashcardNote> notes, ExportOptions options, CancellationToken cancellationToken = default)
    {
        return Result<IReadOnlyList<long>>.Success([]);
    }
}