using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.External.Flashcards;

namespace Vocabi.Infrastructure.External.Flashcards;

public class AnkiConnectService(IAnkiConnectClient ankiConnectClient) : IFlashcardService
{
    public async Task EnsureDeckAsync(CancellationToken cancellationToken = default)
    {
        var response = await ankiConnectClient.InvokeAsync<string[]>("deckNames", new { }, cancellationToken);
        if (response.Error is not null)
            throw new Exception(response.Error);

        var existingDecks = response.Result ?? [];
        if (!existingDecks.Contains("VocabiDeck"))
            await ankiConnectClient.InvokeAsync<object>("createDeck", new { deck = "VocabiDeck" }, cancellationToken);
    }

    public async Task EnsureNoteModelAsync(CancellationToken cancellationToken = default)
    {
        var response = await ankiConnectClient.InvokeAsync<string[]>("modelNames", new { }, cancellationToken);
        if (response.Error is not null)
            throw new Exception(response.Error);

        var existingModels = response.Result ?? [];
        if (!existingModels.Contains("VocabiNote"))
        {
            // 2. Tạo mới
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

            var parameters = new
            {
                modelName = "VocabiNote",
                inOrderFields = new[]
                {
                    "Word", "PartOfSpeech", "Pronunciation", "Audio", "Meaning", "Example", "Cloze", "Image"
                },
                css,
                cardTemplates = new[]
                {
                    new { Name = "VocabiNote", Front = frontTemplate, Back = backTemplate }
                }
            };

            await ankiConnectClient.InvokeAsync<object>("createModel", parameters, cancellationToken);
        }
        else
        {
            // 3. Nếu muốn update thì gọi updateModelTemplates hoặc updateModelStyling
            // Ví dụ:
            // await InvokeAsync<object>("updateModelTemplates", new {
            //     model = "VocabiNote",
            //     templates = new {
            //         VocabiNote = new { Front = frontTemplate, Back = backTemplate }
            //     }
            // }, ct);
        }
    }

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

    public async Task<Result<string?>> GetMediaDirectoryPath(CancellationToken cancellationToken = default)
    {
        var response = await ankiConnectClient.InvokeAsync<string>("getMediaDirPath", new { }, cancellationToken);
        if (response.Error is not null)
            return Result<string?>.Failure(response.Error);

        return Result<string?>.Success(response.Result);
    }
}