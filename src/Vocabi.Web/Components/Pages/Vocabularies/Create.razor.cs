using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;
using Vocabi.Application.Features.LookupEntries.Commands;
using Vocabi.Application.Features.LookupEntries.DTOs;
using Vocabi.Application.Features.MediaFiles.Commands;
using Vocabi.Application.Features.Vocabularies.Commands;
using Vocabi.Domain.Aggregates.MediaFiles;
using Vocabi.Web.Extensions;
using Vocabi.Web.Models;
using Icons = Microsoft.FluentUI.AspNetCore.Components.Icons;

namespace Vocabi.Web.Components.Pages.Vocabularies;

public partial class Create
{
    private EditContext editContext = default!;
    private bool isLookingUp;
    protected bool isSubmitting;

    private VocabularyFormModel formModel = new();
    private List<MediaFileInputModel> mediaFileModels = [];
    private List<LookupEntryDto> lookupEntries = [];

    protected override async Task OnInitializedAsync()
    {
        editContext = new EditContext(formModel);
        mediaFileModels = [
            new MediaFileInputModel
            {
                MediaType = MediaType.Audio,
                MaxFileSizeInKB = 100,
                Accept = "audio/*",
                AnchorId = "audio-file-input",
                Placeholder = "Select an audio file (≤100 KB)",
                Icon = new Icons.Regular.Size32.Headphones(),
            },
            new MediaFileInputModel
            {
                MediaType = MediaType.Image,
                MaxFileSizeInKB = 500,
                Accept = "image/*",
                AnchorId = "image-file-input",
                Placeholder = "Select an image file (≤500 KB)",
                Icon = new Icons.Regular.Size32.Image(),
            }
        ];
    }

    private async Task SubmitAsync()
    {
        if (!editContext.Validate() || isSubmitting)
            return;

        await ExecuteWithLoadingAsync(async () =>
        {
            var command = new CreateVocabularyCommand
            {
                Word = formModel.Word,
                PartOfSpeech = formModel.PartOfSpeech,
                Pronunciation = formModel.Pronunciation,
                Cloze = formModel.Cloze,
                Definition = formModel.Definition,
                Example = formModel.Example,
                Meaning = formModel.Meaning,
                MediaFileIds = [.. mediaFileModels.Select(x => x.Id)],
            };

            var result = await Mediator.Send(command);
            if (result.IsSuccess)
                Navigation.GoToVocabularyList();
            else
                ToastService.ShowError(result.ErrorMessages);
        }, x => isSubmitting = x);
    }

    private void ClearForm()
    {
        //ViewModel.Detail = new VocabDetailViewModel();
        //_editContext = new EditContext(ViewModel.Detail);

        //_lookupResult = new();
        //_mediaLookupResult = new();

        //_availableWordTypes.Clear();
        //_availableDefinitions.Clear();
        //_availableExamples.Clear();
        //_availableMeanings.Clear();
        //_availableImages.Clear();

        //_imageIndex = 0;
    }

    private async Task LookupAsync()
    {
        if (string.IsNullOrWhiteSpace(formModel.Word))
        {
            ToastService.ShowWarning("Please enter a word first.");
            return;
        }

        await ExecuteWithLoadingAsync(async () =>
        {
            var result = await Mediator.Send(new LookupWordCommand { Word = formModel.Word });
            if (result.IsFailure)
            {
                ToastService.ShowWarning(result.ErrorMessages);
                return;
            }

            lookupEntries = result.Data;
            var defaultEntry = lookupEntries.First();

            formModel = VocabularyFormModel.FromEntry(defaultEntry);

            foreach (var fileModel in mediaFileModels)
            {
                var mediaFile = defaultEntry.MediaFiles.FirstOrDefault(x => x.MediaType == fileModel.MediaType);
                if (mediaFile is null)
                    continue;

                fileModel.UpdateFrom(mediaFile);
            }
        }, x => isLookingUp = x);
    }

    private void HandleChangeWordType()
    {
        try
        {
            //ViewModel.Detail.WordType = wordType;

            //if (_lookupResult is null)
            //    return;

            //var entry = _lookupResult.Entries.Find(e => e.WordType == ViewModel.Detail.WordType);
            //if (entry is null)
            //    return;

            //ViewModel.Detail.Phonetic = entry.Phonetic;

            //_availableDefinitions.Clear();
            //_availableDefinitions.AddRange(entry.Definitions.Select(d => d.Definition));
            //ViewModel.Detail.Definition = _availableDefinitions.First();

            //UpdateExamples();
        }
        catch (Exception e)
        {
            ToastService.ShowError($"Error changing word type: {e.Message}");
        }
    }

    private void HandleChangeDefinition()
    {
        try
        {
            //if (_lookupResult is null)
            //    return;

            //var entry = _lookupResult.Entries.Find(
            //    e => e.Definitions.Any(d => d.Definition == ViewModel.Detail.Definition));
            //if (entry is null)
            //    return;

            //ViewModel.Detail.WordType = entry.WordType;
            //ViewModel.Detail.Phonetic = entry.Phonetic;

            //UpdateExamples();
        }
        catch (Exception e)
        {
            ToastService.ShowError($"Error changing definition: {e.Message}");
        }
    }

