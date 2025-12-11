using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Vocabi.Web.Services.Navigation;

namespace Vocabi.Web.Pages;

public abstract class BasePage : ComponentBase
{
    [Inject] protected IDialogService DialogService { get; private set; } = default!;
    [Inject] protected IToastService ToastService { get; private set; } = default!;
    [Inject] protected INavigationService Navigation { get; private set; } = default!;
    [Inject] protected IMediator Mediator { get; private set; } = default!;
    [Inject] protected IMapper Mapper { get; private set; } = default!;
    [Inject] protected ILogger<BasePage> Logger { get; private set; } = default!;

    protected async Task ExecuteWithLoadingAsync(Func<Task> action, Action<bool> setLoading)
    {
        ArgumentNullException.ThrowIfNull(action);
        ArgumentNullException.ThrowIfNull(setLoading);

        setLoading(true);
        try
        {
            await action();
        }
        finally
        {
            setLoading(false);
            StateHasChanged();
        }
    }
}