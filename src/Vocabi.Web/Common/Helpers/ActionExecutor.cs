using FluentResults;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Vocabi.Web.Common.Helpers;

public class ActionExecutor(IToastService toastService) : IActionExecutor
{
    public async Task ExecuteAsync(
        Func<Task<Result>> action,
        Action<bool>? setLoading = null,
        Func<Task>? onSuccess = null)
    {
        setLoading?.Invoke(true);
        try
        {
            var result = await action();

            if (result.IsFailed)
            {
                foreach (var e in result.Errors)
                    toastService.ShowError(e.Message);
                return;
            }

            if (result.Successes.Count > 0)
                toastService.ShowSuccess(result.Successes[0].Message);

            if (onSuccess != null)
                await onSuccess();
        }
        finally
        {
            setLoading?.Invoke(false);
        }
    }

    public async Task<T?> ExecuteAsync<T>(
        Func<Task<Result<T>>> action,
        Action<bool>? setLoading = null,
        Func<T, Task>? onSuccess = null)
    {
        setLoading?.Invoke(true);
        try
        {
            var result = await action();

            if (result.IsFailed)
            {
                foreach (var e in result.Errors)
                    toastService.ShowError(e.Message);
                return default;
            }

            if (result.Successes.Count > 0)
                toastService.ShowSuccess(result.Successes[0].Message);

            if (onSuccess != null && result.Value != null)
                await onSuccess(result.Value);

            return result.Value;
        }
        finally
        {
            setLoading?.Invoke(false);
        }
    }
}