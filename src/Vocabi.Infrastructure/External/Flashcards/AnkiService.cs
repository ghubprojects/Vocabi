using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.External.Flashcards;
using Vocabi.Shared.Extensions;
using Vocabi.Shared.Utils;

namespace Vocabi.Infrastructure.External.Flashcards;

public class AnkiService(IAnkiConnectClient client) : IFlashcardService
{
    private bool _isInitialized;

    private const string DefaultDeck = "VocabiDeck";
    private const string DefaultNoteModel = "VocabiNoteModel";

    #region Initialization
    private async Task EnsureInitializedAsync()
    {
        if (_isInitialized)
            return;

        await EnsureDeckExistsAsync(DefaultDeck);
        await EnsureModelExistsAsync(DefaultNoteModel);

        _isInitialized = true;
    }

    private async Task EnsureDeckExistsAsync(string deckName)
    {
        var decks = await client.InvokeAsync<List<string>>("deckNames");
        if (!decks.Contains(deckName))
            await client.InvokeAsync<object>("createDeck", new { deck = deckName });
    }

    private async Task EnsureModelExistsAsync(string modelName)
    {
        var models = await client.InvokeAsync<List<string>>("modelNames");
        if (!models.Contains(modelName))
            await CreateModelAsync(modelName);
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

        await client.InvokeAsync<object>("createModel", new
        {
            modelName,
            inOrderFields = new[] { "Word", "PartOfSpeech", "Pronunciation", "Audio", "Meaning", "Example", "Cloze", "Image" },
            cardTemplates = new[]
            {
                    new {
                        Name = "Card",
                        Front = frontTemplate,
                        Back = backTemplate }
                },
            css
        });
    }
    #endregion

    public async Task<Result<bool>> IsAvailableAsync()
    {
        try
        {
            var version = await client.InvokeAsync<int>("version");
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
            var successNoteIds = await AddNotesAsync(notes);
            await CopyMediaFilesAsync(mediaPaths);
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
        return await client.InvokeAsync<long[]>("addNotes", payload);
    }

    private async Task CopyMediaFilesAsync(IEnumerable<string> filePaths)
    {
        var mediaPath = await client.InvokeAsync<string>("getMediaDirPath");
        await FileUtils.CopyFilesAsync(filePaths.Select(FileUtils.GetWwwRootPath), mediaPath);
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
        await client.InvokeAsync<long[]>("deleteNotes", payload);
    }
    #endregion
}