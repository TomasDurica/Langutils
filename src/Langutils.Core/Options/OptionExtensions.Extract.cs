using System.Diagnostics.CodeAnalysis;

namespace Langutils.Core.Options;

public static partial class OptionExtensions
{
    public static TValue Expect<TValue>(this Option<TValue> self, string message)
        => self switch
        {
            { IsSome: true, Value: var value } => value,
            _ => throw new InvalidOperationException(message)
        };

    public static async Task<TValue> Expect<TValue>(this Task<Option<TValue>> self, string message)
        => (await self).Expect(message);

    public static TValue Unwrap<TValue>(this Option<TValue> self)
        => self switch
        {
            { IsSome: true, Value: var value } => value,
            _ => throw new InvalidOperationException($"Called `{nameof(Unwrap)}()` on a `{nameof(Option.None)}` value")
        };

    public static async Task<TValue> Unwrap<TValue>(this Task<Option<TValue>> self)
        => (await self).Unwrap();

    public static bool TryUnwrap<TValue>(this Option<TValue> self, [NotNullWhen(true)] out TValue? value)
    {
        if (self is { IsSome: true, Value: var innerValue })
        {
            value = innerValue;
            return true;
        }

        value = default;
        return false;
    }

    public static TValue UnwrapOr<TValue>(this Option<TValue> self, TValue defaultValue)
        => self switch
        {
            { IsSome: true, Value: var value } => value,
            _ => defaultValue
        };

    public static async Task<TValue> UnwrapOr<TValue>(this Task<Option<TValue>> self, TValue defaultValue)
        => (await self).UnwrapOr(defaultValue);

    public static TValue? UnwrapOrDefault<TValue>(this Option<TValue> self)
        => self switch
        {
            { IsSome: true, Value: var value } => value,
            _ => default
        };

    public static async Task<TValue?> UnwrapOrDefault<TValue>(this Task<Option<TValue>> self)
        => (await self).UnwrapOrDefault();

    public static TValue UnwrapOrElse<TValue>(this Option<TValue> self, Func<TValue> defaultValueProvider)
        => self switch
        {
            { IsSome: true, Value: var value } => value,
            _ => defaultValueProvider()
        };

    public static async Task<TValue> UnwrapOrElse<TValue>(this Task<Option<TValue>> self, Func<TValue> defaultValueProvider)
        => (await self).UnwrapOrElse(defaultValueProvider);

    public static async Task<TValue> UnwrapOrElseAsync<TValue>(this Option<TValue> self, Func<Task<TValue>> defaultValueProvider)
        => self switch
        {
            { IsSome: true, Value: var value } => value,
            _ => await defaultValueProvider().ConfigureAwait(false)
        };

    public static async Task<TValue> UnwrapOrElseAsync<TValue>(this Task<Option<TValue>> self, Func<Task<TValue>> defaultValueProvider)
        => await (await self).UnwrapOrElseAsync(defaultValueProvider).ConfigureAwait(false);
}