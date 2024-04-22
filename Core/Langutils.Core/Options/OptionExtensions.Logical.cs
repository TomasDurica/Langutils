namespace Langutils.Core.Options;

public static partial class OptionExtensions
{
    /// <summary>
    /// Returns the second option if the first option is Some, otherwise returns None.
    /// </summary>
    /// <typeparam name="TIn">The type of the value in the first Option.</typeparam>
    /// <typeparam name="TOut">The type of the value in the second Option.</typeparam>
    /// <param name="self">The first Option.</param>
    /// <param name="option">The second Option.</param>
    /// <returns>The second Option if the first Option is Some, otherwise None.</returns>
    public static Option<TOut> And<TIn, TOut>(this Option<TIn> self, Option<TOut> option)
        => self switch
        {
            { IsSome: true } => option,
            _ => None.Instance
        };

    /// <summary>
    /// Asynchronously returns the second option if the first option is Some, otherwise returns None.
    /// </summary>
    /// <typeparam name="TIn">The type of the value in the first Option.</typeparam>
    /// <typeparam name="TOut">The type of the value in the second Option.</typeparam>
    /// <param name="self">The Task of the first Option.</param>
    /// <param name="option">The second Option.</param>
    /// <returns>A Task of the second Option if the first Option is Some, otherwise None.</returns>
    public static async Task<Option<TOut>> And<TIn, TOut>(this Task<Option<TIn>> self, Option<TOut> option)
        => (await self.ConfigureAwait(false)).And(option);

    /// <summary>
    /// Returns the result of the optionProvider if the first option is Some, otherwise returns None.
    /// </summary>
    /// <typeparam name="TIn">The type of the value in the first Option.</typeparam>
    /// <typeparam name="TOut">The type of the value in the second Option.</typeparam>
    /// <param name="self">The first Option.</param>
    /// <param name="optionProvider">A function that provides the second Option.</param>
    /// <returns>The result of the optionProvider if the first Option is Some, otherwise None.</returns>
    public static Option<TOut> AndThen<TIn, TOut>(this Option<TIn> self, Func<TIn, Option<TOut>> optionProvider)
        => self switch
        {
            { IsSome: true, Value: var value } => optionProvider(value),
            _ => None.Instance
        };

    /// <summary>
    /// Asynchronously returns the result of the optionProvider if the first option is Some, otherwise returns None.
    /// </summary>
    /// <typeparam name="TIn">The type of the value in the first Option.</typeparam>
    /// <typeparam name="TOut">The type of the value in the second Option.</typeparam>
    /// <param name="self">The Task of the first Option.</param>
    /// <param name="optionProvider">A function that provides the second Option.</param>
    /// <returns>A Task of the result of the optionProvider if the first Option is Some, otherwise None.</returns>
    public static async Task<Option<TOut>> AndThen<TIn, TOut>(this Task<Option<TIn>> self, Func<TIn, Option<TOut>> optionProvider)
        => (await self.ConfigureAwait(false)).AndThen(optionProvider);

    /// <summary>
    /// Asynchronously returns the result of the optionProvider if the first option is Some, otherwise returns None.
    /// </summary>
    /// <typeparam name="TIn">The type of the value in the first Option.</typeparam>
    /// <typeparam name="TOut">The type of the value in the second Option.</typeparam>
    /// <param name="self">The first Option.</param>
    /// <param name="optionProvider">A function that provides the second Option.</param>
    /// <returns>A Task of the result of the optionProvider if the first Option is Some, otherwise None.</returns>
    public static async Task<Option<TOut>> AndThenAsync<TIn, TOut>(this Option<TIn> self, Func<TIn, Task<Option<TOut>>> optionProvider)
        => self switch
        {
            { IsSome: true, Value: var value } => await optionProvider(value).ConfigureAwait(false),
            _ => None.Instance
        };

    /// <summary>
    /// Asynchronously returns the result of the optionProvider if the first option is Some, otherwise returns None.
    /// </summary>
    /// <typeparam name="TIn">The type of the value in the first Option.</typeparam>
    /// <typeparam name="TOut">The type of the value in the second Option.</typeparam>
    /// <param name="self">The Task of the first Option.</param>
    /// <param name="optionProvider">A function that provides the second Option.</param>
    /// <returns>A Task of the result of the optionProvider if the first Option is Some, otherwise None.</returns>
    public static async Task<Option<TOut>> AndThenAsync<TIn, TOut>(this Task<Option<TIn>> self, Func<TIn, Task<Option<TOut>>> optionProvider)
        => await (await self.ConfigureAwait(false)).AndThenAsync(optionProvider).ConfigureAwait(false);

