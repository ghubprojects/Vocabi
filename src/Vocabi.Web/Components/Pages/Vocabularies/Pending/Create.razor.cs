using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;
using Vocabi.Application.Features.LookupEntries.CreateLookupEntries;
using Vocabi.Application.Features.LookupEntries.GetLookupEntries;
using Vocabi.Application.Features.MediaFiles.GetMediaFiles;
using Vocabi.Application.Features.MediaFiles.UploadMediaFile;
using Vocabi.Application.Features.Vocabularies.CreateVocabulary;
using Vocabi.Shared.Utils;
using Vocabi.Web.ViewModels;
using static Vocabi.Shared.Common.Enums;

namespace Vocabi.Web.Components.Pages.Vocabularies.Pending;

public partial class Create
{
    private EditContext editContext = default!;

    private PendingVocabularyFormViewModel vocabularyForm = new();
    private IReadOnlyList<LookupEntryDto> lookupEntryDtos = [];

    private MediaFileDto audioFile = new();
    private MediaFileDto imageFile = new();

    private bool isSubmitting;
    private bool isLookingUp;

    protected override void OnInitialized()
    {
        editContext = new EditContext(vocabularyForm);
    }

    private async Task SubmitAsync()
    {
        if (!editContext.Validate() || isSubmitting)
            return;

        await ExecuteWithLoadingAsync(async () =>
        {
            var command = new CreateVocabularyCommand
            {
                Word = vocabularyForm.Word,
                PartOfSpeech = vocabularyForm.PartOfSpeech,
                Pronunciation = StringUtils.UnwrapSlashes(vocabularyForm.Pronunciation),
                Cloze = vocabularyForm.Cloze,
                Definition = vocabularyForm.Definition,
                Example = vocabularyForm.Example,
                Meaning = vocabularyForm.Meaning,
                MediaFileIds = [audioFile.Id, imageFile.Id]
            };

            var result = await Mediator.Send(command);
            if (result.IsSuccess)
                Navigation.GoToVocabularyPendingList();
            else
                ToastService.ShowError(result.ErrorMessages);
        }, x => isSubmitting = x);
    }

    private void ClearForm()
    {
        vocabularyForm = new();
        lookupEntryDtos = [];
        audioFile = new();
        imageFile = new();
        editContext = new EditContext(vocabularyForm);
    }

    private async Task LookupAsync()
    {
        if (string.IsNullOrWhiteSpace(vocabularyForm.Word))
        {
            ToastService.ShowWarning("Please enter a word first.");
            return;
        }

        await ExecuteWithLoadingAsync(async () =>
        {
            var result = await Mediator.Send(new CreateLookupEntriesCommand { Word = vocabularyForm.Word });
            if (result.IsFailure)
            {
                ToastService.ShowWarning(result.ErrorMessages);
                return;
            }

            lookupEntryDtos = await Mediator.Send(new GetLookupEntriesQuery { Ids = result.Data });
            var defaultEntry = lookupEntryDtos[0];

            vocabularyForm.UpdateFromEntry(defaultEntry);

            var mediaFileDtos = await Mediator.Send(new GetMediaFilesQuery { Ids = defaultEntry.MediaFileIds });
            var audio = mediaFileDtos.FirstOrDefault(x => FileUtils.GetMediaType(x.ContentType) == MediaType.Audio);
            if (audio is not null)
                audioFile = audio;
            var image = mediaFileDtos.FirstOrDefault(x => FileUtils.GetMediaType(x.ContentType) == MediaType.Image);
            if (image is not null)
                imageFile = image;

            await InvokeAsync(StateHasChanged);
        },
        x => isLookingUp = x);
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

    //protected async Task HandleFileUploadCompletedAsync(IEnumerable<FluentInputFileEventArgs> files, MediaFileInputModel fileModel)
    //{
    //    var file = files.First();
    //    ////if (!IsValidFile(file, inputFile))
    //    ////    return;

    //    //var fileName = FileHelper.NormalizeFileName(ViewModel.Detail.Word, Path.GetExtension(file.Name));
    //    //var tempFilePath = FileHelper.GetFullTempUploadPath(fileName, Environment.WebRootPath);

    //    var command = new UploadMediaFileCommand
    //    {
    //        Stream = file.Stream,
    //        Filename = file.Name
    //    };

    //    var result = await Mediator.Send(command);
    //    if (result.IsFailure)
    //    {
    //        ToastService.ShowWarning(result.ErrorMessages);
    //        return;
    //    }

    //    fileModel.UpdateFrom(result.Data);

    //    //inputFile.FilePath = FileHelper.GetRelativePath(tempFilePath, Environment.WebRootPath);
    //    //inputFile.FileSource = FileSource.Local;
    //    //ToastService.ShowSuccess($"File '{file.Name}' uploaded successfully.");
    //}

    protected async Task HandleFileUploadedAsync(FluentInputFileEventArgs uploadedFile)
    {
        //var file = files.First();
        ////if (!IsValidFile(file, inputFile))
        ////    return;

        //var fileName = FileHelper.NormalizeFileName(ViewModel.Detail.Word, Path.GetExtension(file.Name));
        //var tempFilePath = FileHelper.GetFullTempUploadPath(fileName, Environment.WebRootPath);

        var command = new UploadMediaFileCommand
        {
            Stream = uploadedFile.Stream!,
            Filename = uploadedFile.Name
        };

        var result = await Mediator.Send(command);
        if (result.IsFailure)
        {
            ToastService.ShowWarning(result.ErrorMessages);
            return;
        }

        //fileModel.UpdateFrom(result.Data);
        if (FileUtils.GetMediaType(result.Data.ContentType) == MediaType.Audio)
            audioFile = result.Data;
        else if (FileUtils.GetMediaType(result.Data.ContentType) == MediaType.Image)
            imageFile = result.Data;


        //inputFile.FilePath = FileHelper.GetRelativePath(tempFilePath, Environment.WebRootPath);
        //inputFile.FileSource = FileSource.Local;
        //ToastService.ShowSuccess($"File '{file.Name}' uploaded successfully.");
    }

    protected void HandleRemoveFile(MediaType type)
    {
        //ViewModel.Detail.RemoveFile(fileType);
        //if (fileType == MediaType.Image)
        //    _availableImages.Clear();
        switch (type)
        {
            case MediaType.Audio:
                audioFile = new();
                break;
            case MediaType.Image:
                imageFile = new();
                break;
            default:
                break;
        }
    }
}