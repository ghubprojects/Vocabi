//using AutoMapper;
//using MediatR;
//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Forms;
//using Microsoft.FluentUI.AspNetCore.Components;
//using Vocabi.Application.Features.LookupEntries.DTOs;
//using Vocabi.Application.Features.MediaFiles.DTOs;
//using Vocabi.Application.Features.MediaFiles.Queries;
//using Vocabi.Application.Features.Vocabularies.Commands;
//using Vocabi.Application.Features.Vocabularies.DTOs;
//using Vocabi.Application.Features.Vocabularies.Queries;
//using Vocabi.Domain.Aggregates.Vocabularies;
//using Vocabi.Shared.Utils;
//using WebHost.Common.Helpers;
//using WebHost.Services.Navigation;
//using WebHost.ViewModels.Vocabularies;
//using static Vocabi.Shared.Common.Enums;

//namespace WebHost.Components.Vocabularies;

//public partial class VocabularyDetailDialog : IDialogContentComponent<Guid>
//{
//    [Parameter] public Guid Content { get; set; } = default!;

//    [CascadingParameter] public FluentDialog Dialog { get; set; } = default!;

//    [Inject] protected IDialogService DialogService { get; private set; } = default!;
//    [Inject] protected INavigationService Navigation { get; private set; } = default!;
//    [Inject] protected IMediator Mediator { get; private set; } = default!;
//    [Inject] protected IMapper Mapper { get; private set; } = default!;
//    [Inject] protected IActionExecutor ActionExecutor { get; private set; } = default!;

//    private EditContext editContext = default!;

//    private VocabularyDetailViewModel detail = new();
//    private IReadOnlyList<LookupEntryDto> lookupEntryDtos = [];

//    private List<MediaFileDto> alternativeImageFiles = [];

//    private bool isRefreshing;
//    private bool isDeleting;
//    private bool isExporting;
//    private bool isRetryingExport;
//    private bool isRemovingExport;

//    private bool isSaving;
//    private bool isLookingUp;

//    protected override async Task OnInitializedAsync()
//    {
//        editContext = new EditContext(detail);

//        // await ActionExecutor.ExecuteAsync<VocabularyDto>(
//        //    () => Mediator.Send(new GetVocabularyQuery(Id)),
//        //    isLoading => state.IsRefreshing = isLoading,
//        //    result =>
//        //    {
//        //        detail = result.AudioFile;
//        //        audioFile = detail.AudioFile;
//        //        imageFile = detail.ImageFile;
//        //    }
//        //);
//    }

//    private void ClearForm()
//    {
//        detail = new();
//        lookupEntryDtos = [];
//        editContext = new EditContext(detail);
//    }

//    private async Task LookupAsync()
//    {
//        //if (string.IsNullOrWhiteSpace(detail.Word))
//        //{
//        //    ToastService.ShowWarning("Please enter a word first.");
//        //    return;
//        //}

//        // TODO: Use ExecuteAsync
//        //await ExecuteWithLoadingAsync(async () =>
//        //{
//        //    var result = await Mediator.Send(new CreateLookupEntriesCommand { Word = detail.Word });
//        //    if (result.IsFailed)
//        //    {
//        //        foreach (var error in result.Errors)
//        //            ToastService.ShowError(error.Message);
//        //        return;
//        //    }

//        //    lookupEntryDtos = await Mediator.Send(new GetLookupEntriesQuery { Ids = result.Value });
//        //    var defaultEntry = lookupEntryDtos[0];

//        //    detail.UpdateFromEntry(defaultEntry);

//        //    var mediaFileDtos = await Mediator.Send(new GetMediaFilesQuery { Ids = defaultEntry.MediaFileIds });
//        //    var audio = mediaFileDtos.FirstOrDefault(x => FileUtils.GetMediaType(x.ContentType) == MediaType.Audio);
//        //    if (audio is not null)
//        //        audioFile = audio;
//        //    var image = mediaFileDtos.FirstOrDefault(x => FileUtils.GetMediaType(x.ContentType) == MediaType.Image);
//        //    if (image is not null)
//        //        imageFile = image;
//        //    if (mediaFileDtos.Count > 1)
//        //        alternativeImageFiles = [.. mediaFileDtos
//        //            .Where(x => FileUtils.GetMediaType(x.ContentType) == MediaType.Image && x.Id != imageFile.Id)];

//        //    await InvokeAsync(StateHasChanged);
//        //},
//        //x => isLookingUp = x);
//    }

//    private void HandleChangeWordType()
//    {
//        //Model.Detail.WordType = wordType;

//        //if (_lookupResult is null)
//        //    return;

//        //var entry = _lookupResult.Entries.Find(e => e.WordType == Model.Detail.WordType);
//        //if (entry is null)
//        //    return;

//        //Model.Detail.Phonetic = entry.Phonetic;

//        //_availableDefinitions.Clear();
//        //_availableDefinitions.AddRange(entry.Definitions.Select(d => d.Definition));
//        //Model.Detail.Definition = _availableDefinitions.First();

//        //UpdateExamples();
//    }

//    private void HandleChangeDefinition()
//    {
//        //if (_lookupResult is null)
//        //    return;

//        //var entry = _lookupResult.Entries.Find(
//        //    e => e.Definitions.Any(d => d.Definition == Model.Detail.Definition));
//        //if (entry is null)
//        //    return;

