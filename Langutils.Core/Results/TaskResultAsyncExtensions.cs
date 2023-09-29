namespace Langutils.Core.Results;

public static class TaskResultAsyncExtensions
{
    public static async Task<bool> IsSuccessAndAsync<TValue, TError>(this Task<Result<TValue, TError>> self, Func<TValue, Task<bool>> predicate)
        => await (await self).IsSuccessAndAsync(predicate).ConfigureAwait(false);

    public static async Task<bool> IsErrorAndAsync<TValue, TError>(this Task<Result<TValue, TError>> self, Func<TError?, Task<bool>> predicate)
        => await (await self).IsErrorAndAsync(predicate).ConfigureAwait(false);

    public static async Task<TValue> UnwrapOrElseAsync<TValue, TError>(this Task<Result<TValue, TError>> self, Func<TError?, Task<TValue>> defaultValueProvider)
        => await (await self).UnwrapOrElseAsync(defaultValueProvider).ConfigureAwait(false);

    public static async Task<Result<TValue, TError>> TapAsync<TValue, TError>(this Task<Result<TValue, TError>> self, Func<TValue, Task> onSuccess)
        => await (await self).TapAsync(onSuccess).ConfigureAwait(false);

    public static async Task<Result<TValue, TError>> TapErrorAsync<TValue, TError>(this Task<Result<TValue, TError>> self, Func<TError?, Task> onError)
        => await (await self).TapErrorAsync(onError).ConfigureAwait(false);

    public static async Task<Result<TOut, TError>> MapAsync<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TIn, Task<TOut>> selector)
        => await (await self).MapAsync(selector).ConfigureAwait(false);

    public static async Task<Result<TValue, TOut>> MapErrorAsync<TValue, TIn, TOut>(this Task<Result<TValue, TIn>> self, Func<TIn?, Task<TOut>> selector)
        => await (await self).MapErrorAsync(selector).ConfigureAwait(false);

    public static async Task<TOut> MapOrAsync<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, TOut defaultValue, Func<TIn, Task<TOut>> selector)
        => await (await self).MapOrAsync(defaultValue, selector).ConfigureAwait(false);

    public static async Task<TOut> MapOrElseAsync<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TError?, TOut> defaultValueProvider, Func<TIn, Task<TOut>> selector)
        => await (await self).MapOrElseAsync(defaultValueProvider, selector).ConfigureAwait(false);

    public static async Task<TOut> MapOrElseAsync<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TError?, Task<TOut>> defaultValueProvider, Func<TIn, TOut> selector)
        => await (await self).MapOrElseAsync(defaultValueProvider, selector).ConfigureAwait(false);

    public static async Task<TOut> MapOrElseAsync<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TError?, Task<TOut>> defaultValueProvider, Func<TIn, Task<TOut>> selector)
        => await (await self).MapOrElseAsync(defaultValueProvider, selector).ConfigureAwait(false);

    public static async Task<Result<TOut, TError>> AndThenAsync<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TIn, Task<Result<TOut, TError>>> optionProvider)
        => await (await self).AndThenAsync(optionProvider).ConfigureAwait(false);

    public static async Task<Result<TValue, TOut>> OrElseAsync<TValue, TIn, TOut>(this Task<Result<TValue, TIn>> self, Func<TIn?, Task<Result<TValue, TOut>>> optionProvider)
        => await (await self).OrElseAsync(optionProvider).ConfigureAwait(false);
}