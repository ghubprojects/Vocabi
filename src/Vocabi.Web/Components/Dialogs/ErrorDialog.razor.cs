using Microsoft.AspNetCore.Components;
using System.Net;
using Vocabi.Domain.Exceptions;

namespace Vocabi.Web.Components.Dialogs;

public partial class ErrorDialog
{
    [Inject] protected NavigationManager Navigation { get; private set; } = default!;
    [Inject] protected ILogger<ErrorDialog> Logger { get; private set; } = default!;

    [EditorRequired][Parameter] public Exception Exception { get; set; } = default!;

    private string? Message { get; set; }

    protected override void OnInitialized()
    {
        switch (Exception)
        {
            case DomainException ex:
                Message = ex.Message;

                break;
            default:
                if (Exception.InnerException != null)
                    while (Exception.InnerException != null)
                        Exception = Exception.InnerException;

                Message = Exception.Message;
                break;
        }

        Logger.LogError(Exception, "{Message}. request url: {@url} {@UserName}", Message, Navigation.Uri, "Hihi");
    }

    private void HandleRefresh()
        => Navigation.NavigateTo(Navigation.Uri, true);
}
