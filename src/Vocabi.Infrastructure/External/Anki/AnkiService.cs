using System.Text.Json;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.External.Flashcards;
using Vocabi.Infrastructure.External.Anki.Requests;
using Vocabi.Shared.Extensions;
using Vocabi.Shared.Utils;

namespace Vocabi.Infrastructure.External.Anki;

public class AnkiService(IAnkiConnectClient client) : IFlashcardService
{
    private bool isInitialized;
    private string cachedMediaPath;

    private const string DefaultDeck = "VocabiDeck";
    private const string DefaultNoteModel = "VocabiNoteModel";

    #region Initialization
    private async Task EnsureInitializedAsync()
    {
        if (isInitialized)
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

        if (!decks.Contains(DefaultDeck))
            await client.InvokeAsync(new AnkiRequest("createDeck", new { deck = DefaultDeck }));

        if (!models.Contains(DefaultNoteModel))
            await CreateModelAsync(DefaultNoteModel);

        isInitialized = true;
    }

    private async Task CreateModelAsync(string modelName)
    {
        var frontTemplate = @"
<div class=""container"">
	<div class=""wrapper"">
		
    <div class=""image-container"">
        {{Image}}
    </div>
		<div class=""content-container"">  
    	<div class=""suggestion"">
        {{Cloze}} 
	    </div>
			<span class=""word-type"">
				{{PartOfSpeech}}
			</span>
			<br/>
    	<div class=""meaning"">
        {{Meaning}}
    	</div>
    </div>
	</div>
 		<div class=""type-word"">
    		{{type:Word}}
   </div>
</div>
";

        var backTemplate = @"
<div class=""container"">
	<div class=""wrapper"">
		
		<div class=""check-word"">
			{{type:Word}}
		</div>
    <div class=""image-container"">
        {{Image}}
    </div>
    <div class=""content-container"">  
    	<div class=""word-detail"">
        {{Word}} 
	    </div>
			<span class=""word-type"">
				{{PartOfSpeech}}
			</span>
  	  <div class=""phonetic"">
        {{Audio}} {{Pronunciation}}
    	</div>
			<br/>
    	<div class=""meaning"">
        {{Meaning}}
    	</div>
			<div class=""example"">
			Eg: {{Example}}
			</div>
    </div>
	</div>
</div>
";

        var css = @"
@font-face {
  	font-family: NunitoSans;
  	src: url(""_NunitoSans_10pt.ttf"");
}

html, body {
		margin: 0;
  	padding: 0;
}

.container {
		font-family: 'NunitoSans', sans-serif !important;
		background-color: rgb(32, 35, 42);
		height: 100vh;
		font-size: 20px;

		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		
}

.no {
		position: absolute;
		top: 0;
		left: 0;

		padding: 20px 24px;
		width: 100%;
		
    font-size: 16px;
    font-weight: bold;
		color: rgb(196, 143, 245);
}

.check-word {
		position: absolute;
		top: 0px;
		padding: 32px 0;
		width: 100%;
		
    font-size: 20px !important;
    color: #2b6cb0;
    text-align: center;
}

.type-word {
		position: absolute;
		bottom: 80px;
		min-width: 60vw;
}

#typeans {
		padding: 6px 8px !important;
		font-size: 18px !important;
		font-family: 'NunitoSans', sans-serif !important;
		border-radius: 8px;
		text-align: center;
}

.check-word #typeans {
		padding: 0 !important;
}

.wrapper {
  	display: flex;
	  align-items: center;
  	justify-content: center;
    gap: 36px;
		color: #eee;
		margin: 0 60px;
}

.image-container {
		display: flex;
}
.image-container img {
    max-width: 360px;
    border-radius: 8px;
}

.content-container {
    display: flex;
    flex-direction: column;
}

.word-detail {
    font-size: 36px;
    font-weight: bold;
		color: rgb(253, 231, 205);
}

.suggestion {
    font-size: 18px;
		color: rgb(253, 231, 205);
}

