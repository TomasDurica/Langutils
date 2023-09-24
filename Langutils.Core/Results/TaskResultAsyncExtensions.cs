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

    public static async Task<Result<TOut, TError>> SelectManyAsync<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TIn, Task<Result<TOut, TError>>> selector)
        => await (await self).SelectManyAsync(selector).ConfigureAwait(false);

    public static async Task<Result<TOut, TError>> SelectAsync<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TIn, Task<TOut>> selector)
        => await (await self).SelectAsync(selector).ConfigureAwait(false);

    public static async Task<Result<TValue, TOut>> SelectErrorAsync<TValue, TIn, TOut>(this Task<Result<TValue, TIn>> self, Func<TIn?, Task<TOut>> selector)
        => await (await self).SelectErrorAsync(selector).ConfigureAwait(false);

    public static async Task<TOut> SelectOrAsync<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, TOut defaultValue, Func<TIn, Task<TOut>> selector)
        => await (await self).SelectOrAsync(defaultValue, selector).ConfigureAwait(false);

    public static async Task<TOut> SelectOrElseAsync<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TOut> defaultValueProvider, Func<TIn, Task<TOut>> selector)
        => await (await self).SelectOrElseAsync(defaultValueProvider, selector).ConfigureAwait(false);

    public static async Task<TOut> SelectOrElseAsync<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<Task<TOut>> defaultValueProvider, Func<TIn, TOut> selector)
        => await (await self).SelectOrElseAsync(defaultValueProvider, selector).ConfigureAwait(false);

    public static async Task<TOut> SelectOrElseAsync<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<Task<TOut>> defaultValueProvider, Func<TIn, Task<TOut>> selector)
        => await (await self).SelectOrElseAsync(defaultValueProvider, selector).ConfigureAwait(false);

    public static async Task<Result<TValue, TError>> AndThenAsync<TValue, TError>(this Task<Result<TValue, TError>> self, Func<Task<Result<TValue, TError>>> optionProvider)
        => await (await self).AndThenAsync(optionProvider).ConfigureAwait(false);

    public static async Task<Result<TValue, TError>> OrElseAsync<TValue, TError>(this Task<Result<TValue, TError>> self, Func<Task<Result<TValue, TError>>> optionProvider)
        => await (await self).OrElseAsync(optionProvider).ConfigureAwait(false);
}