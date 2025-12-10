using FluentResults;

namespace Vocabi.Shared.Extensions;

public static class ResultExtensions
{
    public static string GetErrorMessages(this Result result, string separator = "\n")
        => result.IsSuccess
            ? string.Empty
            : string.Join(separator, result.Errors.Select(error => error.Message));

    public static string GetErrorMessages<T>(this Result<T> result, string separator = "\n")
        => result.IsSuccess
            ? string.Empty
            : string.Join(separator, result.Errors.Select(error => error.Message));
}
