namespace Vocabi.Application.Common.Models;

public class Result
{
    #region Properties

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IReadOnlyList<string> Errors { get; }
    public string ErrorMessages => string.Join(", ", Errors);

    #endregion

    #region Constructors

    protected Result(bool isSuccess, IEnumerable<string> errors)
    {
        if (isSuccess && errors.Any())
            throw new InvalidOperationException("Successful result cannot have errors.");
        if (!isSuccess && !errors.Any())
            throw new InvalidOperationException("Failure result must contain at least one error.");

        IsSuccess = isSuccess;
        Errors = [.. errors];
    }

    #endregion

    #region Factory Methods

    public static Result Success()
        => new(true, []);

    public static Task<Result> SuccessAsync()
        => Task.FromResult(Success());

    public static Result Failure(params IEnumerable<string> errors)
        => new(false, errors);

    public static Task<Result> FailureAsync(params IEnumerable<string> errors)
        => Task.FromResult(Failure(errors));

    #endregion
}

public class Result<T> : Result
{
    #region Properties

    public T Data { get; }

    #endregion

    #region Constructors             

    private Result(T data, bool isSuccess, IEnumerable<string> errors) : base(isSuccess, errors)
    {
        Data = data;
    }

    #endregion

    #region Factory Methods

    public static Result<T> Success(T data)
        => new(data, true, []);

    public static Task<Result<T>> SuccessAsync(T data)
        => Task.FromResult(Success(data));

    public new static Result<T> Failure(params IEnumerable<string> errors)
        => new(default!, false, errors);

    public static new Task<Result<T>> FailureAsync(params IEnumerable<string> errors)
        => Task.FromResult(Failure(errors));

    #endregion

    #region Functional Methods

    public Result<U> Bind<U>(Func<T, Result<U>> next)
        => IsFailure ? Result<U>.Failure(Errors) : next(Data);

    public async Task<Result<U>> BindAsync<U>(Func<T, Task<Result<U>>> next)
        => IsFailure ? await Result<U>.FailureAsync(Errors) : await next(Data);

    #endregion
}