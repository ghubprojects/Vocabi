using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Vocabi.Web.Services.Navigation;

namespace Vocabi.Web.Components.Pages;

public abstract class BasePage : ComponentBase
{
    [Inject] protected IDialogService DialogService { get; set; } = default!;
    [Inject] protected IToastService ToastService { get; set; } = default!;
    [Inject] protected INavigationService Navigation { get; set; } = default!;
    [Inject] protected IMediator Mediator { get; set; } = default!;

    protected bool IsLoading { get; set; } = false;

    protected async Task ExecuteWithLoadingAsync(Func<Task> action)
    {
        try
        {
            IsLoading = true;
            StateHasChanged();

            await action();
        }
        catch (Exception e)
        {
            ToastService.ShowError(e.Message);
        }
        finally
        {
            IsLoading = false;
            StateHasChanged();
        }
    }
}
