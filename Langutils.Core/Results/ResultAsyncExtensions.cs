namespace Langutils.Core.Results;

public static class ResultAsyncExtensions
{
    public static async Task<bool> IsSuccessAndAsync<TValue, TError>(this Result<TValue, TError> self, Func<TValue, Task<bool>> predicate)
        => self switch
        {
            { IsSuccess: true, Value: var value } => await predicate(value).ConfigureAwait(false),
            _ => false
        };

    public static async Task<bool> IsErrorAndAsync<TValue, TError>(this Result<TValue, TError> self, Func<TError?, Task<bool>> predicate)
        => self switch
        {
            { IsError: true, Error: var error } => await predicate(error).ConfigureAwait(false),
            _ => false
        };

    public static async Task<TValue> UnwrapOrElseAsync<TValue, TError>(this Result<TValue, TError> self, Func<TError?, Task<TValue>> defaultValueProvider)
        => self switch
        {
            { IsSuccess: true, Value: var value } => value,
            { Error: var error } => await defaultValueProvider(error).ConfigureAwait(false)
        };

    public static async Task<Result<TValue, TError>> TapAsync<TValue, TError>(this Result<TValue, TError> self, Func<TValue, Task> onSuccess)
    {
        if (self is { IsSuccess: true, Value: var value })
        {
            await onSuccess(value).ConfigureAwait(false);
        }

        return self;
    }

    public static async Task<Result<TValue, TError>> TapErrorAsync<TValue, TError>(this Result<TValue, TError> self, Func<TError?, Task> onError)
    {
        if (self is { IsError: true, Error: var error })
        {
            await onError(error).ConfigureAwait(false);
        }

        return self;
    }

    public static async Task<Result<TOut, TError>> MapAsync<TIn, TOut, TError>(this Result<TIn, TError> self, Func<TIn, Task<TOut>> selector)
        => self switch
        {
            { IsSuccess: true, Value: var value } => await selector(value).ConfigureAwait(false),
            { Error: var error } => error!
        };

    public static async Task<Result<TValue, TOut>> MapErrorAsync<TValue, TIn, TOut>(this Result<TValue, TIn> self, Func<TIn?, Task<TOut>> selector)
        => self switch
        {
            { IsError: true, Error: var error } => await selector(error).ConfigureAwait(false),
            { Value: var value } => value
        };

    public static async Task<TOut> MapOrAsync<TIn, TOut, TError>(this Result<TIn, TError> self, TOut defaultValue, Func<TIn, Task<TOut>> selector)
        => self switch
        {
            { IsSuccess: true, Value: var value } => await selector(value).ConfigureAwait(false),
            _ => defaultValue
        };

    public static async Task<TOut> MapOrElseAsync<TIn, TOut, TError>(this Result<TIn, TError> self, Func<TOut> defaultValueProvider, Func<TIn, Task<TOut>> selector)
        => self switch
        {
            { IsSuccess: true, Value: var value } => await selector(value).ConfigureAwait(false),
            _ => defaultValueProvider()
        };

    public static async Task<TOut> MapOrElseAsync<TIn, TOut, TError>(this Result<TIn, TError> self, Func<Task<TOut>> defaultValueProvider, Func<TIn, TOut> selector)
        => self switch
        {
            { IsSuccess: true, Value: var value } => selector(value),
            _ => await defaultValueProvider().ConfigureAwait(false)
        };

    public static async Task<TOut> MapOrElseAsync<TIn, TOut, TError>(this Result<TIn, TError> self, Func<Task<TOut>> defaultValueProvider, Func<TIn, Task<TOut>> selector)
        => self switch
        {
            { IsSuccess: true, Value: var value } => await selector(value).ConfigureAwait(false),
            _ => await defaultValueProvider().ConfigureAwait(false)
        };

    public static async Task<Result<TOut, TError>> AndThenAsync<TIn, TOut, TError>(this Result<TIn, TError> self, Func<TIn, Task<Result<TOut, TError>>> optionProvider)
        => self switch
        {
            { IsSuccess: true, Value: var value} => await optionProvider(value).ConfigureAwait(false),
            { Error: var error } => error!
        };

    public static async Task<Result<TValue, TOut>> OrElseAsync<TValue, TIn, TOut>(this Result<TValue, TIn> self, Func<TIn?, Task<Result<TValue, TOut>>> optionProvider)
        => self switch
        {
            { IsSuccess: true, Value: var value } => value,
            { Error: var error } => await optionProvider(error).ConfigureAwait(false)
        };
}