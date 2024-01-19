using System.Diagnostics.CodeAnalysis;

namespace Langutils.Core.Options;

public static partial class OptionExtensions
{
    /// <summary>
    /// Returns the value from a Some Option or throws an InvalidOperationException if the Option is None.
    /// </summary>
    /// <param name="self">The Option to extract the value from.</param>
    /// <param name="message">The message to include in the exception if the Option is None.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>The value from a Some Option.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Option is None.</exception>
    public static TValue Expect<TValue>(this Option<TValue> self, string message)
        => self switch
        {
            { IsSome: true, Value: var value } => value,
            _ => throw new InvalidOperationException(message)
        };

    /// <summary>
    /// Asynchronously returns the value from a Some Option or throws an InvalidOperationException if the Option is None.
    /// </summary>
    /// <param name="self">The Task of Option to extract the value from.</param>
    /// <param name="message">The message to include in the exception if the Option is None.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>A Task of the value from a Some Option.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Option is None.</exception>
    public static async Task<TValue> Expect<TValue>(this Task<Option<TValue>> self, string message)
        => (await self.ConfigureAwait(false)).Expect(message);

    /// <summary>
    /// Returns the value from a Some Option or throws an InvalidOperationException if the Option is None.
    /// </summary>
    /// <param name="self">The Option to extract the value from.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>The value from a Some Option.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Option is None.</exception>
    public static TValue Unwrap<TValue>(this Option<TValue> self)
        => self switch
        {
            { IsSome: true, Value: var value } => value,
            _ => throw new InvalidOperationException($"Called `{nameof(Unwrap)}()` on a `{nameof(Option)}.{nameof(Option.None)}` value")
        };

    /// <summary>
    /// Asynchronously returns the value from a Some Option or throws an InvalidOperationException if the Option is None.
    /// </summary>
    /// <param name="self">The Task of Option to extract the value from.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>A Task of the value from a Some Option.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Option is None.</exception>
    public static async Task<TValue> Unwrap<TValue>(this Task<Option<TValue>> self)
        => (await self.ConfigureAwait(false)).Unwrap();

    /// <summary>
    /// Tries to extract the value from a Some Option.
    /// </summary>
    /// <param name="self">The Option to extract the value from.</param>
    /// <param name="value">The extracted value if the Option is Some, otherwise default.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>True if the Option is Some and the value was extracted, otherwise false.</returns>
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

    /// <summary>
    /// Returns the value from a Some Option or a default value if the Option is None.
    /// </summary>
    /// <param name="self">The Option to extract the value from.</param>
    /// <param name="defaultValue">The default value to return if the Option is None.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>The value from a Some Option or the default value.</returns>
    public static TValue UnwrapOr<TValue>(this Option<TValue> self, TValue defaultValue)
        => self switch
        {
            { IsSome: true, Value: var value } => value,
            _ => defaultValue
        };

    /// <summary>
    /// Asynchronously returns the value from a Some Option or a default value if the Option is None.
    /// </summary>
    /// <param name="self">The Task of Option to extract the value from.</param>
    /// <param name="defaultValue">The default value to return if the Option is None.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>A Task of the value from a Some Option or the default value.</returns>
    public static async Task<TValue> UnwrapOr<TValue>(this Task<Option<TValue>> self, TValue defaultValue)
        => (await self.ConfigureAwait(false)).UnwrapOr(defaultValue);

    /// <summary>
    /// Returns the value from a Some Option or the default value of the type if the Option is None.
    /// </summary>
    /// <param name="self">The Option to extract the value from.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>The value from a Some Option or the default value of the type.</returns>
    public static TValue? UnwrapOrDefault<TValue>(this Option<TValue> self)
        => self switch
        {
            { IsSome: true, Value: var value } => value,
            _ => default
        };

    /// <summary>
    /// Asynchronously returns the value from a Some Option or the default value of the type if the Option is None.
    /// </summary>
    /// <param name="self">The Task of Option to extract the value from.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>A Task of the value from a Some Option or the default value of the type.</returns>
    public static async Task<TValue?> UnwrapOrDefault<TValue>(this Task<Option<TValue>> self)
        => (await self.ConfigureAwait(false)).UnwrapOrDefault();

    /// <summary>
    /// Returns the value from a Some Option or a value provided by a default value provider if the Option is None.
    /// </summary>
    /// <param name="self">The Option to extract the value from.</param>
    /// <param name="defaultValueProvider">The function that provides the default value if the Option is None.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>The value from a Some Option or a value provided by the default value provider.</returns>
    public static TValue UnwrapOrElse<TValue>(this Option<TValue> self, Func<TValue> defaultValueProvider)
        => self switch
        {
            { IsSome: true, Value: var value } => value,
            _ => defaultValueProvider()
        };

    /// <summary>
    /// Asynchronously returns the value from a Some Option or a value provided by a default value provider if the Option is None.
    /// </summary>
    /// <param name="self">The Task of Option to extract the value from.</param>
    /// <param name="defaultValueProvider">The function that provides the default value if the Option is None.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>A Task of the value from a Some Option or a value provided by the default value provider.</returns>
    public static async Task<TValue> UnwrapOrElse<TValue>(this Task<Option<TValue>> self, Func<TValue> defaultValueProvider)
        => (await self.ConfigureAwait(false)).UnwrapOrElse(defaultValueProvider);

    /// <summary>
    /// Asynchronously returns the value from a Some Option or a value provided by an asynchronous default value provider if the Option is None.
    /// </summary>
    /// <param name="self">The Option to extract the value from.</param>
    /// <param name="defaultValueProvider">The function that provides the default value if the Option is None.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>A Task of the value from a Some Option or a value provided by the default value provider.</returns>
    public static async Task<TValue> UnwrapOrElseAsync<TValue>(this Option<TValue> self, Func<Task<TValue>> defaultValueProvider)
        => self switch
        {
            { IsSome: true, Value: var value } => value,
            _ => await defaultValueProvider().ConfigureAwait(false)
        };

    /// <summary>
    /// Asynchronously returns the value from a Some Option or a value provided by an asynchronous default value provider if the Option is None.
    /// </summary>
    /// <param name="self">The Task of Option to extract the value from.</param>
    /// <param name="defaultValueProvider">The function that provides the default value if the Option is None.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>A Task of the value from a Some Option or a value provided by the default value provider.</returns>
    public static async Task<TValue> UnwrapOrElseAsync<TValue>(this Task<Option<TValue>> self, Func<Task<TValue>> defaultValueProvider)
        => await (await self.ConfigureAwait(false)).UnwrapOrElseAsync(defaultValueProvider).ConfigureAwait(false);
}