namespace Langutils.Core;

/// <summary>
/// Extension methods for inline value inspection and manipulation.
/// </summary>
public static class ScopeExtensions
{
    /// <summary>
    /// This method is used to execute an action on a value while returning the value itself.
    /// </summary>
    /// <param name="self">The value that the action is executed on</param>
    /// <param name="action">The action that should be executed on the value</param>
    /// <typeparam name="T">The value type</typeparam>
    /// <returns>The value</returns>
    public static T Also<T>(this T self, Action<T> action)
    {
        action(self);
        return self;
    }

    /// <summary>
    /// This method is used to execute an action on a value task while returning the value itself.
    /// </summary>
    /// <param name="self">The value task that the action is executed on</param>
    /// <param name="action">The action that should be executed on the value</param>
    /// <typeparam name="T">The value type</typeparam>
    /// <returns>The value</returns>
    public static async Task<T> Also<T>(this Task<T> self, Action<T> action)
        => (await self.ConfigureAwait(false)).Also(action);

    /// <summary>
    /// This method is used to execute an asynchronous action on a value while returning the value itself.
    /// </summary>
    /// <param name="self">The value that the asynchronous action is executed on</param>
    /// <param name="action">The asynchronous action that should be executed on the value</param>
    /// <typeparam name="T">The value type</typeparam>
    /// <returns>The value</returns>
    public static async Task<T> AlsoAsync<T>(this T self, Func<T, Task> action)
    {
        await action(self).ConfigureAwait(false);
        return self;
    }

    /// <summary>
    /// This method is used to execute an asynchronous action on a value task while returning the value itself.
    /// </summary>
    /// <param name="self">The value task that the asynchronous action is executed on</param>
    /// <param name="action">The asynchronous action that should be executed on the value</param>
    /// <typeparam name="T">The value type</typeparam>
    /// <returns>The value</returns>
    public static async Task<T> AlsoAsync<T>(this Task<T> self, Func<T, Task> action)
        => await (await self.ConfigureAwait(false)).AlsoAsync(action).ConfigureAwait(false);

    /// <summary>
    /// This method is used to modify a value inline by using a function.
    /// </summary>
    /// <param name="value">The value that the function is executed on</param>
    /// <param name="func">The function the value is transformed by</param>
    /// <typeparam name="TIn">The value type</typeparam>
    /// <typeparam name="TOut">The function result type</typeparam>
    /// <returns>The result of the function</returns>
    public static TOut Let<TIn, TOut>(this TIn value, Func<TIn, TOut> func)
        => func(value);

    /// <summary>
    /// This method is used to modify a value task inline by using a function.
    /// </summary>
    /// <param name="value">The value task that the function is executed on</param>
    /// <param name="projection">The function the value is transformed by</param>
    /// <typeparam name="TIn">The value type</typeparam>
    /// <typeparam name="TOut">The function result type</typeparam>
    /// <returns>The result of the function</returns>
    public static async Task<TOut> Let<TIn, TOut>(this Task<TIn> value, Func<TIn, TOut> projection)
        => projection(await value.ConfigureAwait(false));

    /// <summary>
    /// This method is used to modify a value inline by using an asynchronous function.
    /// </summary>
    /// <param name="value">The value that the asynchronous function is executed on</param>
    /// <param name="projection">The asynchronous function the value is transformed by</param>
    /// <typeparam name="TIn">The value type</typeparam>
    /// <typeparam name="TOut">The function result type</typeparam>
    /// <returns>The result of the function</returns>
    public static async Task<TOut> LetAsync<TIn, TOut>(this TIn value, Func<TIn, Task<TOut>> projection)
        => await projection(value).ConfigureAwait(false);

    /// <summary>
    /// This method is used to modify a value task inline by using an asynchronous function.
    /// </summary>
    /// <param name="value">The value task that the asynchronous function is executed on</param>
    /// <param name="projection">The asynchronous function the value is transformed by</param>
    /// <typeparam name="TIn">The value type</typeparam>
    /// <typeparam name="TOut">The function result type</typeparam>
    /// <returns>The result of the function</returns>
    public static async Task<TOut> LetAsync<TIn, TOut>(this Task<TIn> value, Func<TIn, Task<TOut>> projection)
        => await (await value.ConfigureAwait(false)).LetAsync(projection).ConfigureAwait(false);
}