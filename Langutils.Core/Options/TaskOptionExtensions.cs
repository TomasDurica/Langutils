using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Langutils.Core.Results;

namespace Langutils.Core.Options;

public static class TaskOptionExtensions
{
    public static async Task<bool> IsSomeAnd<TValue>(this Task<Option<TValue>> self, Func<TValue, bool> predicate)
        => (await self).IsSomeAnd(predicate);

    public static async Task<TValue> Expect<TValue>(this Task<Option<TValue>> self, string message)
        => (await self).Expect(message);

    public static async Task<TValue> Unwrap<TValue>(this Task<Option<TValue>> self)
        => (await self).Unwrap();

    public static async Task<TValue> UnwrapOr<TValue>(this Task<Option<TValue>> self, TValue defaultValue)
        => (await self).UnwrapOr(defaultValue);

    public static async Task<TValue?> UnwrapOrDefault<TValue>(this Task<Option<TValue>> self)
        => (await self).UnwrapOrDefault();

    public static async Task<TValue> UnwrapOrElse<TValue>(this Task<Option<TValue>> self, Func<TValue> defaultValueProvider)
        => (await self).UnwrapOrElse(defaultValueProvider);

    public static async Task<Option<TValue>> Tap<TValue>(this Task<Option<TValue>> self, Action<TValue> onSome)
        => (await self).Tap(onSome);

    public static async Task<Option<TValue>> Where<TValue>(this Task<Option<TValue>> self, Func<TValue, bool> predicate)
        => (await self).Where(predicate);

    public static async Task<Option<TOut>> SelectMany<TIn, TOut>(this Task<Option<TIn>> self, Func<TIn, Option<TOut>> selector)
        => (await self).SelectMany(selector);

    public static async Task<Option<TValue>> Flatten<TValue>(this Task<Option<Option<TValue>>> self)
        => (await self).Flatten();

    public static async Task<Option<TOut>> Select<TIn, TOut>(this Task<Option<TIn>> self, Func<TIn, TOut> selector)
        => (await self).Select(selector);

    public static async Task<TOut> SelectOr<TIn, TOut>(this Task<Option<TIn>> self, TOut defaultValue, Func<TIn, TOut> selector)
        => (await self).SelectOr(defaultValue, selector);

    public static async Task<TOut> SelectOrElse<TIn, TOut>(this Task<Option<TIn>> self, Func<TOut> defaultValueProvider, Func<TIn, TOut> selector)
        => (await self).SelectOrElse(defaultValueProvider, selector);

    public static async Task<Result<TValue, TError>> SomeOr<TValue, TError>(this Task<Option<TValue>> self, TError error)
        => (await self).SomeOr(error);

    public static async Task<Result<TValue, TError>> SomeOrElse<TValue, TError>(this Task<Option<TValue>> self, Func<TError> errorProvider)
        => (await self).SomeOrElse(errorProvider);

    public static async Task<Result<Option<TValue>, TError>> Transpose<TValue, TError>(this Task<Option<Result<TValue, TError>>> self)
        => (await self).Transpose();

    public static async Task<IEnumerable<TValue>> AsEnumerable<TValue>(this Task<Option<TValue>> self)
        => (await self).AsEnumerable();

    public static async Task<List<TValue>> ToList<TValue>(this Task<Option<TValue>> self)
        => (await self).ToList();

    public static async Task<TValue[]> ToArray<TValue>(this Task<Option<TValue>> self)
        => (await self).ToArray();

    public static async Task<Option<TValue>> And<TValue>(this Task<Option<TValue>> self, Option<TValue> option)
        => (await self).And(option);

    public static async Task<Option<TValue>> AndThen<TValue>(this Task<Option<TValue>> self, Func<Option<TValue>> optionProvider)
        => (await self).AndThen(optionProvider);

    public static async Task<Option<TValue>> Or<TValue>(this Task<Option<TValue>> self, Option<TValue> option)
        => (await self).Or(option);

    public static async Task<Option<TValue>> OrElse<TValue>(this Task<Option<TValue>> self, Func<Option<TValue>> optionProvider)
        => (await self).OrElse(optionProvider);

    public static async Task<Option<TValue>> Xor<TValue>(this Task<Option<TValue>> self, Option<TValue> option)
        => (await self).Xor(option);

    public static async Task<Option<(TValue1 Left, TValue2 Right)>> Zip<TValue1, TValue2>(this Task<Option<TValue1>> self, Option<TValue2> option)
        => (await self).Zip(option);

    public static async Task<Option<TOut>> ZipWith<TIn1, TIn2, TOut>(this Task<Option<TIn1>> self, Option<TIn2> option, Func<TIn1, TIn2, TOut> selector)
        => (await self).ZipWith(option, selector);

    public static async Task<(Option<TValue1> Left, Option<TValue2> Right)> Unzip<TValue1, TValue2>(this Task<Option<(TValue1 Left, TValue2 Right)>> self)
        => (await self).Unzip();

    public static async Task<Option<List<TValue>>> Collect<TValue>(this Task<List<Option<TValue>>> options)
        => (await options).Collect();

    public static async Task<Option<TValue[]>> Collect<TValue>(this Task<Option<TValue>[]> options)
        => (await options).Collect();

    public static async Task<Option<IEnumerable<TValue>>> Collect<TValue>(this Task<IEnumerable<Option<TValue>>> options)
        => (await options).Collect();
}