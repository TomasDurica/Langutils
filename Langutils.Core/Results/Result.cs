using System;
using System.Diagnostics.CodeAnalysis;

namespace Langutils.Core.Results;

public static class Result
{
    public static Result<TValue, TError> Success<TValue, TError>(TValue value)
        => new(value);

    public static Result<TValue, string> Success<TValue>(TValue value)
        => new(value);

    public static Result<TValue, TError> Error<TValue, TError>(TError error)
        => new(error);

    public static Result<TValue, string> Error<TValue>(string error)
        => new(error);

    public static Result<TValue, Exception> Error<TValue>(Exception error)
        => new(error);
}

public readonly record struct Result<TValue, TError>
{
    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsSuccess { get; }

    [MemberNotNullWhen(false, nameof(Value))]
    public bool IsError => !IsSuccess;

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
}