//        //Model.Detail.WordType = entry.WordType;
//        //Model.Detail.Phonetic = entry.Phonetic;

//        //UpdateExamples();
//    }

//    private void HandleChangeExample()
//    {
//        //if (_lookupResult is null)
//        //    return;

//        //var entry = _lookupResult.Entries.Find(
//        //    e => e.Definitions.Any(d => d.Examples.Contains(Model.Detail.Example)));
//        //if (entry is null)
//        //    return;

//        //Model.Detail.WordType = entry.WordType;
//        //Model.Detail.Phonetic = entry.Phonetic;

//        //var definition = entry.Definitions.Find(d => d.Examples.Contains(Model.Detail.Example));
//        //if (definition is not null)
//        //{
//        //    Model.Detail.Definition = definition.Definition;
//        //}
//    }

//    //private void UpdateExamples()
//    //{
//    //    try
//    //    {
//    //        if (_lookupResult is null)
//    //            return;

//    //        var entry = _lookupResult.Entries.Find(
//    //            e => e.Definitions.Any(d => d.Definition == Model.Detail.Definition));
//    //        if (entry is null)
//    //            return;

//    //        _availableExamples.Clear();
//    //        _availableExamples.AddRange(
//    //            entry.Definitions
//    //            .Where(d => d.Definition == Model.Detail.Definition)
//    //            .SelectMany(d => d.Examples));
//    //        Model.Detail.Example = _availableExamples.First();
//    //    }
//    //    catch (Exception e)
//    //    {
//    //        ToastService.ShowError($"Error updating examples: {e.Message}");
//    //    }
//    //}

//    //protected async Task HandleFileUploadCompletedAsync(IEnumerable<FluentInputFileEventArgs> files, MediaFileInputModel fileModel)
//    //{
//    //    var file = files.First();
//    //    ////if (!IsValidFile(file, inputFile))
//    //    ////    return;

//    //    //var fileName = FileHelper.NormalizeFileName(Model.Detail.Word, Path.GetExtension(file.Name));
//    //    //var tempFilePath = FileHelper.GetFullTempUploadPath(fileName, Environment.WebRootPath);

//    //    var command = new UploadMediaFileCommand
//    //    {
//    //        Stream = file.Stream,
//    //        Filename = file.Name
//    //    };

//    //    var result = await Mediator.Send(command);
//    //    if (result.IsFailure)
//    //    {
//    //        ToastService.ShowWarning(result.ErrorMessages);
//    //        return;
//    //    }

//    //    fileModel.UpdateFrom(result.Data);

//    //    //inputFile.FilePath = FileHelper.GetRelativePath(tempFilePath, Environment.WebRootPath);
//    //    //inputFile.FileSource = FileSource.Local;
//    //    //ToastService.ShowSuccess($"File '{file.Name}' uploaded successfully.");
//    //}

//    protected async Task HandleFileUploadedAsync(FluentInputFileEventArgs uploadedFile)
//    {
//        //var file = files.First();
//        ////if (!IsValidFile(file, inputFile))
//        ////    return;

//        //var fileName = FileHelper.NormalizeFileName(Model.Detail.Word, Path.GetExtension(file.Name));
//        //var tempFilePath = FileHelper.GetFullTempUploadPath(fileName, Environment.WebRootPath);

//        //var command = new UploadMediaFileCommand
//        //{
//        //    Stream = uploadedFile.Stream!,
//        //    Filename = uploadedFile.Name
//        //};

//        //var result = await Mediator.Send(command);
//        //if (result.IsFailed)
//        //{
//        //    ToastService.ShowWarning(result.GetErrorMessages());
//        //    return;
//        //}

//        ////fileModel.UpdateFrom(result.Data);
//        //if (FileUtils.GetMediaType(result.Value.ContentType) == MediaType.Audio)
//        //    audioFile = result.Value;
//        //else if (FileUtils.GetMediaType(result.Value.ContentType) == MediaType.Image)
//        //    imageFile = result.Value;


//        //inputFile.FilePath = FileHelper.GetRelativePath(tempFilePath, Environment.WebRootPath);
//        //inputFile.FileSource = FileSource.Local;
//        //ToastService.ShowSuccess($"File '{file.Name}' uploaded successfully.");
//    }

//    protected void HandleRemoveFile(MediaType type)
//    {
//        //Model.Detail.RemoveFile(fileType);
//        //if (fileType == MediaType.Image)
//        //    _availableImages.Clear();
//        switch (type)
//        {
//            case MediaType.Audio:
//                detail.AudioFile = new();
//                break;
//            case MediaType.Image:
//                detail.ImageFile = new();
//                break;
//            default:
//                break;
//        }
//    }

//    private async Task SaveAsync()
//    {
//        if (!editContext.Validate() || isSaving)
//            return;

//        await ActionExecutor.ExecuteAsync(
//            () => Mediator.Send(new CreateVocabularyCommand(
//                detail.Word,
//                detail.PartOfSpeech,
//                detail.Pronunciation,
//                detail.Cloze,
//                detail.Definition,
//                detail.Example,
//                detail.Meaning,
//                detail.AudioFile.Id,
//                detail.ImageFile.Id)),
//            isLoading => isSaving = isLoading
//        );

//        //await Dialog.CloseAsync();
//    }

//    private async Task CancelAsync() => await Dialog.CancelAsync();
//}