.word-type {
    font-size: 16px;
		font-style: italic;
		color: rgb(201, 201, 201);
}

.replay-button svg {
  	width: 18px;
  	height: 18px;
}

.phonetic {
		display: flex;
	  align-items: center;
		gap: 4px;

		font-size: 16px !important;
		color: rgb(201, 201, 201);	
}

.meaning {
    font-size: 18px;
    font-weight: 500;
		color: rgb(245, 143, 199);
}

.example {
		font-size: 16px;
		color: rgb(201, 201, 201);	
}
";

        var request = new AnkiRequest(
            "createModel",
            new
            {
                modelName,
                inOrderFields = new[] { "Word", "PartOfSpeech", "Pronunciation", "Audio", "Meaning", "Example", "Cloze", "Image" },
                cardTemplates = new[]
                {
                    new {
                        Name = "VocabiCard",
                        Front = frontTemplate,
                        Back = backTemplate }
                },
                css
            }
        );

        await client.InvokeAsync(request);
    }
    #endregion

    public async Task<Result<bool>> IsAvailableAsync()
    {
        try
        {
            var result = await client.InvokeAsync(new AnkiRequest("version"));
            var version = result?.Deserialize<int>();
            return Result<bool>.Success(version > 0);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
    }

    #region Export Methods
    public async Task<Result<long>> ExportNoteAsync(FlashcardNote note, IEnumerable<string> mediaPaths)
    {
        var result = await ExportNotesAsync([note], mediaPaths);
        if (result.IsFailure)
            return Result<long>.Failure(result.Errors);

        var noteIds = result.Data;
        if (noteIds.IsNullOrEmpty())
            return Result<long>.Failure("Failed to add note, no noteId returned.");

        return Result<long>.Success(noteIds[0]);
    }

    public async Task<Result<long[]>> ExportNotesAsync(IEnumerable<FlashcardNote> notes, IEnumerable<string> mediaPaths)
    {
        try
        {
            if (notes.IsNullOrEmpty())
                return Result<long[]>.Failure("No valid notes provided for export.");

            await EnsureInitializedAsync();

            var addNotesTask = AddNotesAsync(notes);
            var copyMediaFilesTask = CopyMediaFilesAsync(mediaPaths);
            await Task.WhenAll(addNotesTask, copyMediaFilesTask);

            var successNoteIds = addNotesTask.Result;
            return Result<long[]>.Success(successNoteIds);
        }
        catch (Exception ex)
        {
            return Result<long[]>.Failure(ex.Message);
        }
    }

    private async Task<long[]> AddNotesAsync(IEnumerable<FlashcardNote> notes)
    {
        var payload = new
        {
            notes = notes.Select(x => new
            {
                deckName = DefaultDeck,
                modelName = DefaultNoteModel,
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
        if (string.IsNullOrEmpty(cachedMediaPath))
        {
            var result = await client.InvokeAsync(new AnkiRequest("getMediaDirPath"));
            cachedMediaPath = result?.Deserialize<string>()
                ?? throw new InvalidOperationException("Failed to get Anki media directory path.");
        }

        await FileUtils.CopyFilesAsync(filePaths.Select(FileUtils.GetWwwRootPath), cachedMediaPath);
    }
    #endregion

    #region Unexport Methods
    public async Task<Result> UnexportNoteAsync(long noteId)
    {
        var result = await UnexportNotesAsync([noteId]);
        if (result.IsFailure)
            return Result.Failure(result.Errors);

        return Result.Success();
    }

    public async Task<Result> UnexportNotesAsync(IEnumerable<long> noteIds)
    {
        try
        {
            if (noteIds.IsNullOrEmpty())
                return Result<long[]>.Failure("No valid note IDs provided for deletion.");

            await DeleteNotesAsync(noteIds);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    private async Task DeleteNotesAsync(IEnumerable<long> noteIds)
    {
        var payload = new { notes = noteIds };
        await client.InvokeAsync(new AnkiRequest("deleteNotes", payload));
    }
    #endregion
}