using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;
using Vocabi.Application.Features.LookupEntries.Commands;
using Vocabi.Application.Features.LookupEntries.DTOs;
using Vocabi.Application.Features.LookupEntries.Queries;
using Vocabi.Application.Features.MediaFiles.Commands;
using Vocabi.Application.Features.MediaFiles.DTOs;
using Vocabi.Application.Features.MediaFiles.Queries;
using Vocabi.Application.Features.Vocabularies.Commands;
using Vocabi.Shared.Extensions;
using Vocabi.Shared.Utils;
using Vocabi.Web.Models.Vocabularies;
using static Vocabi.Shared.Common.Enums;

namespace Vocabi.Web.Pages.Vocabularies.Pending;

public partial class Create
{
    private EditContext editContext = default!;

    private VocabularyFormModel vocabularyForm = new();
    private IReadOnlyList<LookupEntryDto> lookupEntryDtos = [];

    private MediaFileDto audioFile = new();
    private MediaFileDto imageFile = new();
    private List<MediaFileDto> alternativeImageFiles = [];

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
                Pronunciation = FormatterUtils.TrimSlashes(vocabularyForm.Pronunciation),
                Cloze = vocabularyForm.Cloze,
                Definition = vocabularyForm.Definition,
                Example = vocabularyForm.Example,
                Meaning = vocabularyForm.Meaning,
                MediaFileIds = [audioFile.Id, imageFile.Id]
            };

            var result = await Mediator.Send(command);
            if (result.IsSuccess)
                Navigation.GoToVocabularyPendingList();
        },
        x => isSubmitting = x);
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
            if (result.IsFailed)
            {
                ToastService.ShowWarning(result.GetErrorMessages());
                return;
            }

            lookupEntryDtos = await Mediator.Send(new GetLookupEntriesQuery { Ids = result.Value });
            var defaultEntry = lookupEntryDtos[0];

            vocabularyForm.UpdateFromEntry(defaultEntry);

            var mediaFileDtos = await Mediator.Send(new GetMediaFilesQuery { Ids = defaultEntry.MediaFileIds });
            var audio = mediaFileDtos.FirstOrDefault(x => FileUtils.GetMediaType(x.ContentType) == MediaType.Audio);
            if (audio is not null)
                audioFile = audio;
            var image = mediaFileDtos.FirstOrDefault(x => FileUtils.GetMediaType(x.ContentType) == MediaType.Image);
            if (image is not null)
                imageFile = image;
            if (mediaFileDtos.Count > 1)
                alternativeImageFiles = [.. mediaFileDtos
                    .Where(x => FileUtils.GetMediaType(x.ContentType) == MediaType.Image && x.Id != imageFile.Id)];

            await InvokeAsync(StateHasChanged);
        },
        x => isLookingUp = x);
    }

    private void HandleChangeWordType()
    {
        try
        {
            //Model.Detail.WordType = wordType;

            //if (_lookupResult is null)
            //    return;

            //var entry = _lookupResult.Entries.Find(e => e.WordType == Model.Detail.WordType);
            //if (entry is null)
            //    return;

            //Model.Detail.Phonetic = entry.Phonetic;

            //_availableDefinitions.Clear();
            //_availableDefinitions.AddRange(entry.Definitions.Select(d => d.Definition));
            //Model.Detail.Definition = _availableDefinitions.First();

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
            //    e => e.Definitions.Any(d => d.Definition == Model.Detail.Definition));
            //if (entry is null)
            //    return;

            //Model.Detail.WordType = entry.WordType;
            //Model.Detail.Phonetic = entry.Phonetic;

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
            //    e => e.Definitions.Any(d => d.Examples.Contains(Model.Detail.Example)));
            //if (entry is null)
            //    return;

            //Model.Detail.WordType = entry.WordType;
            //Model.Detail.Phonetic = entry.Phonetic;

            //var definition = entry.Definitions.Find(d => d.Examples.Contains(Model.Detail.Example));
            //if (definition is not null)
            //{
            //    Model.Detail.Definition = definition.Definition;
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
    //            e => e.Definitions.Any(d => d.Definition == Model.Detail.Definition));
    //        if (entry is null)
    //            return;

    //        _availableExamples.Clear();
    //        _availableExamples.AddRange(
    //            entry.Definitions
    //            .Where(d => d.Definition == Model.Detail.Definition)
    //            .SelectMany(d => d.Examples));
    //        Model.Detail.Example = _availableExamples.First();
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

    //    //var fileName = FileHelper.NormalizeFileName(Model.Detail.Word, Path.GetExtension(file.Name));
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

        //var fileName = FileHelper.NormalizeFileName(Model.Detail.Word, Path.GetExtension(file.Name));
        //var tempFilePath = FileHelper.GetFullTempUploadPath(fileName, Environment.WebRootPath);

        var command = new UploadMediaFileCommand
        {
            Stream = uploadedFile.Stream!,
            Filename = uploadedFile.Name
        };

        var result = await Mediator.Send(command);
        if (result.IsFailed)
        {
            ToastService.ShowWarning(result.GetErrorMessages());
            return;
        }

        //fileModel.UpdateFrom(result.Data);
        if (FileUtils.GetMediaType(result.Value.ContentType) == MediaType.Audio)
            audioFile = result.Value;
        else if (FileUtils.GetMediaType(result.Value.ContentType) == MediaType.Image)
            imageFile = result.Value;


        //inputFile.FilePath = FileHelper.GetRelativePath(tempFilePath, Environment.WebRootPath);
        //inputFile.FileSource = FileSource.Local;
        //ToastService.ShowSuccess($"File '{file.Name}' uploaded successfully.");
    }

    protected void HandleRemoveFile(MediaType type)
    {
        //Model.Detail.RemoveFile(fileType);
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