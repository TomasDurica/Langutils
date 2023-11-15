using Langutils.Core.Options;

namespace Langutils.Core.Results;

public static partial class ResultExtensions
{
    /// <summary>
    /// Transforms the value in a successful Result using the provided function.
    /// </summary>
    /// <param name="self">The Result to transform.</param>
    /// <param name="selector">The function to transform the value of the Result.</param>
    /// <typeparam name="TIn">The type of the value in the original Result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the transformed Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Result containing the transformed value if the original Result is a success, otherwise the original Result.</returns>
    public static Result<TOut, TError> Map<TIn, TOut, TError>(this Result<TIn, TError> self, Func<TIn, TOut> selector)
        => self switch
        {
            { IsSuccess: true, Value: var value } => selector(value),
            { Error: var error } => error!
        };

    /// <summary>
    /// Asynchronously transforms the value in a successful Result using the provided function.
    /// </summary>
    /// <param name="self">The Task of Result to transform.</param>
    /// <param name="selector">The function to transform the value of the Result.</param>
    /// <typeparam name="TIn">The type of the value in the original Result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the transformed Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of a Result containing the transformed value if the original Result is a success, otherwise the original Result.</returns>
    public static async Task<Result<TOut, TError>> Map<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TIn, TOut> selector)
        => (await self.ConfigureAwait(false)).Map(selector);

    /// <summary>
    /// Transforms the value in a successful Result using the provided asynchronous function.
    /// </summary>
    /// <param name="self">The Result to transform.</param>
    /// <param name="selector">The asynchronous function to transform the value of the Result.</param>
    /// <typeparam name="TIn">The type of the value in the original Result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the transformed Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of a Result containing the transformed value if the original Result is a success, otherwise the original Result.</returns>
    public static async Task<Result<TOut, TError>> MapAsync<TIn, TOut, TError>(this Result<TIn, TError> self, Func<TIn, Task<TOut>> selector)
        => self switch
        {
            { IsSuccess: true, Value: var value } => await selector(value).ConfigureAwait(false),
            { Error: var error } => error!
        };

    /// <summary>
    /// Asynchronously transforms the value in a successful Result using the provided asynchronous function.
    /// </summary>
    /// <param name="self">The Task of Result to transform.</param>
    /// <param name="selector">The asynchronous function to transform the value of the Result.</param>
    /// <typeparam name="TIn">The type of the value in the original Result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the transformed Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of a Result containing the transformed value if the original Result is a success, otherwise the original Result.</returns>
    public static async Task<Result<TOut, TError>> MapAsync<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TIn, Task<TOut>> selector)
        => await (await self.ConfigureAwait(false)).MapAsync(selector).ConfigureAwait(false);

    /// <summary>
    /// Transforms the error in a failed Result using the provided function.
    /// </summary>
    /// <param name="self">The Result to transform.</param>
    /// <param name="selector">The function to transform the error of the Result.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TIn">The type of the error in the original Result.</typeparam>
    /// <typeparam name="TOut">The type of the error in the transformed Result.</typeparam>
    /// <returns>A Result containing the original value if the Result is a success, otherwise a Result with the transformed error.</returns>
    public static Result<TValue, TOut> MapError<TValue, TIn, TOut>(this Result<TValue, TIn> self, Func<TIn?, TOut> selector)
        => self switch
        {
            { IsError: true, Error: var error } => selector(error),
            { Value: var value } => value
        };

    /// <summary>
    /// Asynchronously transforms the error in a failed Result using the provided function.
    /// </summary>
    /// <param name="self">The Task of Result to transform.</param>
    /// <param name="selector">The function to transform the error of the Result.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TIn">The type of the error in the original Result.</typeparam>
    /// <typeparam name="TOut">The type of the error in the transformed Result.</typeparam>
    /// <returns>A Task of a Result containing the original value if the Result is a success, otherwise a Result with the transformed error.</returns>
    public static async Task<Result<TValue, TOut>> MapError<TValue, TIn, TOut>(this Task<Result<TValue, TIn>> self, Func<TIn?, TOut> selector)
        => (await self.ConfigureAwait(false)).MapError(selector);

    /// <summary>
    /// Transforms the error in a failed Result using the provided asynchronous function.
    /// </summary>
    /// <param name="self">The Result to transform.</param>
    /// <param name="selector">The asynchronous function to transform the error of the Result.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TIn">The type of the error in the original Result.</typeparam>
    /// <typeparam name="TOut">The type of the error in the transformed Result.</typeparam>
    /// <returns>A Task of a Result containing the original value if the Result is a success, otherwise a Result with the transformed error.</returns>
    public static async Task<Result<TValue, TOut>> MapErrorAsync<TValue, TIn, TOut>(this Result<TValue, TIn> self, Func<TIn?, Task<TOut>> selector)
        => self switch
        {
            { IsError: true, Error: var error } => await selector(error).ConfigureAwait(false),
            { Value: var value } => value
        };

    /// <summary>
    /// Asynchronously transforms the error in a failed Result using the provided asynchronous function.
    /// </summary>
    /// <param name="self">The Task of Result to transform.</param>
    /// <param name="selector">The asynchronous function to transform the error of the Result.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TIn">The type of the error in the original Result.</typeparam>
    /// <typeparam name="TOut">The type of the error in the transformed Result.</typeparam>
    /// <returns>A Task of a Result containing the original value if the Result is a success, otherwise a Result with the transformed error.</returns>
    public static async Task<Result<TValue, TOut>> MapErrorAsync<TValue, TIn, TOut>(this Task<Result<TValue, TIn>> self, Func<TIn?, Task<TOut>> selector)
        => await (await self.ConfigureAwait(false)).MapErrorAsync(selector).ConfigureAwait(false);

    /// <summary>
    /// Transforms the value in a successful Result using the provided function, or returns a default value if the Result is a failure.
    /// </summary>
    /// <param name="self">The Result to transform.</param>
    /// <param name="defaultValue">The default value to return if the Result is a failure.</param>
    /// <param name="selector">The function to transform the value of the Result.</param>
    /// <typeparam name="TIn">The type of the value in the original Result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the transformed Result or the default value.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A value of type TOut which is either the transformed value if the original Result is a success, or the default value if the Result is a failure.</returns>
    public static TOut MapOr<TIn, TOut, TError>(this Result<TIn, TError> self, TOut defaultValue, Func<TIn, TOut> selector)
        => self switch
        {
            { IsSuccess: true, Value: var value } => selector(value),
            _ => defaultValue
        };

    /// <summary>
    /// Asynchronously transforms the value in a successful Result using the provided function, or returns a default value if the Result is a failure.
    /// </summary>
    /// <param name="self">The Task of Result to transform.</param>
    /// <param name="defaultValue">The default value to return if the Result is a failure.</param>
    /// <param name="selector">The function to transform the value of the Result.</param>
    /// <typeparam name="TIn">The type of the value in the original Result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the transformed Result or the default value.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of a value of type TOut which is either the transformed value if the original Result is a success, or the default value if the Result is a failure.</returns>
    public static async Task<TOut> MapOr<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, TOut defaultValue, Func<TIn, TOut> selector)
        => (await self.ConfigureAwait(false)).MapOr(defaultValue, selector);

    /// <summary>
    /// Transforms the value in a successful Result using the provided asynchronous function, or returns a default value if the Result is a failure.
    /// </summary>
    /// <param name="self">The Result to transform.</param>
    /// <param name="defaultValue">The default value to return if the Result is a failure.</param>
    /// <param name="selector">The asynchronous function to transform the value of the Result.</param>
    /// <typeparam name="TIn">The type of the value in the original Result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the transformed Result or the default value.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of a value of type TOut which is either the transformed value if the original Result is a success, or the default value if the Result is a failure.</returns>
    public static async Task<TOut> MapOrAsync<TIn, TOut, TError>(this Result<TIn, TError> self, TOut defaultValue, Func<TIn, Task<TOut>> selector)
        => self switch
        {
            { IsSuccess: true, Value: var value } => await selector(value).ConfigureAwait(false),
            _ => defaultValue
        };

    /// <summary>
    /// Asynchronously transforms the value in a successful Result using the provided asynchronous function, or returns a default value if the Result is a failure.
    /// </summary>
    /// <param name="self">The Task of Result to transform.</param>
    /// <param name="defaultValue">The default value to return if the Result is a failure.</param>
    /// <param name="selector">The asynchronous function to transform the value of the Result.</param>
    /// <typeparam name="TIn">The type of the value in the original Result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the transformed Result or the default value.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of a value of type TOut which is either the transformed value if the original Result is a success, or the default value if the Result is a failure.</returns>
    public static async Task<TOut> MapOrAsync<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, TOut defaultValue, Func<TIn, Task<TOut>> selector)
        => await (await self.ConfigureAwait(false)).MapOrAsync(defaultValue, selector).ConfigureAwait(false);

    /// <summary>
    /// Transforms the value in a successful Result using the provided function, or returns a default value if the Result is a failure.
    /// </summary>
    /// <param name="self">The Result to transform.</param>
    /// <param name="defaultValueProvider">A function that provides the default value if the Result is a failure.</param>
    /// <param name="selector">The function to transform the value of the Result.</param>
    /// <typeparam name="TIn">The type of the value in the original Result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the transformed Result or the default value.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A value of type TOut which is either the transformed value if the original Result is a success, or the default value if the Result is a failure.</returns>
    public static TOut MapOrElse<TIn, TOut, TError>(this Result<TIn, TError> self, Func<TError?, TOut> defaultValueProvider, Func<TIn, TOut> selector)
        => self switch
        {
            { IsSuccess: true, Value: var value } => selector(value),
            { Error: var error } _ => defaultValueProvider(error)
        };

    /// <summary>
    /// Asynchronously transforms the value in a successful Result using the provided function, or returns a default value if the Result is a failure.
    /// </summary>
    /// <param name="self">The Task of Result to transform.</param>
    /// <param name="defaultValueProvider">A function that provides the default value if the Result is a failure.</param>
    /// <param name="selector">The function to transform the value of the Result.</param>
    /// <typeparam name="TIn">The type of the value in the original Result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the transformed Result or the default value.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of a value of type TOut which is either the transformed value if the original Result is a success, or the default value if the Result is a failure.</returns>
    public static async Task<TOut> MapOrElse<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TError?, TOut> defaultValueProvider, Func<TIn, TOut> selector)
        => (await self.ConfigureAwait(false)).MapOrElse(defaultValueProvider, selector);

    /// <summary>
    /// Transforms the value in a successful Result using the provided asynchronous function, or returns a default value if the Result is a failure.
    /// </summary>
    /// <param name="self">The Result to transform.</param>
    /// <param name="defaultValueProvider">A function that provides the default value if the Result is a failure.</param>
    /// <param name="selector">The asynchronous function to transform the value of the Result.</param>
    /// <typeparam name="TIn">The type of the value in the original Result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the transformed Result or the default value.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of a value of type TOut which is either the transformed value if the original Result is a success, or the default value if the Result is a failure.</returns>
    public static async Task<TOut> MapOrElseAsync<TIn, TOut, TError>(this Result<TIn, TError> self, Func<TError?, TOut> defaultValueProvider, Func<TIn, Task<TOut>> selector)
        => self switch
        {
            { IsSuccess: true, Value: var value } => await selector(value).ConfigureAwait(false),
            { Error: var error } => defaultValueProvider(error)
        };

    /// <summary>
    /// Asynchronously transforms the value in a successful Result using the provided asynchronous function, or returns a default value if the Result is a failure.
    /// </summary>
    /// <param name="self">The Task of Result to transform.</param>
    /// <param name="defaultValueProvider">A function that provides the default value if the Result is a failure.</param>
    /// <param name="selector">The asynchronous function to transform the value of the Result.</param>
    /// <typeparam name="TIn">The type of the value in the original Result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the transformed Result or the default value.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of a value of type TOut which is either the transformed value if the original Result is a success, or the default value if the Result is a failure.</returns>
    public static async Task<TOut> MapOrElseAsync<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TError?, TOut> defaultValueProvider, Func<TIn, Task<TOut>> selector)
        => await (await self.ConfigureAwait(false)).MapOrElseAsync(defaultValueProvider, selector).ConfigureAwait(false);

    /// <summary>
    /// Transforms the value in a successful Result using the provided function, or returns a default value if the Result is a failure.
    /// The default value is provided by a function that takes the error of the Result as a parameter.
    /// </summary>
    /// <param name="self">The Result to transform.</param>
    /// <param name="defaultValueProvider">A function that provides the default value if the Result is a failure.</param>
    /// <param name="selector">The asynchronous function to transform the value of the Result.</param>
    /// <typeparam name="TIn">The type of the value in the original Result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the transformed Result or the default value.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of a value of type TOut which is either the transformed value if the original Result is a success, or the default value if the Result is a failure.</returns>
    public static async Task<TOut> MapOrElseAsync<TIn, TOut, TError>(this Result<TIn, TError> self, Func<TError?, Task<TOut>> defaultValueProvider, Func<TIn, TOut> selector)
        => self switch
        {
            { IsSuccess: true, Value: var value } => selector(value),
            { Error: var error } => await defaultValueProvider(error).ConfigureAwait(false)
        };

    /// <summary>
    /// Asynchronously transforms the value in a successful Result using the provided asynchronous function, or returns a default value if the Result is a failure.
    /// The default value is provided by a function that takes the error of the Result as a parameter.
    /// </summary>
    /// <param name="self">The Task of Result to transform.</param>
    /// <param name="defaultValueProvider">A function that provides the default value if the Result is a failure.</param>
    /// <param name="selector">The asynchronous function to transform the value of the Result.</param>
    /// <typeparam name="TIn">The type of the value in the original Result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the transformed Result or the default value.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of a value of type TOut which is either the transformed value if the original Result is a success, or the default value if the Result is a failure.</returns>
    public static async Task<TOut> MapOrElseAsync<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TError?, Task<TOut>> defaultValueProvider, Func<TIn, TOut> selector)
        => await (await self.ConfigureAwait(false)).MapOrElseAsync(defaultValueProvider, selector).ConfigureAwait(false);

    /// <summary>
    /// Transforms the value in a successful Result using the provided asynchronous function, or returns a default value if the Result is a failure.
    /// The default value is provided by a function that takes the error of the Result as a parameter.
    /// </summary>
    /// <param name="self">The Result to transform.</param>
    /// <param name="defaultValueProvider">A function that provides the default value if the Result is a failure.</param>
    /// <param name="selector">The asynchronous function to transform the value of the Result.</param>
    /// <typeparam name="TIn">The type of the value in the original Result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the transformed Result or the default value.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of a value of type TOut which is either the transformed value if the original Result is a success, or the default value if the Result is a failure.</returns>
    public static async Task<TOut> MapOrElseAsync<TIn, TOut, TError>(this Result<TIn, TError> self, Func<TError?, Task<TOut>> defaultValueProvider, Func<TIn, Task<TOut>> selector)
        => self switch
        {
            { IsSuccess: true, Value: var value } => await selector(value).ConfigureAwait(false),
            { Error: var error } => await defaultValueProvider(error).ConfigureAwait(false)
        };

    /// <summary>
    /// Asynchronously transforms the value in a successful Result using the provided asynchronous function, or returns a default value if the Result is a failure.
    /// The default value is provided by a function that takes the error of the Result as a parameter.
    /// </summary>
    /// <param name="self">The Task of Result to transform.</param>
    /// <param name="defaultValueProvider">A function that provides the default value if the Result is a failure.</param>
    /// <param name="selector">The asynchronous function to transform the value of the Result.</param>
    /// <typeparam name="TIn">The type of the value in the original Result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the transformed Result or the default value.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of a value of type TOut which is either the transformed value if the original Result is a success, or the default value if the Result is a failure.</returns>
    public static async Task<TOut> MapOrElseAsync<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TError?, Task<TOut>> defaultValueProvider, Func<TIn, Task<TOut>> selector)
        => await (await self.ConfigureAwait(false)).MapOrElseAsync(defaultValueProvider, selector).ConfigureAwait(false);

    /// <summary>
    /// Converts a successful Result into an Option containing the value of the Result, or an empty Option if the Result is a failure.
    /// </summary>
    /// <param name="self">The Result to convert.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>An Option containing the value if the Result is a success, or an empty Option if the Result is a failure.</returns>
    public static Option<TValue> Success<TValue, TError>(this Result<TValue, TError> self)
        => self switch
        {
            { IsSuccess: true, Value: var value } => value,
            _ => None.Instance
        };

    /// <summary>
    /// Asynchronously converts a successful Result into an Option containing the value of the Result, or an empty Option if the Result is a failure.
    /// </summary>
    /// <param name="self">The Task of Result to convert.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of an Option containing the value if the Result is a success, or an empty Option if the Result is a failure.</returns>
    public static async Task<Option<TValue>> Success<TValue, TError>(this Task<Result<TValue, TError>> self)
        => (await self.ConfigureAwait(false)).Success();

    /// <summary>
    /// Converts a failed Result into an Option containing the error of the Result, or an empty Option if the Result is a success.
    /// </summary>
    /// <param name="self">The Result to convert.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>An Option containing the error if the Result is a failure, or an empty Option if the Result is a success.</returns>
    public static Option<TError> Error<TValue, TError>(this Result<TValue, TError> self)
        => self switch
        {
            { IsError: true, Error: var error } => error,
            _ => None.Instance
        };

    /// <summary>
    /// Asynchronously converts a failed Result into an Option containing the error of the Result, or an empty Option if the Result is a success.
    /// </summary>
    /// <param name="self">The Task of Result to convert.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of an Option containing the error if the Result is a failure, or an empty Option if the Result is a success.</returns>
    public static async Task<Option<TError>> Error<TValue, TError>(this Task<Result<TValue, TError>> self)
        => (await self.ConfigureAwait(false)).Error();

    /// <summary>
    /// Transposes a Result of an Option into an Option of a Result.
    /// </summary>
    /// <param name="self">The Result of an Option to transpose.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>An Option containing a successful Result if the original Result is a success and the Option is Some, an empty Option if the original Result is a success and the Option is None, or a Result with an error if the original Result is a failure.</returns>
    public static Option<Result<TValue, TError>> Transpose<TValue, TError>(this Result<Option<TValue>, TError> self)
        => self switch
        {
            { IsSuccess: true, Value.IsSome: false } => None.Instance,
            { IsSuccess: true, Value: { IsSome: true, Value: var value }} => Result.Success<TValue, TError>(value),
            { Error: var error } => Result.Error<TValue, TError>(error!)
        };

    /// <summary>
    /// Asynchronously transposes a Result of an Option into an Option of a Result.
    /// </summary>
    /// <param name="self">The Task of Result of an Option to transpose.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of an Option containing a successful Result if the original Result is a success and the Option is Some, an empty Option if the original Result is a success and the Option is None, or a Result with an error if the original Result is a failure.</returns>
    public static async Task<Option<Result<TValue, TError>>> Transpose<TValue, TError>(this Task<Result<Option<TValue>, TError>> self)
        => (await self.ConfigureAwait(false)).Transpose();

    /// <summary>
    /// Flattens a Result of a Result into a single Result.
    /// </summary>
    /// <param name="self">The Result of a Result to flatten.</param>
    /// <typeparam name="TValue">The type of the value in the inner Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Results.</typeparam>
    /// <returns>The inner Result if the outer Result is a success, or a Result with an error if the outer Result is a failure.</returns>
    public static Result<TValue, TError> Flatten<TValue, TError>(this Result<Result<TValue, TError>, TError> self)
        => self switch
        {
            { IsSuccess: true, Value: var option} => option,
            { Error: var error } => error!
        };

    /// <summary>
    /// Asynchronously flattens a Result of a Result into a single Result.
    /// </summary>
    /// <param name="self">The Task of Result of a Result to flatten.</param>
    /// <typeparam name="TValue">The type of the value in the inner Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Results.</typeparam>
    /// <returns>A Task of the inner Result if the outer Result is a success, or a Result with an error if the outer Result is a failure.</returns>
    public static async Task<Result<TValue, TError>> Flatten<TValue, TError>(this Task<Result<Result<TValue, TError>, TError>> self)
        => (await self.ConfigureAwait(false)).Flatten();
}