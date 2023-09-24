using Langutils.Core.Results;

namespace Langutils.Core.Options;

public static class OptionExtensionsAsync
{
    public static async Task<bool> IsSomeAndAsync<TValue>(this Option<TValue> self, Func<TValue, Task<bool>> predicate) => self switch
    {
        { IsSome: true, Value: var value } => await predicate(value).ConfigureAwait(false),
        _ => false
    };

    public static async Task<TValue> UnwrapOrElseAsync<TValue>(this Option<TValue> self, Func<Task<TValue>> defaultValueProvider) => self switch
    {
        { IsSome: true, Value: var value } => value,
        _ => await defaultValueProvider().ConfigureAwait(false)
    };

    public static async Task<Option<TValue>> TapAsync<TValue>(this Option<TValue> self, Func<TValue, Task> onSome)
    {
        if (self is { IsSome: true, Value: var value })
        {
            await onSome(value).ConfigureAwait(false);
        }

        return self;
    }

    public static async Task<Option<TValue>> FilterAsync<TValue>(this Option<TValue> self, Func<TValue, Task<bool>> predicate) => self switch
    {
        { IsSome: true, Value: var value } when await predicate(value).ConfigureAwait(false) => self,
        _ => None.Instance
    };

    public static async Task<Option<TOut>> MapAsync<TIn, TOut>(this Option<TIn> self, Func<TIn, Task<TOut>> selector) => self switch
    {
        { IsSome: true, Value: var value } => await selector(value).ConfigureAwait(false),
        _ => None.Instance
    };

    public static async Task<TOut> MapOrAsync<TIn, TOut>(this Option<TIn> self, TOut defaultValue, Func<TIn, Task<TOut>> selector) => self switch
    {
        { IsSome: true, Value: var value } => await selector(value).ConfigureAwait(false),
        _ => defaultValue
    };

    public static async Task<TOut> MapOrElseAsync<TIn, TOut>(this Option<TIn> self, Func<TOut> defaultValueProvider, Func<TIn, Task<TOut>> selector) => self switch
    {
        { IsSome: true, Value: var value } => await selector(value).ConfigureAwait(false),
        _ => defaultValueProvider()
    };

    public static async Task<Result<TValue, TError>> SomeOrElseAsync<TValue, TError>(this Option<TValue> self, Func<Task<TError>> errorProvider) => self switch
    {
        { IsSome: true, Value: var value } => value,
        _ => await errorProvider().ConfigureAwait(false)
    };

    public static async Task<Option<TOut>> AndThenAsync<TIn, TOut>(this Option<TIn> self, Func<TIn, Task<Option<TOut>>> optionProvider) => self switch
    {
        { IsSome: true, Value: var value } => await optionProvider(value).ConfigureAwait(false),
        _ => None.Instance
    };

    public static async Task<Option<TValue>> OrElseAsync<TValue>(this Option<TValue> self, Func<Task<Option<TValue>>> optionProvider) => self switch
    {
        { IsSome: true } => self,
        _ => await optionProvider().ConfigureAwait(false)
    };

    public static async Task<Option<TOut>> ZipWithAsync<TIn1, TIn2, TOut>(this Option<TIn1> self, Option<TIn2> option, Func<TIn1, TIn2, Task<TOut>> selector) => (self, option) switch
    {
        ({ IsSome: true, Value: var left }, { IsSome: true, Value: var right}) => await selector(left!, right!).ConfigureAwait(false),
        _ => None.Instance
    };
}