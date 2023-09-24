using Langutils.Core.Options;

namespace Langutils.Core.Results;

public static class TaskResultExtensionsResultExtensions
{
    public static async Task<bool> IsSuccessAnd<TValue, TError>(this Task<Result<TValue, TError>> self, Func<TValue, bool> predicate)
        => (await self).IsSuccessAnd(predicate);

    public static async Task<bool> IsErrorAnd<TValue, TError>(this Task<Result<TValue, TError>> self, Func<TError?, bool> predicate)
        => (await self).IsErrorAnd(predicate);

    public static async Task<TValue> Expect<TValue, TError>(this Task<Result<TValue, TError>> self, string message)
        => (await self).Expect(message);

    public static async Task<TValue> Unwrap<TValue, TError>(this Task<Result<TValue, TError>> self)
        => (await self).Unwrap();

    public static async Task<TValue> UnwrapOr<TValue, TError>(this Task<Result<TValue, TError>> self, TValue defaultValue)
        => (await self).UnwrapOr(defaultValue);

    public static async Task<TValue?> UnwrapOrDefault<TValue, TError>(this Task<Result<TValue, TError>> self)
        => (await self).UnwrapOrDefault();

    public static async Task<TValue> UnwrapOrElse<TValue, TError>(this Task<Result<TValue, TError>> self, Func<TError?, TValue> defaultValueProvider)
        => (await self).UnwrapOrElse(defaultValueProvider);

    public static async Task<TError?> ExpectError<TValue, TError>(this Task<Result<TValue, TError>> self, string message)
        => (await self).ExpectError(message);

    public static async Task<TError?> UnwrapError<TValue, TError>(this Task<Result<TValue, TError>> self)
        => (await self).UnwrapError();

    public static async Task<Option<TValue>> Success<TValue, TError>(this Task<Result<TValue, TError>> self)
        => (await self).Success();

    public static async Task<Option<TError>> Error<TValue, TError>(this Task<Result<TValue, TError>> self)
        => (await self).Error();

    public static async Task<Option<Result<TValue, TError>>> Transpose<TValue, TError>(this Task<Result<Option<TValue>, TError>> self)
        => (await self).Transpose();

    public static async Task<Result<TValue, TError>> Tap<TValue, TError>(this Task<Result<TValue, TError>> self, Action<TValue> onSuccess)
        => (await self).Tap(onSuccess);

    public static async Task<Result<TValue, TError>> TapError<TValue, TError>(this Task<Result<TValue, TError>> self, Action<TError?> onError)
        => (await self).TapError(onError);

    public static async Task<Result<TOut, TError>> SelectMany<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TIn, Result<TOut, TError>> selector)
        => (await self).SelectMany(selector);

    public static async Task<Result<TValue, TError>> Flatten<TValue, TError>(this Task<Result<Result<TValue, TError>, TError>> self)
        => (await self).Flatten();

    public static async Task<Result<TOut, TError>> Select<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TIn, TOut> selector)
        => (await self).Select(selector);

    public static async Task<Result<TValue, TOut>> SelectError<TValue, TIn, TOut>(this Task<Result<TValue, TIn>> self, Func<TIn?, TOut> selector)
        => (await self).SelectError(selector);

    public static async Task<TOut> SelectOr<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, TOut defaultValue, Func<TIn, TOut> selector)
        => (await self).SelectOr(defaultValue, selector);

    public static async Task<TOut> SelectOrElse<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TOut> defaultValueProvider, Func<TIn, TOut> selector)
        => (await self).SelectOrElse(defaultValueProvider, selector);

    public static async Task<IEnumerable<TValue>> AsEnumerable<TValue, TError>(this Task<Result<TValue, TError>> self)
        => (await self).AsEnumerable();

    public static async Task<List<TValue>> ToList<TValue, TError>(this Task<Result<TValue, TError>> self)
        => (await self).ToList();

    public static async Task<TValue[]> ToArray<TValue, TError>(this Task<Result<TValue, TError>> self)
        => (await self).ToArray();

    public static async Task<Result<TValue, TError>> And<TValue, TError>(this Task<Result<TValue, TError>> self, Result<TValue, TError> option)
        => (await self).And(option);

    public static async Task<Result<TValue, TError>> AndThen<TValue, TError>(this Task<Result<TValue, TError>> self, Func<Result<TValue, TError>> optionProvider)
        => (await self).AndThen(optionProvider);

    public static async Task<Result<TValue, TError>> Or<TValue, TError>(this Task<Result<TValue, TError>> self, Result<TValue, TError> option)
        => (await self).Or(option);

    public static async Task<Result<TValue, TError>> OrElse<TValue, TError>(this Task<Result<TValue, TError>> self, Func<Result<TValue, TError>> optionProvider)
        => (await self).OrElse(optionProvider);

    public static async Task<Result<List<TValue>, TError>> Collect<TValue, TError>(this Task<List<Result<TValue, TError>>> results)
        => (await results).Collect();

    public static async Task<Result<TValue[], TError>> Collect<TValue, TError>(this Task<Result<TValue, TError>[]> results)
        => (await results).Collect();

    public static async Task<Result<IEnumerable<TValue>, TError>> Collect<TValue, TError>(this Task<IEnumerable<Result<TValue, TError>>> results)
        => (await results).Collect();
}