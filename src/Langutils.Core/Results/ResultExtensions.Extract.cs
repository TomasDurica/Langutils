using System.Diagnostics.CodeAnalysis;

namespace Langutils.Core.Results;

public static partial class ResultExtensions
{
    public static TValue Expect<TValue, TError>(this Result<TValue, TError> self, string message)
        => self switch
        {
            { IsSuccess: true, Value: var value } => value,
            _ => throw new InvalidOperationException(message)
        };

    public static async Task<TValue> Expect<TValue, TError>(this Task<Result<TValue, TError>> self, string message)
        => (await self).Expect(message);

    public static bool TryUnwrap<TValue, TError>(this Result<TValue, TError> self, [NotNullWhen(true)] out TValue? value)
    {
        if (self is { IsSuccess: true, Value: var innerValue })
        {
            value = innerValue;
            return true;
        }

        value = default;
        return false;
    }

    public static TValue Unwrap<TValue, TError>(this Result<TValue, TError> self)
        => self switch
        {
            { IsSuccess: true, Value: var value } => value,
            { Error: var error } => throw new InvalidOperationException($"Called `{nameof(Unwrap)}()` on a `{nameof(Result)}.{nameof(Result.Error)}` value: {error}")
        };

    public static async Task<TValue> Unwrap<TValue, TError>(this Task<Result<TValue, TError>> self)
        => (await self).Unwrap();

    public static TValue UnwrapOr<TValue, TError>(this Result<TValue, TError> self, TValue defaultValue)
        => self switch
        {
            { IsSuccess: true, Value: var value } => value,
            _ => defaultValue
        };

    public static async Task<TValue> UnwrapOr<TValue, TError>(this Task<Result<TValue, TError>> self, TValue defaultValue)
        => (await self).UnwrapOr(defaultValue);

    public static TValue? UnwrapOrDefault<TValue, TError>(this Result<TValue, TError> self)
        => self switch
        {
            { IsSuccess: true, Value: var value } => value,
            _ => default
        };

    public static async Task<TValue?> UnwrapOrDefault<TValue, TError>(this Task<Result<TValue, TError>> self)
        => (await self).UnwrapOrDefault();

    public static TValue UnwrapOrElse<TValue, TError>(this Result<TValue, TError> self, Func<TError?, TValue> defaultValueProvider)
        => self switch
        {
            { IsSuccess: true, Value: var value } => value,
            { Error: var error } => defaultValueProvider(error)
        };

    public static async Task<TValue> UnwrapOrElse<TValue, TError>(this Task<Result<TValue, TError>> self, Func<TError?, TValue> defaultValueProvider)
        => (await self).UnwrapOrElse(defaultValueProvider);

    public static async Task<TValue> UnwrapOrElseAsync<TValue, TError>(this Result<TValue, TError> self, Func<TError?, Task<TValue>> defaultValueProvider)
        => self switch
        {
            { IsSuccess: true, Value: var value } => value,
            { Error: var error } => await defaultValueProvider(error).ConfigureAwait(false)
        };

    public static async Task<TValue> UnwrapOrElseAsync<TValue, TError>(this Task<Result<TValue, TError>> self, Func<TError?, Task<TValue>> defaultValueProvider)
        => await (await self).UnwrapOrElseAsync(defaultValueProvider).ConfigureAwait(false);

    public static TError? ExpectError<TValue, TError>(this Result<TValue, TError> self, string message)
        => self switch
        {
            { IsError: true, Error: var error } => error,
            _ => throw new InvalidOperationException(message)
        };

    public static async Task<TError?> ExpectError<TValue, TError>(this Task<Result<TValue, TError>> self, string message)
        => (await self).ExpectError(message);

    public static bool TryUnwrapError<TValue, TError>(this Result<TValue, TError> self, out TError? error)
    {
        if (self is { IsError: true, Error: var innerError })
        {
            error = innerError;
            return true;
        }

        error = default;
        return false;
    }

    public static TError? UnwrapError<TValue, TError>(this Result<TValue, TError> self)
        => self switch
        {
            { IsError: true, Error: var error } => error,
            { Value: var value } => throw new InvalidOperationException($"Called `{nameof(UnwrapError)}()` on a `{nameof(Result)}.{nameof(Result.Success)}` value: {value}")
        };

    public static async Task<TError?> UnwrapError<TValue, TError>(this Task<Result<TValue, TError>> self)
        => (await self).UnwrapError();
}