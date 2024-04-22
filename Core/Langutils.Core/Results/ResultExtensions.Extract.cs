using System.Diagnostics.CodeAnalysis;

namespace Langutils.Core.Results;

public static partial class ResultExtensions
{
    /// <summary>
    /// Returns the value from a successful Result or throws an InvalidOperationException if the Result is an error.
    /// </summary>
    /// <param name="self">The Result to extract the value from.</param>
    /// <param name="message">The message to include in the exception if the Result is an error.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>The value from a successful Result.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Result is an error.</exception>
    public static TValue Expect<TValue, TError>(this Result<TValue, TError> self, string message)
        => self switch
        {
            { IsSuccess: true, Value: var value } => value,
            _ => throw new InvalidOperationException(message)
        };

    /// <summary>
    /// Asynchronously returns the value from a successful Result or throws an InvalidOperationException if the Result is an error.
    /// </summary>
    /// <param name="self">The Task of Result to extract the value from.</param>
    /// <param name="message">The message to include in the exception if the Result is an error.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of the value from a successful Result.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Result is an error.</exception>
    public static async Task<TValue> Expect<TValue, TError>(this Task<Result<TValue, TError>> self, string message)
        => (await self.ConfigureAwait(false)).Expect(message);

    /// <summary>
    /// Tries to extract the value from a successful Result.
    /// </summary>
    /// <param name="self">The Result to extract the value from.</param>
    /// <param name="value">The extracted value if the Result is a success, otherwise default.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>True if the Result is a success and the value was extracted, otherwise false.</returns>
    public static bool TryUnwrap<TValue, TError>(this Result<TValue, TError> self, [NotNullWhen(true)] out TValue? value)
    {
        if (self is { IsSuccess: true, Value: var innerValue })
        {
            value = innerValue;
            return true;
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Extracts the value from a successful Result or throws an InvalidOperationException if the Result is an error.
    /// </summary>
    /// <param name="self">The Result to extract the value from.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>The value from a successful Result.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Result is an error.</exception>
    public static TValue Unwrap<TValue, TError>(this Result<TValue, TError> self)
        => self switch
        {
            { IsSuccess: true, Value: var value } => value,
            { Error: var error } => throw new InvalidOperationException($"Called `{nameof(Unwrap)}()` on a `{nameof(Result)}.{nameof(Result.Error)}` value: {error}")
        };

    /// <summary>
    /// Asynchronously extracts the value from a successful Result or throws an InvalidOperationException if the Result is an error.
    /// </summary>
    /// <param name="self">The Task of Result to extract the value from.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>The value from a successful Result.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Result is an error.</exception>
    public static async Task<TValue> Unwrap<TValue, TError>(this Task<Result<TValue, TError>> self)
        => (await self.ConfigureAwait(false)).Unwrap();

    /// <summary>
    /// Returns the value from a successful Result or the provided default value if the Result is an error.
    /// </summary>
    /// <param name="self">The Result to extract the value from.</param>
    /// <param name="defaultValue">The default value to return if the Result is an error.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>The value from a successful Result or the provided default value.</returns>
    public static TValue UnwrapOr<TValue, TError>(this Result<TValue, TError> self, TValue defaultValue)
        => self switch
        {
            { IsSuccess: true, Value: var value } => value,
            _ => defaultValue
        };

    /// <summary>
    /// Asynchronously returns the value from a successful Result or the provided default value if the Result is an error.
    /// </summary>
    /// <param name="self">The Task of Result to extract the value from.</param>
    /// <param name="defaultValue">The default value to return if the Result is an error.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of the value from a successful Result or the provided default value.</returns>
    public static async Task<TValue> UnwrapOr<TValue, TError>(this Task<Result<TValue, TError>> self, TValue defaultValue)
        => (await self.ConfigureAwait(false)).UnwrapOr(defaultValue);

    /// <summary>
    /// Returns the value from a successful Result or the default value if the Result is an error.
    /// </summary>
    /// <param name="self">The Result to extract the value from.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>The value from a successful Result or the default value.</returns>
    public static TValue? UnwrapOrDefault<TValue, TError>(this Result<TValue, TError> self)
        => self switch
        {
            { IsSuccess: true, Value: var value } => value,
            _ => default
        };

    /// <summary>
    /// Asynchronously returns the value from a successful Result or the default value if the Result is an error.
    /// </summary>
    /// <param name="self">The Task of Result to extract the value from.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of the value from a successful Result or the default value.</returns>
    public static async Task<TValue?> UnwrapOrDefault<TValue, TError>(this Task<Result<TValue, TError>> self)
        => (await self.ConfigureAwait(false)).UnwrapOrDefault();

    /// <summary>
    /// Returns the value from a successful Result or the provided default value if the Result is an error.
    /// </summary>
    /// <param name="self">The Result to extract the value from.</param>
    /// <param name="defaultValueProvider">A function that provides the default value if the Result is an error.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>The value from a successful Result or the provided default value.</returns>
    public static TValue UnwrapOrElse<TValue, TError>(this Result<TValue, TError> self, Func<TError?, TValue> defaultValueProvider)
        => self switch
        {
            { IsSuccess: true, Value: var value } => value,
            { Error: var error } => defaultValueProvider(error)
        };

    /// <summary>
    /// Asynchronously returns the value from a successful Result or the provided default value if the Result is an error.
    /// </summary>
    /// <param name="self">The Task of Result to extract the value from.</param>
    /// <param name="defaultValueProvider">A function that provides the default value if the Result is an error.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of the value from a successful Result or the provided default value.</returns>
    public static async Task<TValue> UnwrapOrElse<TValue, TError>(this Task<Result<TValue, TError>> self, Func<TError?, TValue> defaultValueProvider)
        => (await self.ConfigureAwait(false)).UnwrapOrElse(defaultValueProvider);

    /// <summary>
    /// Asynchronously returns the value from a successful Result or the provided default value if the Result is an error.
    /// </summary>
    /// <param name="self">The Result to extract the value from.</param>
    /// <param name="defaultValueProvider">A function that provides the default value if the Result is an error.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of the value from a successful Result or the provided default value.</returns>
    public static async Task<TValue> UnwrapOrElseAsync<TValue, TError>(this Result<TValue, TError> self, Func<TError?, Task<TValue>> defaultValueProvider)
        => self switch
        {
            { IsSuccess: true, Value: var value } => value,
            { Error: var error } => await defaultValueProvider(error).ConfigureAwait(false)
        };

    /// <summary>
    /// Asynchronously returns the value from a successful Result or the provided default value if the Result is an error.
    /// </summary>
    /// <param name="self">The Task of Result to extract the value from.</param>
    /// <param name="defaultValueProvider">A function that provides the default value if the Result is an error.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of the value from a successful Result or the provided default value.</returns>
    public static async Task<TValue> UnwrapOrElseAsync<TValue, TError>(this Task<Result<TValue, TError>> self, Func<TError?, Task<TValue>> defaultValueProvider)
        => await (await self.ConfigureAwait(false)).UnwrapOrElseAsync(defaultValueProvider).ConfigureAwait(false);

    /// <summary>
    /// Returns the error from a failed Result or throws an InvalidOperationException if the Result is a success.
    /// </summary>
    /// <param name="self">The Result to extract the error from.</param>
    /// <param name="message">The message to include in the exception if the Result is a success.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>The error from a failed Result.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Result is a success.</exception>
    public static TError? ExpectError<TValue, TError>(this Result<TValue, TError> self, string message)
        => self switch
        {
            { IsError: true, Error: var error } => error,
            _ => throw new InvalidOperationException(message)
        };

    /// <summary>
    /// Asynchronously returns the error from a failed Result or throws an InvalidOperationException if the Result is a success.
    /// </summary>
    /// <param name="self">The Task of Result to extract the error from.</param>
    /// <param name="message">The message to include in the exception if the Result is a success.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of the error from a failed Result.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Result is a success.</exception>
    public static async Task<TError?> ExpectError<TValue, TError>(this Task<Result<TValue, TError>> self, string message)
        => (await self.ConfigureAwait(false)).ExpectError(message);

    /// <summary>
    /// Tries to extract the error from a failed Result.
    /// </summary>
    /// <param name="self">The Result to extract the error from.</param>
    /// <param name="error">The extracted error if the Result is a failure, otherwise default.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>True if the Result is a failure and the error was extracted, otherwise false.</returns>
    public static bool TryUnwrapError<TValue, TError>(this Result<TValue, TError> self, out TError? error)
    {
        if (self is { IsError: true, Error: var innerError })
        {
            error = innerError;
            return true;
        }

        error = default;
        return false;
    }

    /// <summary>
    /// Extracts the error from a failed Result or throws an InvalidOperationException if the Result is a success.
    /// </summary>
    /// <param name="self">The Result to extract the error from.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>The error from a failed Result.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Result is a success.</exception>
    public static TError? UnwrapError<TValue, TError>(this Result<TValue, TError> self)
        => self switch
        {
            { IsError: true, Error: var error } => error,
            { Value: var value } => throw new InvalidOperationException($"Called `{nameof(UnwrapError)}()` on a `{nameof(Result)}.{nameof(Result.Success)}` value: {value}")
        };

    /// <summary>
    /// Asynchronously returns the error from a failed Result or throws an InvalidOperationException if the Result is a success.
    /// </summary>
    /// <param name="self">The Task of Result to extract the error from.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of the error from a failed Result.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Result is a success.</exception>
    public static async Task<TError?> UnwrapError<TValue, TError>(this Task<Result<TValue, TError>> self)
        => (await self.ConfigureAwait(false)).UnwrapError();
}