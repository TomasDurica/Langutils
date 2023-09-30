using System.Diagnostics.CodeAnalysis;

namespace Langutils.Core.Results;

public static class Result
{
    public static Result<TValue, TError> Success<TValue, TError>(TValue value)
        => new(value);

    public static Success<TValue> Success<TValue>(TValue value)
        => new(value);

    public static Result<TValue, TError> Error<TValue, TError>(TError error)
        => new(error);

    public static Error<TError> Error<TError>(TError error)
        => new(error);
}

public readonly record struct Success<TValue>(TValue Value)
{
    public Result<TValue, TError> ToResult<TError>() => new(Value);
};

public readonly record struct Error<TError>(TError ErrorValue)
{
    public Result<TValue, TError> ToResult<TValue>() => new(ErrorValue);
}

public readonly record struct Result<TValue, TError>
{
    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsSuccess { get; }

    [MemberNotNullWhen(false, nameof(Value))]
    public bool IsError
        => !IsSuccess;

    public TValue? Value { get; }

    public TError? Error { get; }

    public Result(TValue value)
    {
        IsSuccess = true;
        Value = value;
    }

    public Result(TError value)
    {
        IsSuccess = false;
        Error = value;
    }

    public static implicit operator Result<TValue, TError>(TValue value)
        => new(value);

    public static implicit operator Result<TValue, TError>(TError value)
        => new(value);

    public static implicit operator Result<TValue, TError>(Success<TValue> success)
        => success.ToResult<TError>();

    public static implicit operator Result<TValue, TError>(Error<TError> error)
        => error.ToResult<TValue>();

    public override string ToString()
        => IsSuccess
            ? $"Result.Success({Value})"
            : $"Result.Error({Error})";
}