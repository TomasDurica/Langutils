using Langutils.Core.Results;

namespace Langutils.Core.Options;

public static partial class OptionExtensions
{
    public static Option<TValue> Filter<TValue>(this Option<TValue> self, Func<TValue, bool> predicate)
        => self switch
        {
            { IsSome: true, Value: var value } when predicate(value) => self,
            _ => None.Instance
        };

    public static async Task<Option<TValue>> Filter<TValue>(this Task<Option<TValue>> self, Func<TValue, bool> predicate)
        => (await self.ConfigureAwait(false)).Filter(predicate);

    public static async Task<Option<TValue>> FilterAsync<TValue>(this Option<TValue> self, Func<TValue, Task<bool>> predicate)
        => self switch
        {
            { IsSome: true, Value: var value } when await predicate(value).ConfigureAwait(false) => self,
            _ => None.Instance
        };

    public static async Task<Option<TValue>> FilterAsync<TValue>(this Task<Option<TValue>> self, Func<TValue, Task<bool>> predicate)
        => await (await self.ConfigureAwait(false)).FilterAsync(predicate).ConfigureAwait(false);

    public static Option<TOut> Map<TIn, TOut>(this Option<TIn> self, Func<TIn, TOut> selector)
        => self switch
        {
            { IsSome: true, Value: var value } => selector(value),
            _ => None.Instance
        };

    public static async Task<Option<TOut>> Map<TIn, TOut>(this Task<Option<TIn>> self, Func<TIn, TOut> selector)
        => (await self.ConfigureAwait(false)).Map(selector);

    public static async Task<Option<TOut>> MapAsync<TIn, TOut>(this Option<TIn> self, Func<TIn, Task<TOut>> selector)
        => self switch
        {
            { IsSome: true, Value: var value } => await selector(value).ConfigureAwait(false),
            _ => None.Instance
        };

    public static async Task<Option<TOut>> MapAsync<TIn, TOut>(this Task<Option<TIn>> self, Func<TIn, Task<TOut>> selector)
        => await (await self.ConfigureAwait(false)).MapAsync(selector).ConfigureAwait(false);

    public static TOut MapOr<TIn, TOut>(this Option<TIn> self, TOut defaultValue, Func<TIn, TOut> selector)
        => self switch
        {
            { IsSome: true, Value: var value } => selector(value),
            _ => defaultValue
        };

    public static async Task<TOut> MapOr<TIn, TOut>(this Task<Option<TIn>> self, TOut defaultValue, Func<TIn, TOut> selector)
        => (await self.ConfigureAwait(false)).MapOr(defaultValue, selector);

    public static async Task<TOut> MapOrAsync<TIn, TOut>(this Option<TIn> self, TOut defaultValue, Func<TIn, Task<TOut>> selector)
        => self switch
        {
            { IsSome: true, Value: var value } => await selector(value).ConfigureAwait(false),
            _ => defaultValue
        };

    public static async Task<TOut> MapOrAsync<TIn, TOut>(this Task<Option<TIn>> self, TOut defaultValue, Func<TIn, Task<TOut>> selector)
        => await (await self.ConfigureAwait(false)).MapOrAsync(defaultValue, selector).ConfigureAwait(false);

    public static TOut MapOrElse<TIn, TOut>(this Option<TIn> self, Func<TOut> defaultValueProvider, Func<TIn, TOut> selector)
        => self switch
        {
            { IsSome: true, Value: var value } => selector(value),
            _ => defaultValueProvider()
        };

    public static async Task<TOut> MapOrElse<TIn, TOut>(this Task<Option<TIn>> self, Func<TOut> defaultValueProvider, Func<TIn, TOut> selector)
        => (await self.ConfigureAwait(false)).MapOrElse(defaultValueProvider, selector);

    public static async Task<TOut> MapOrElseAsync<TIn, TOut>(this Option<TIn> self, Func<TOut> defaultValueProvider, Func<TIn, Task<TOut>> selector)
        => self switch
        {
            { IsSome: true, Value: var value } => await selector(value).ConfigureAwait(false),
            _ => defaultValueProvider()
        };

    public static async Task<TOut> MapOrElseAsync<TIn, TOut>(this Task<Option<TIn>> self, Func<TOut> defaultValueProvider, Func<TIn, Task<TOut>> selector)
        => await (await self.ConfigureAwait(false)).MapOrElseAsync(defaultValueProvider, selector).ConfigureAwait(false);

    public static Result<TValue, TError> SomeOr<TValue, TError>(this Option<TValue> self, TError error)
        => self switch
        {
            { IsSome: true, Value: var value } => value,
            _ => error
        };

