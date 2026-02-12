using FluentResults;

namespace WebHost.Common.Helpers;

public interface IActionExecutor
{
    Task ExecuteAsync(
        Func<Task<Result>> action,
        Action<bool>? setLoading = null,
        Func<Task>? onSuccess = null);

    Task<T?> ExecuteAsync<T>(
        Func<Task<Result<T>>> action,
        Action<bool>? setLoading = null,
        Func<T, Task>? onSuccess = null);
}
