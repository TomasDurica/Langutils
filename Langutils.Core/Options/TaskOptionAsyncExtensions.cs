﻿using Langutils.Core.Results;

namespace Langutils.Core.Options;

public static class TaskOptionAsyncExtensions
{
    public static async Task<bool> IsSomeAndAsync<TValue>(this Task<Option<TValue>> self, Func<TValue, Task<bool>> predicate)
        => await (await self).IsSomeAndAsync(predicate).ConfigureAwait(false);

    public static async Task<TValue> UnwrapOrElseAsync<TValue>(this Task<Option<TValue>> self, Func<Task<TValue>> defaultValueProvider)
        => await (await self).UnwrapOrElseAsync(defaultValueProvider).ConfigureAwait(false);

    public static async Task<Option<TValue>> TapAsync<TValue>(this Task<Option<TValue>> self, Func<TValue, Task> onSome)
        => await (await self).TapAsync(onSome).ConfigureAwait(false);

    public static async Task<Option<TValue>> FilterAsync<TValue>(this Task<Option<TValue>> self, Func<TValue, Task<bool>> predicate)
        => await (await self).FilterAsync(predicate).ConfigureAwait(false);

    public static async Task<Option<TOut>> MapAsync<TIn, TOut>(this Task<Option<TIn>> self, Func<TIn, Task<TOut>> selector)
        => await (await self).MapAsync(selector).ConfigureAwait(false);

    public static async Task<TOut> MapOrAsync<TIn, TOut>(this Task<Option<TIn>> self, TOut defaultValue, Func<TIn, Task<TOut>> selector)
        => await (await self).MapOrAsync(defaultValue, selector).ConfigureAwait(false);

    public static async Task<TOut> MapOrElseAsync<TIn, TOut>(this Task<Option<TIn>> self, Func<TOut> defaultValueProvider, Func<TIn, Task<TOut>> selector)
        => await (await self).MapOrElseAsync(defaultValueProvider, selector).ConfigureAwait(false);

    public static async Task<Result<TValue, TError>> SomeOrElseAsync<TValue, TError>(this Task<Option<TValue>> self, Func<Task<TError>> errorProvider)
        => await (await self).SomeOrElseAsync(errorProvider).ConfigureAwait(false);

    public static async Task<Option<TOut>> AndThenAsync<TIn, TOut>(this Task<Option<TIn>> self, Func<TIn, Task<Option<TOut>>> optionProvider)
        => await (await self).AndThenAsync(optionProvider).ConfigureAwait(false);

    public static async Task<Option<TValue>> OrElseAsync<TValue>(this Task<Option<TValue>> self, Func<Task<Option<TValue>>> optionProvider)
        => await (await self).OrElseAsync(optionProvider).ConfigureAwait(false);

    public static async Task<Option<TOut>> ZipWithAsync<TIn1, TIn2, TOut>(this Task<Option<TIn1>> self, Option<TIn2> option, Func<TIn1, TIn2, Task<TOut>> selector)
        => await (await self).ZipWithAsync(option, selector).ConfigureAwait(false);
}