    public static async Task<Result<TValue, TError>> SomeOr<TValue, TError>(this Task<Option<TValue>> self, TError error)
        => (await self.ConfigureAwait(false)).SomeOr(error);

    public static Result<TValue, TError> SomeOrElse<TValue, TError>(this Option<TValue> self, Func<TError> errorProvider)
        => self switch
        {
            { IsSome: true, Value: var value } => value,
            _ => errorProvider()
        };

    public static async Task<Result<TValue, TError>> SomeOrElse<TValue, TError>(this Task<Option<TValue>> self, Func<TError> errorProvider)
        => (await self.ConfigureAwait(false)).SomeOrElse(errorProvider);

    public static async Task<Result<TValue, TError>> SomeOrElseAsync<TValue, TError>(this Option<TValue> self, Func<Task<TError>> errorProvider)
        => self switch
        {
            { IsSome: true, Value: var value } => value,
            _ => await errorProvider().ConfigureAwait(false)
        };

    public static async Task<Result<TValue, TError>> SomeOrElseAsync<TValue, TError>(this Task<Option<TValue>> self, Func<Task<TError>> errorProvider)
        => await (await self.ConfigureAwait(false)).SomeOrElseAsync(errorProvider).ConfigureAwait(false);

    public static Result<Option<TValue>, TError> Transpose<TValue, TError>(this Option<Result<TValue, TError>> self)
        => self switch
        {
            { IsSome: true, Value: { IsSuccess: true, Value: var value }} => Option.Some(value),
            { IsSome: true, Value: { IsError: true, Error: var error }} => error!,
            _ => Option.None<TValue>()
        };

    public static async Task<Result<Option<TValue>, TError>> Transpose<TValue, TError>(this Task<Option<Result<TValue, TError>>> self)
        => (await self.ConfigureAwait(false)).Transpose();

    public static Option<TValue> Flatten<TValue>(this Option<Option<TValue>> self)
        => self switch
        {
            { IsSome: true, Value: var option} => option,
            _ => None.Instance
        };

    public static async Task<Option<TValue>> Flatten<TValue>(this Task<Option<Option<TValue>>> self)
        => (await self.ConfigureAwait(false)).Flatten();

    public static Option<(TValue1 Left, TValue2 Right)> Zip<TValue1, TValue2>(this Option<TValue1> self, Option<TValue2> option)
        => (self, option) switch
        {
            ({ IsSome: true, Value: var left }, { IsSome: true, Value: var right}) => (left!, right!),
            _ => None.Instance
        };

    public static async Task<Option<(TLeft Left, TRight Right)>> Zip<TLeft, TRight>(this Task<Option<TLeft>> self, Option<TRight> option)
        => (await self.ConfigureAwait(false)).Zip(option);

    public static Option<TOut> ZipWith<TIn1, TIn2, TOut>(this Option<TIn1> self, Option<TIn2> option, Func<TIn1, TIn2, TOut> selector)
        => (self, option) switch
        {
            ({ IsSome: true, Value: var left }, { IsSome: true, Value: var right}) => selector(left!, right!),
            _ => None.Instance
        };

    public static async Task<Option<TOut>> ZipWith<TLeft, TRight, TOut>(this Task<Option<TLeft>> self, Option<TRight> option, Func<TLeft, TRight, TOut> selector)
        => (await self.ConfigureAwait(false)).ZipWith(option, selector);

    public static async Task<Option<TOut>> ZipWithAsync<TIn1, TIn2, TOut>(this Option<TIn1> self, Option<TIn2> option, Func<TIn1, TIn2, Task<TOut>> selector)
        => (self, option) switch
        {
            ({ IsSome: true, Value: var left }, { IsSome: true, Value: var right}) => await selector(left!, right!).ConfigureAwait(false),
            _ => None.Instance
        };

    public static async Task<Option<TOut>> ZipWithAsync<TIn1, TIn2, TOut>(this Task<Option<TIn1>> self, Option<TIn2> option, Func<TIn1, TIn2, Task<TOut>> selector)
        => await (await self.ConfigureAwait(false)).ZipWithAsync(option, selector).ConfigureAwait(false);

    public static (Option<TValue1> Left, Option<TValue2> Right) Unzip<TValue1, TValue2>(this Option<(TValue1 Left, TValue2 Right)> self)
        => self switch
        {
            { IsSome: true, Value: var (left, right) } => (left, right),
            _ => (None.Instance, None.Instance)
        };

    public static async Task<(Option<TLeft> Left, Option<TRight> Right)> Unzip<TLeft, TRight>(this Task<Option<(TLeft Left, TRight Right)>> self)
        => (await self.ConfigureAwait(false)).Unzip();
}