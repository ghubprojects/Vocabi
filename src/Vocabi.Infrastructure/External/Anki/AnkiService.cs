using FluentResults;
using System.Text.Json;
using Vocabi.Application.Contracts.External.Flashcards;
using Vocabi.Infrastructure.External.Anki.Requests;
using Vocabi.Infrastructure.External.Anki.Templates;
using Vocabi.Shared.Extensions;
using Vocabi.Shared.Utils;

namespace Vocabi.Infrastructure.External.Anki;

public class AnkiService(IAnkiConnectClient client) : IFlashcardService
{
    private bool _initialized;
    private string? _mediaPath;

    private const string DeckName = "VocabiDeck_Prod";
    private const string ModelName = "VocabiNoteModel";
    private const string CardName = "VocabiCard";

    #region Initialization
    private async Task EnsureInitializedAsync()
    {
        if (_initialized)
            return;

        var results = await client.InvokeMultiAsync(
            new AnkiRequest("deckNames"),
            new AnkiRequest("modelNames")
        );

        if (results.IsNullOrEmpty())
            throw new InvalidOperationException("Failed to initialize AnkiService: invalid response from AnkiConnect.");

        if (results.Any(r => r.Error is not null))
            throw new InvalidOperationException($"Failed to initialize AnkiService: {string.Join("; ", results.Where(r => r.Error is not null).Select(r => r.Error))}");

        var decks = results[0].Result.Deserialize<string[]>() ?? [];
        var models = results[1].Result.Deserialize<string[]>() ?? [];

        if (!decks.Contains(DeckName))
            await client.InvokeAsync(new AnkiRequest("createDeck", new { deck = DeckName }));

        if (!models.Contains(ModelName))
            await CreateModelAsync(ModelName);

        _initialized = true;
    }

    private async Task CreateModelAsync(string modelName)
    {
        var payload = new
        {
            modelName,
            inOrderFields = ModelFields.All,
            cardTemplates = new[]
            {
                new {
                    Name = CardName,
                    CardTemplates.Front,
                    CardTemplates.Back
                }
            },
            css = CardTemplates.Style
        };

        await client.InvokeAsync(new AnkiRequest("createModel", payload));
    }
    #endregion

    public async Task<Result<bool>> IsAvailableAsync()
    {
        var result = await client.InvokeAsync(new AnkiRequest("version"));
        return Result.Ok(result?.Deserialize<int>() > 0);
    }

    #region Export Methods
    public async Task<Result<long>> ExportNoteAsync(FlashcardNote note, IEnumerable<string> mediaPaths)
    {
        var result = await ExportNotesAsync([note], mediaPaths);
        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var noteIds = result.Value;
        if (noteIds.IsNullOrEmpty())
            return Result.Fail("Failed to add note, no noteId returned.");

        return Result.Ok(noteIds[0]);
    }

    public async Task<Result<long[]>> ExportNotesAsync(IEnumerable<FlashcardNote> notes, IEnumerable<string> mediaPaths)
    {
        if (notes.IsNullOrEmpty())
            return Result.Fail("No valid notes provided for export.");

        await EnsureInitializedAsync();

        var addNotesTask = AddNotesAsync(notes);
        var copyMediaFilesTask = CopyMediaFilesAsync(mediaPaths);
        await Task.WhenAll(addNotesTask, copyMediaFilesTask);

        var successNoteIds = addNotesTask.Result;
        return Result.Ok(successNoteIds);
    }

    private async Task<long[]> AddNotesAsync(IEnumerable<FlashcardNote> notes)
    {
        var payload = new
        {
            notes = notes.Select(x => new
            {
                deckName = DeckName,
                modelName = ModelName,
                fields = new
                {
                    x.Word,
                    x.PartOfSpeech,
                    x.Pronunciation,
                    x.Cloze,
                    x.Definition,
                    x.Example,
                    x.Meaning,
                    x.Audio,
                    x.Image
                }
            })
        };
        var result = await client.InvokeAsync(new AnkiRequest("addNotes", payload));
        var noteIds = result?.Deserialize<long[]>() ?? [];
        return noteIds;
    }

    private async Task CopyMediaFilesAsync(IEnumerable<string> filePaths)
    {
        if (string.IsNullOrEmpty(_mediaPath))
        {
            var result = await client.InvokeAsync(new AnkiRequest("getMediaDirPath"));
            _mediaPath = result?.Deserialize<string>()
                ?? throw new InvalidOperationException("Failed to get Anki media directory path.");
        }

        await FileUtils.CopyFilesAsync(filePaths.Select(FileUtils.GetWwwRootPath), _mediaPath);
    }
    #endregion

    #region Unexport Methods
    public async Task<Result> UnexportNoteAsync(long id)
        => (await UnexportNotesAsync([id]));

    public async Task<Result> UnexportNotesAsync(IEnumerable<long> noteIds)
    {

        if (noteIds.IsNullOrEmpty())
            return Result.Fail("No valid note IDs provided for deletion.");

        await DeleteNotesAsync(noteIds);
        return Result.Ok();
    }

    private Task DeleteNotesAsync(IEnumerable<long> ids)
        => client.InvokeAsync(new AnkiRequest("deleteNotes", new { notes = ids }));

    #endregion
}