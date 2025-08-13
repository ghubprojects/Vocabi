namespace Vocabi.Application.Common.Models;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IReadOnlyList<string> Errors { get; }
    public string ErrorMessages => string.Join(", ", Errors);

    protected Result(bool isSuccess, IEnumerable<string> errors)
    {
        if (isSuccess && errors.Any())
            throw new InvalidOperationException("Successful result cannot have errors.");
        if (!isSuccess && !errors.Any())
            throw new InvalidOperationException("Failure result must contain at least one error.");

        IsSuccess = isSuccess;
        Errors = [.. errors];
    }

    public static Result Success() => new(true, []);

    public static Result Failure(params string[] errors) => new(false, errors);
}

public class Result<T> : Result
{
    public T Data { get; }

    private Result(T data, bool isSuccess, IEnumerable<string> errors) : base(isSuccess, errors)
    {
        Data = data;
    }

    public static Result<T> Success(T data) => new(data, true, []);

    public new static Result<T> Failure(params string[] errors) => new(default!, false, errors);
}