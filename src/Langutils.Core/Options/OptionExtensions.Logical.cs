namespace Langutils.Core.Options;

public static partial class OptionExtensions
{
    public static Option<TOut> And<TIn, TOut>(this Option<TIn> self, Option<TOut> option)
        => self switch
        {
            { IsSome: true } => option,
            _ => None.Instance
        };

    public static async Task<Option<TOut>> And<TIn, TOut>(this Task<Option<TIn>> self, Option<TOut> option)
        => (await self).And(option);

    public static Option<TOut> AndThen<TIn, TOut>(this Option<TIn> self, Func<TIn, Option<TOut>> optionProvider)
        => self switch
        {
            { IsSome: true, Value: var value } => optionProvider(value),
            _ => None.Instance
        };

    public static async Task<Option<TOut>> AndThen<TIn, TOut>(this Task<Option<TIn>> self, Func<TIn, Option<TOut>> optionProvider)
        => (await self).AndThen(optionProvider);

    public static async Task<Option<TOut>> AndThenAsync<TIn, TOut>(this Option<TIn> self, Func<TIn, Task<Option<TOut>>> optionProvider)
        => self switch
        {
            { IsSome: true, Value: var value } => await optionProvider(value).ConfigureAwait(false),
            _ => None.Instance
        };

    public static async Task<Option<TOut>> AndThenAsync<TIn, TOut>(this Task<Option<TIn>> self, Func<TIn, Task<Option<TOut>>> optionProvider)
        => await (await self).AndThenAsync(optionProvider).ConfigureAwait(false);

    public static Option<TValue> Or<TValue>(this Option<TValue> self, Option<TValue> option)
        => self switch
        {
            { IsSome: true } => self,
            _ => option
        };

    public static async Task<Option<TValue>> Or<TValue>(this Task<Option<TValue>> self, Option<TValue> option)
        => (await self).Or(option);

    public static Option<TValue> OrElse<TValue>(this Option<TValue> self, Func<Option<TValue>> optionProvider)
        => self switch
        {
            { IsSome: true } => self,
            _ => optionProvider()
        };

    public static async Task<Option<TValue>> OrElse<TValue>(this Task<Option<TValue>> self, Func<Option<TValue>> optionProvider)
        => (await self).OrElse(optionProvider);

    public static async Task<Option<TValue>> OrElseAsync<TValue>(this Option<TValue> self, Func<Task<Option<TValue>>> optionProvider)
        => self switch
        {
            { IsSome: true } => self,
            _ => await optionProvider().ConfigureAwait(false)
        };

    public static async Task<Option<TValue>> OrElseAsync<TValue>(this Task<Option<TValue>> self, Func<Task<Option<TValue>>> optionProvider)
        => await (await self).OrElseAsync(optionProvider).ConfigureAwait(false);

    public static Option<TValue> Xor<TValue>(this Option<TValue> self, Option<TValue> option)
        => (self, option) switch
        {
            (self: { IsSome: true }, option: { IsSome: false }) => self,
            (self: { IsSome: false }, option: { IsSome: true }) => option,
            _ => None.Instance
        };

    public static async Task<Option<TValue>> Xor<TValue>(this Task<Option<TValue>> self, Option<TValue> option)
        => (await self).Xor(option);
}