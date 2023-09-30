using System.Diagnostics.CodeAnalysis;

namespace Langutils.Core.Results;

/// <summary>
/// This type is used to create results.
/// </summary>
public static class Result
{
    /// <summary>
    /// Returns a success result with a value.
    /// </summary>
    /// <param name="value">The underlying value</param>
    /// <typeparam name="TValue">The Value type</typeparam>
    /// <typeparam name="TError">The Error type</typeparam>
    /// <returns></returns>
    public static Result<TValue, TError> Success<TValue, TError>(TValue value)
        => new(value);

    /// <summary>
    /// Returns a success result with a value.
    /// </summary>
    /// <param name="value">The underlying value</param>
    /// <typeparam name="TValue">The Value type</typeparam>
    /// <returns></returns>
    public static Success<TValue> Success<TValue>(TValue value)
        => new(value);

    /// <summary>
    /// Returns an error result with an error.
    /// </summary>
    /// <param name="error">The underlying error</param>
    /// <typeparam name="TValue">The Value type</typeparam>
    /// <typeparam name="TError">The Error type</typeparam>
    /// <returns></returns>
    public static Result<TValue, TError> Error<TValue, TError>(TError error)
        => new(error);

    /// <summary>
    /// Returns an error result with an error.
    /// </summary>
    /// <param name="error">The underlying error</param>
    /// <typeparam name="TError">The Error type</typeparam>
    /// <returns></returns>
    public static Error<TError> Error<TError>(TError error)
        => new(error);
}

/// <summary>
/// This type is used to represent a success in ambiguous cases - e.g. when you want to return `Result&gt;T, T&lt;`.
/// </summary>
/// <param name="Value">The underlying Value</param>
/// <typeparam name="TValue">The Value type</typeparam>
public readonly record struct Success<TValue>(TValue Value)
{
    public Result<TValue, TError> ToResult<TError>() => new(Value);
};

/// <summary>
/// This type is used to represent an error in ambiguous cases - e.g. when you want to return `Result&gt;T, T&lt`;.
/// </summary>
/// <param name="ErrorValue">The underlying error</param>
/// <typeparam name="TError">The Error type</typeparam>
public readonly record struct Error<TError>(TError ErrorValue)
{
    public Result<TValue, TError> ToResult<TValue>() => new(ErrorValue);
}

/// <summary>
/// A type that represents either a value or an error. It is similar to `Either` or `Result` in functional languages.
/// </summary>
/// <typeparam name="TValue">The Value type</typeparam>
/// <typeparam name="TError">The Error type</typeparam>
public readonly record struct Result<TValue, TError>
{
    /// <summary>
    /// Indicates whether the result is a success.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsSuccess { get; }

    /// <summary>
    /// Indicates whether the result is an error.
    /// </summary>
    /// <returns>
    /// `true` when &lt;see cref="IsSuccess"/&gt; is false.
    /// </returns>
    [MemberNotNullWhen(false, nameof(Value))]
    public bool IsError
        => !IsSuccess;

    /// <summary>
    /// The value of the result.
    /// It is not null when <see cref="IsSuccess"/> is true.
    /// </summary>
    public TValue? Value { get; }

    /// <summary>
    /// The error of the result.
    /// It always can be null, as results created with default or array constructor will not have an error, but still not be success.
    /// </summary>
    public TError? Error { get; }

    /// <summary>
    /// Creates a new result with a value.
    /// </summary>
    public Result(TValue value)
    {
        IsSuccess = true;
        Value = value;
    }

    /// <summary>
    /// Creates a new result with an error.
    /// </summary>
    public Result(TError value)
    {
        IsSuccess = false;
        Error = value;
    }

    /// <summary>
    /// Creates a new result with a value.
    /// </summary>
    public static implicit operator Result<TValue, TError>(TValue value)
        => new(value);

    /// <summary>
    /// Creates a new result with an error.
    /// </summary>
    public static implicit operator Result<TValue, TError>(TError value)
        => new(value);

    /// <summary>
    /// Creates a new result with a value.
    /// </summary>
    public static implicit operator Result<TValue, TError>(Success<TValue> success)
        => success.ToResult<TError>();

    /// <summary>
    /// Creates a new result with an error.
    /// </summary>
    public static implicit operator Result<TValue, TError>(Error<TError> error)
        => error.ToResult<TValue>();

    /// <returns>
    /// "Result.Success({Value})" when <see cref="IsSuccess"/> is true.
    /// "Result.Error({Error})" when <see cref="IsSuccess"/> is false.
    /// </returns>
    public override string ToString()
        => IsSuccess
            ? $"Result.Success({Value})"
            : $"Result.Error({Error})";
}