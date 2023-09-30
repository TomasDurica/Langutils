using Langutils.Core.Options;

namespace Langutils.Core.Results;

public static partial class ResultExtensions
{
    public static Result<TOut, TError> Map<TIn, TOut, TError>(this Result<TIn, TError> self, Func<TIn, TOut> selector) => self switch
    {
        { IsSuccess: true, Value: var value } => selector(value),
        { Error: var error } => error!
    };

    public static async Task<Result<TOut, TError>> Map<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TIn, TOut> selector)
        => (await self).Map(selector);

    public static async Task<Result<TOut, TError>> MapAsync<TIn, TOut, TError>(this Result<TIn, TError> self, Func<TIn, Task<TOut>> selector)
        => self switch
        {
            { IsSuccess: true, Value: var value } => await selector(value).ConfigureAwait(false),
            { Error: var error } => error!
        };

    public static async Task<Result<TOut, TError>> MapAsync<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TIn, Task<TOut>> selector)
        => await (await self).MapAsync(selector).ConfigureAwait(false);

    public static Result<TValue, TOut> MapError<TValue, TIn, TOut>(this Result<TValue, TIn> self, Func<TIn?, TOut> selector) => self switch
    {
        { IsError: true, Error: var error } => selector(error),
        { Value: var value } => value
    };

    public static async Task<Result<TValue, TOut>> MapError<TValue, TIn, TOut>(this Task<Result<TValue, TIn>> self, Func<TIn?, TOut> selector)
        => (await self).MapError(selector);

    public static async Task<Result<TValue, TOut>> MapErrorAsync<TValue, TIn, TOut>(this Result<TValue, TIn> self, Func<TIn?, Task<TOut>> selector)
        => self switch
        {
            { IsError: true, Error: var error } => await selector(error).ConfigureAwait(false),
            { Value: var value } => value
        };

    public static async Task<Result<TValue, TOut>> MapErrorAsync<TValue, TIn, TOut>(this Task<Result<TValue, TIn>> self, Func<TIn?, Task<TOut>> selector)
        => await (await self).MapErrorAsync(selector).ConfigureAwait(false);

    public static TOut MapOr<TIn, TOut, TError>(this Result<TIn, TError> self, TOut defaultValue, Func<TIn, TOut> selector) => self switch
    {
        { IsSuccess: true, Value: var value } => selector(value),
        _ => defaultValue
    };

    public static async Task<TOut> MapOr<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, TOut defaultValue, Func<TIn, TOut> selector)
        => (await self).MapOr(defaultValue, selector);

    public static async Task<TOut> MapOrAsync<TIn, TOut, TError>(this Result<TIn, TError> self, TOut defaultValue, Func<TIn, Task<TOut>> selector)
        => self switch
        {
            { IsSuccess: true, Value: var value } => await selector(value).ConfigureAwait(false),
            _ => defaultValue
        };

    public static async Task<TOut> MapOrAsync<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, TOut defaultValue, Func<TIn, Task<TOut>> selector)
        => await (await self).MapOrAsync(defaultValue, selector).ConfigureAwait(false);

    public static TOut MapOrElse<TIn, TOut, TError>(this Result<TIn, TError> self, Func<TError?, TOut> defaultValueProvider, Func<TIn, TOut> selector) => self switch
    {
        { IsSuccess: true, Value: var value } => selector(value),
        { Error: var error } _ => defaultValueProvider(error)
    };

    public static async Task<TOut> MapOrElse<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TError?, TOut> defaultValueProvider, Func<TIn, TOut> selector)
        => (await self).MapOrElse(defaultValueProvider, selector);

    public static async Task<TOut> MapOrElseAsync<TIn, TOut, TError>(this Result<TIn, TError> self, Func<TError?, TOut> defaultValueProvider, Func<TIn, Task<TOut>> selector)
        => self switch
        {
            { IsSuccess: true, Value: var value } => await selector(value).ConfigureAwait(false),
            { Error: var error } => defaultValueProvider(error)
        };

    public static async Task<TOut> MapOrElseAsync<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TError?, TOut> defaultValueProvider, Func<TIn, Task<TOut>> selector)
        => await (await self).MapOrElseAsync(defaultValueProvider, selector).ConfigureAwait(false);

    public static async Task<TOut> MapOrElseAsync<TIn, TOut, TError>(this Result<TIn, TError> self, Func<TError?, Task<TOut>> defaultValueProvider, Func<TIn, TOut> selector)
        => self switch
        {
            { IsSuccess: true, Value: var value } => selector(value),
            { Error: var error } => await defaultValueProvider(error).ConfigureAwait(false)
        };

    public static async Task<TOut> MapOrElseAsync<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TError?, Task<TOut>> defaultValueProvider, Func<TIn, TOut> selector)
        => await (await self).MapOrElseAsync(defaultValueProvider, selector).ConfigureAwait(false);

    public static async Task<TOut> MapOrElseAsync<TIn, TOut, TError>(this Result<TIn, TError> self, Func<TError?, Task<TOut>> defaultValueProvider, Func<TIn, Task<TOut>> selector)
        => self switch
        {
            { IsSuccess: true, Value: var value } => await selector(value).ConfigureAwait(false),
            { Error: var error } => await defaultValueProvider(error).ConfigureAwait(false)
        };
    
    public static async Task<TOut> MapOrElseAsync<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TError?, Task<TOut>> defaultValueProvider, Func<TIn, Task<TOut>> selector)
        => await (await self).MapOrElseAsync(defaultValueProvider, selector).ConfigureAwait(false);

    public static Option<TValue> Success<TValue, TError>(this Result<TValue, TError> self) => self switch
    {
        { IsSuccess: true, Value: var value } => value,
        _ => None.Instance
    };

    public static async Task<Option<TValue>> Success<TValue, TError>(this Task<Result<TValue, TError>> self)
        => (await self).Success();

    public static Option<TError> Error<TValue, TError>(this Result<TValue, TError> self) => self switch
    {
        { IsError: true, Error: var error } => error,
        _ => None.Instance
    };

    public static async Task<Option<TError>> Error<TValue, TError>(this Task<Result<TValue, TError>> self)
        => (await self).Error();

    public static Option<Result<TValue, TError>> Transpose<TValue, TError>(this Result<Option<TValue>, TError> self) => self switch
    {
        { IsSuccess: true, Value.IsSome: false } => None.Instance,
        { IsSuccess: true, Value: { IsSome: true, Value: var value }} => Result.Success<TValue, TError>(value),
        { Error: var error } => Result.Error<TValue, TError>(error!)
    };

    public static async Task<Option<Result<TValue, TError>>> Transpose<TValue, TError>(this Task<Result<Option<TValue>, TError>> self)
        => (await self).Transpose();

    public static Result<TValue, TError> Flatten<TValue, TError>(this Result<Result<TValue, TError>, TError> self) => self switch
    {
        { IsSuccess: true, Value: var option} => option,
        { Error: var error } => error!
    };

    public static async Task<Result<TValue, TError>> Flatten<TValue, TError>(this Task<Result<Result<TValue, TError>, TError>> self)
        => (await self).Flatten();
}