namespace Langutils.Core.Options;

public static partial class OptionExtensions
{
    /// <summary>
    /// Checks if the Option is Some and the value satisfies the provided predicate.
    /// </summary>
    /// <param name="self">The Option to check.</param>
    /// <param name="predicate">The predicate to apply to the value of the Option.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>True if the Option is Some and the value satisfies the predicate, false otherwise.</returns>
    public static bool IsSomeAnd<TValue>(this Option<TValue> self, Func<TValue, bool> predicate)
        => self switch
        {
            { IsSome: true, Value: var value } => predicate(value),
            _ => false
        };

    /// <summary>
    /// Asynchronously checks if the Option is Some and the value satisfies the provided predicate.
    /// </summary>
    /// <param name="self">The Task of Option to check.</param>
    /// <param name="predicate">The predicate to apply to the value of the Option.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>A Task of true if the Option is Some and the value satisfies the predicate, false otherwise.</returns>
    public static async Task<bool> IsSomeAnd<TValue>(this Task<Option<TValue>> self, Func<TValue, bool> predicate)
        => (await self.ConfigureAwait(false)).IsSomeAnd(predicate);

    /// <summary>
    /// Asynchronously checks if the Option is Some and the value satisfies the provided asynchronous predicate.
    /// </summary>
    /// <param name="self">The Option to check.</param>
    /// <param name="predicate">The asynchronous predicate to apply to the value of the Option.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>A Task of true if the Option is Some and the value satisfies the predicate, false otherwise.</returns>
    public static async Task<bool> IsSomeAndAsync<TValue>(this Option<TValue> self, Func<TValue, Task<bool>> predicate)
        => self switch
        {
            { IsSome: true, Value: var value } => await predicate(value).ConfigureAwait(false),
            _ => false
        };

    /// <summary>
    /// Asynchronously checks if the Option is Some and the value satisfies the provided asynchronous predicate.
    /// </summary>
    /// <param name="self">The Task of Option to check.</param>
    /// <param name="predicate">The asynchronous predicate to apply to the value of the Option.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>A Task of true if the Option is Some and the value satisfies the predicate, false otherwise.</returns>
    public static async Task<bool> IsSomeAndAsync<TValue>(this Task<Option<TValue>> self, Func<TValue, Task<bool>> predicate)
        => await (await self.ConfigureAwait(false)).IsSomeAndAsync(predicate).ConfigureAwait(false);

    /// <summary>
    /// Performs an action on the value of the Option if it is Some.
    /// </summary>
    /// <param name="self">The Option to tap.</param>
    /// <param name="onSome">The action to perform on the value of the Option.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>The original Option.</returns>
    public static Option<TValue> Tap<TValue>(this Option<TValue> self, Action<TValue> onSome)
    {
        if (self is { IsSome: true, Value: var value })
        {
            onSome(value);
        }

        return self;
    }

    /// <summary>
    /// Performs an action on the value of the Option if it is Some.
    /// </summary>
    /// <param name="self">The Task of Option to tap.</param>
    /// <param name="onSome">The action to perform on the value of the Option.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>A Task of the original Option.</returns>
    public static async Task<Option<TValue>> Tap<TValue>(this Task<Option<TValue>> self, Action<TValue> onSome)
        => (await self.ConfigureAwait(false)).Tap(onSome);

    /// <summary>
    /// Performs an asynchronous action on the value of the Option if it is Some.
    /// </summary>
    /// <param name="self">The Option to tap.</param>
    /// <param name="onSome">The asynchronous action to perform on the value of the Option.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>A Task of the original Option.</returns>
    public static async Task<Option<TValue>> TapAsync<TValue>(this Option<TValue> self, Func<TValue, Task> onSome)
    {
        if (self is { IsSome: true, Value: var value })
        {
            await onSome(value).ConfigureAwait(false);
        }

        return self;
    }

    /// <summary>
    /// Performs an asynchronous action on the value of the Option if it is Some.
    /// </summary>
    /// <param name="self">The Task of Option to tap.</param>
    /// <param name="onSome">The asynchronous action to perform on the value of the Option.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>A Task of the original Option.</returns>
    public static async Task<Option<TValue>> TapAsync<TValue>(this Task<Option<TValue>> self, Func<TValue, Task> onSome)
        => await (await self.ConfigureAwait(false)).TapAsync(onSome).ConfigureAwait(false);
}