    private void HandleChangeExample()
    {
        try
        {
            //if (_lookupResult is null)
            //    return;

            //var entry = _lookupResult.Entries.Find(
            //    e => e.Definitions.Any(d => d.Examples.Contains(ViewModel.Detail.Example)));
            //if (entry is null)
            //    return;

            //ViewModel.Detail.WordType = entry.WordType;
            //ViewModel.Detail.Phonetic = entry.Phonetic;

            //var definition = entry.Definitions.Find(d => d.Examples.Contains(ViewModel.Detail.Example));
            //if (definition is not null)
            //{
            //    ViewModel.Detail.Definition = definition.Definition;
            //}
        }
        catch (Exception e)
        {
            ToastService.ShowError($"Error changing example: {e.Message}");
        }
    }

    //private void UpdateExamples()
    //{
    //    try
    //    {
    //        if (_lookupResult is null)
    //            return;

    //        var entry = _lookupResult.Entries.Find(
    //            e => e.Definitions.Any(d => d.Definition == ViewModel.Detail.Definition));
    //        if (entry is null)
    //            return;

    //        _availableExamples.Clear();
    //        _availableExamples.AddRange(
    //            entry.Definitions
    //            .Where(d => d.Definition == ViewModel.Detail.Definition)
    //            .SelectMany(d => d.Examples));
    //        ViewModel.Detail.Example = _availableExamples.First();
    //    }
    //    catch (Exception e)
    //    {
    //        ToastService.ShowError($"Error updating examples: {e.Message}");
    //    }
    //}

    protected async Task HandleFileUploadCompletedAsync(IEnumerable<FluentInputFileEventArgs> files, MediaFileInputModel fileModel)
    {
        var file = files.First();
        ////if (!IsValidFile(file, inputFile))
        ////    return;

        //var fileName = FileHelper.NormalizeFileName(ViewModel.Detail.Word, Path.GetExtension(file.Name));
        //var tempFilePath = FileHelper.GetFullTempUploadPath(fileName, Environment.WebRootPath);

        var command = new UploadMediaFileCommand
        {
            Stream = file.Stream,
            Filename = file.Name
        };

        var result = await Mediator.Send(command);
        if (result.IsFailure)
        {
            ToastService.ShowWarning(result.ErrorMessages);
            return;
        }

        fileModel.UpdateFrom(result.Data);

        //inputFile.FilePath = FileHelper.GetRelativePath(tempFilePath, Environment.WebRootPath);
        //inputFile.FileSource = FileSource.Local;
        //ToastService.ShowSuccess($"File '{file.Name}' uploaded successfully.");
    }

    protected void HandleRemoveFile(MediaType type)
    {
        //ViewModel.Detail.RemoveFile(fileType);
        //if (fileType == MediaType.Image)
        //    _availableImages.Clear();
    }

    //private async Task LookupMediaAsync()
    //{
    //    if (_lookupResult is null)
    //    {
    //        ToastService.ShowWarning("Please search for a word first.");
    //        return;
    //    }

    //    if (string.IsNullOrEmpty(ViewModel.Detail.Word)
    //        || string.IsNullOrEmpty(ViewModel.Detail.WordType)
    //        || string.IsNullOrEmpty(ViewModel.Detail.Definition))
    //    {
    //        ToastService.ShowWarning("Please complete the word, word type, and definition first.");
    //        return;
    //    }

    //    if (_isMediaSearching)
    //        return;

    //    _isMediaSearching = true;
    //    _imageIndex = 0;
    //    StateHasChanged();

    //    try
    //    {
    //        var entry = _lookupResult.Entries.First(e => e.WordType == ViewModel.Detail.WordType);
    //        var definition = entry.Definitions.First(d => d.Definition == ViewModel.Detail.Definition);

    //        var result = await VocabService.LookupMediaAsync(entry.AudioUrl, definition.ImageUrl, ViewModel.Detail.Word);
    //        if (!result.Success)
    //            ToastService.ShowWarning(result.Error);

    //        _mediaLookupResult = result.Data!;
    //        if (_mediaLookupResult is null)
    //            return;

    //        foreach (var inputFile in ViewModel.Detail.InputFiles)
    //        {
    //            switch (inputFile.FileType)
    //            {
    //                case FileType.Audio:
    //                    inputFile.FilePath = _mediaLookupResult.AudioFilePath;
    //                    inputFile.FileSource = _mediaLookupResult.AudioFileSource;
    //                    break;
    //                case FileType.Image:
    //                    inputFile.FilePath = _mediaLookupResult.ImageFilePath;
    //                    inputFile.FileSource = _mediaLookupResult.ImageFileSource;
    //                    break;

    //            }
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        ToastService.ShowError($"Error during searching media: {e.Message}");
    //    }
    //    finally
    //    {
    //        _isMediaSearching = false;
    //        StateHasChanged();
    //    }
    //}
}