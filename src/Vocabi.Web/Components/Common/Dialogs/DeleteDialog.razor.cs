using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Vocabi.Web.Common.Helpers;

namespace Vocabi.Web.Components.Common.Dialogs;

public partial class DeleteDialog : IDialogContentComponent<string>
{
    [Parameter] public string Content { get; set; } = default!;

    [CascadingParameter] public FluentDialog Dialog { get; set; } = default!;

    [Inject] public IActionExecutor ActionExecutor { get; set; } = default!;

    private async Task DeleteAsync() => await Dialog.CloseAsync(Content);

    private async Task CancelAsync() => await Dialog.CancelAsync();
}
