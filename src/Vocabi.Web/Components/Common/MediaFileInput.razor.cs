using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Vocabi.Application.Features.MediaFiles.GetMediaFiles;
using static Vocabi.Shared.Common.Enums;

namespace Vocabi.Web.Components.Common;

public partial class MediaFileInput
{
    [Parameter] public MediaType FileType { get; set; } = MediaType.Unknown;
    [Parameter] public int MaxFileSizeInKB { get; set; } = 1024;
    [Parameter] public string Accept { get; set; } = "*/*";
    [Parameter] public string AnchorId { get; set; } = "file-input";
    [Parameter] public string Placeholder { get; set; } = string.Empty;
    [Parameter] public Icon Icon { get; set; } = default!;

    [Parameter] public MediaFileDto UploadedFile { get; set; } = default!;
    [Parameter] public List<MediaFileDto> AlternativeFiles { get; set; } = [];
    [Parameter] public EventCallback<FluentInputFileEventArgs> OnFileUploaded { get; set; }
    [Parameter] public EventCallback<MediaType> OnRemoveFile { get; set; }

    private readonly List<MediaFileDto> _files = [];
    private int fileIndex = 0;

    private async Task HandleFileUploaded(IEnumerable<FluentInputFileEventArgs> files)
    {
        var file = files.First();
        // TODO: validate...
        await OnFileUploaded.InvokeAsync(file);
    }

    private async Task HandleRemoveFile(MediaType type)
    {
        await OnRemoveFile.InvokeAsync(type);
        _files.Clear();
    }

    private void HandleChangeFile()
    {
        if (_files.Count == 0)
        {
            _files.Add(UploadedFile);
            _files.AddRange(AlternativeFiles);
        }

        UploadedFile = _files.ElementAt(fileIndex);
        fileIndex = (fileIndex + 1) % _files.Count;
    }
}