    /// <summary>
    /// Returns the first option if it is Some, otherwise returns the second option.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the Options.</typeparam>
    /// <param name="self">The first Option.</param>
    /// <param name="option">The second Option.</param>
    /// <returns>The first Option if it is Some, otherwise the second Option.</returns>
    public static Option<TValue> Or<TValue>(this Option<TValue> self, Option<TValue> option)
        => self switch
        {
            { IsSome: true } => self,
            _ => option
        };

    /// <summary>
    /// Asynchronously returns the first option if it is Some, otherwise returns the second option.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the Options.</typeparam>
    /// <param name="self">The Task of the first Option.</param>
    /// <param name="option">The second Option.</param>
    /// <returns>A Task of the first Option if it is Some, otherwise the second Option.</returns>
    public static async Task<Option<TValue>> Or<TValue>(this Task<Option<TValue>> self, Option<TValue> option)
        => (await self.ConfigureAwait(false)).Or(option);

    /// <summary>
    /// Returns the first option if it is Some, otherwise returns the result of the optionProvider.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the Options.</typeparam>
    /// <param name="self">The first Option.</param>
    /// <param name="optionProvider">A function that provides the second Option.</param>
    /// <returns>The first Option if it is Some, otherwise the result of the optionProvider.</returns>
    public static Option<TValue> OrElse<TValue>(this Option<TValue> self, Func<Option<TValue>> optionProvider)
        => self switch
        {
            { IsSome: true } => self,
            _ => optionProvider()
        };

    /// <summary>
    /// Asynchronously returns the first option if it is Some, otherwise returns the result of the optionProvider.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the Options.</typeparam>
    /// <param name="self">The Task of the first Option.</param>
    /// <param name="optionProvider">A function that provides the second Option.</param>
    /// <returns>A Task of the first Option if it is Some, otherwise the result of the optionProvider.</returns>
    public static async Task<Option<TValue>> OrElse<TValue>(this Task<Option<TValue>> self, Func<Option<TValue>> optionProvider)
        => (await self.ConfigureAwait(false)).OrElse(optionProvider);

    /// <summary>
    /// Asynchronously returns the first option if it is Some, otherwise returns the result of the optionProvider.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the Options.</typeparam>
    /// <param name="self">The first Option.</param>
    /// <param name="optionProvider">A function that provides the second Option.</param>
    /// <returns>A Task of the first Option if it is Some, otherwise the result of the optionProvider.</returns>
    public static async Task<Option<TValue>> OrElseAsync<TValue>(this Option<TValue> self, Func<Task<Option<TValue>>> optionProvider)
        => self switch
        {
            { IsSome: true } => self,
            _ => await optionProvider().ConfigureAwait(false)
        };

    /// <summary>
    /// Asynchronously returns the first option if it is Some, otherwise returns the result of the optionProvider.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the Options.</typeparam>
    /// <param name="self">The Task of the first Option.</param>
    /// <param name="optionProvider">A function that provides the second Option.</param>
    /// <returns>A Task of the first Option if it is Some, otherwise the result of the optionProvider.</returns>
    public static async Task<Option<TValue>> OrElseAsync<TValue>(this Task<Option<TValue>> self, Func<Task<Option<TValue>>> optionProvider)
        => await (await self.ConfigureAwait(false)).OrElseAsync(optionProvider).ConfigureAwait(false);

    /// <summary>
    /// Returns the first option if only one of the options is Some, otherwise returns None.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the Options.</typeparam>
    /// <param name="self">The first Option.</param>
    /// <param name="option">The second Option.</param>
    /// <returns>The first Option if only one of the options is Some, otherwise None.</returns>
    public static Option<TValue> Xor<TValue>(this Option<TValue> self, Option<TValue> option)
        => (self, option) switch
        {
            (self: { IsSome: true }, option: { IsSome: false }) => self,
            (self: { IsSome: false }, option: { IsSome: true }) => option,
            _ => None.Instance
        };

    /// <summary>
    /// Asynchronously returns the first option if only one of the options is Some, otherwise returns None.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the Options.</typeparam>
    /// <param name="self">The Task of the first Option.</param>
    /// <param name="option">The second Option.</param>
    /// <returns>A Task of the first Option if only one of the options is Some, otherwise None.</returns>
    public static async Task<Option<TValue>> Xor<TValue>(this Task<Option<TValue>> self, Option<TValue> option)
        => (await self.ConfigureAwait(false)).Xor(option);
}