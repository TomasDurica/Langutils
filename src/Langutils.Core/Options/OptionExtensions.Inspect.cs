namespace Langutils.Core.Options;

public static partial class OptionExtensions
{
    public static bool IsSomeAnd<TValue>(this Option<TValue> self, Func<TValue, bool> predicate) => self switch
    {
        { IsSome: true, Value: var value } => predicate(value),
        _ => false
    };

    public static async Task<bool> IsSomeAnd<TValue>(this Task<Option<TValue>> self, Func<TValue, bool> predicate)
        => (await self).IsSomeAnd(predicate);

    public static async Task<bool> IsSomeAndAsync<TValue>(this Option<TValue> self, Func<TValue, Task<bool>> predicate) => self switch
    {
        { IsSome: true, Value: var value } => await predicate(value).ConfigureAwait(false),
        _ => false
    };

    public static async Task<bool> IsSomeAndAsync<TValue>(this Task<Option<TValue>> self, Func<TValue, Task<bool>> predicate)
        => await (await self).IsSomeAndAsync(predicate).ConfigureAwait(false);

    public static Option<TValue> Tap<TValue>(this Option<TValue> self, Action<TValue> onSome)
    {
        if (self is { IsSome: true, Value: var value })
        {
            onSome(value);
        }

        return self;
    }

    public static async Task<Option<TValue>> Tap<TValue>(this Task<Option<TValue>> self, Action<TValue> onSome)
        => (await self).Tap(onSome);

    public static async Task<Option<TValue>> TapAsync<TValue>(this Option<TValue> self, Func<TValue, Task> onSome)
    {
        if (self is { IsSome: true, Value: var value })
        {
            await onSome(value).ConfigureAwait(false);
        }

        return self;
    }

    public static async Task<Option<TValue>> TapAsync<TValue>(this Task<Option<TValue>> self, Func<TValue, Task> onSome)
        => await (await self).TapAsync(onSome).ConfigureAwait(false);
}