namespace Langutils.Core.Results;

public static partial class ResultExtensions
{
    /// <summary>
    /// Returns the second Result if the first Result is a success, otherwise returns the first Result.
    /// </summary>
    /// <param name="self">The first Result.</param>
    /// <param name="result">The second Result.</param>
    /// <typeparam name="TIn">The type of the value in the first Result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the second Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Results.</typeparam>
    /// <returns>The second Result if the first Result is a success, otherwise the first Result.</returns>
    public static Result<TOut, TError> And<TIn, TOut, TError>(this Result<TIn, TError> self, Result<TOut, TError> result)
        => self switch
        {
            { IsSuccess: true } => result,
            { Error: var error} => error!
        };

    /// <summary>
    /// Asynchronously returns the second Result if the first Result is a success, otherwise returns the first Result.
    /// </summary>
    /// <param name="self">The Task of the first Result.</param>
    /// <param name="result">The second Result.</param>
    /// <typeparam name="TIn">The type of the value in the first Result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the second Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Results.</typeparam>
    /// <returns>A Task of the second Result if the first Result is a success, otherwise the first Result.</returns>
    public static async Task<Result<TOut, TError>> And<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Result<TOut, TError> result)
        => (await self.ConfigureAwait(false)).And(result);

    /// <summary>
    /// Returns the result of the provided function if the first Result is a success, otherwise returns the first Result.
    /// </summary>
    /// <param name="self">The first Result.</param>
    /// <param name="resultProvider">The function that produces the second Result.</param>
    /// <typeparam name="TIn">The type of the value in the first Result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the second Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Results.</typeparam>
    /// <returns>The result of the provided function if the first Result is a success, otherwise the first Result.</returns>
    public static Result<TOut, TError> AndThen<TIn, TOut, TError>(this Result<TIn, TError> self, Func<TIn, Result<TOut, TError>> resultProvider)
        => self switch
        {
            { IsSuccess: true, Value: var value } => resultProvider(value),
            { Error: var error } => error!
        };

    /// <summary>
    /// Asynchronously returns the result of the provided function if the first Result is a success, otherwise returns the first Result.
    /// </summary>
    /// <param name="self">The Task of the first Result.</param>
    /// <param name="resultProvider">The function that produces the second Result.</param>
    /// <typeparam name="TIn">The type of the value in the first Result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the second Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Results.</typeparam>
    /// <returns>A Task of the result of the provided function if the first Result is a success, otherwise the first Result.</returns>
    public static async Task<Result<TOut, TError>> AndThen<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TIn, Result<TOut, TError>> resultProvider)
        => (await self.ConfigureAwait(false)).AndThen(resultProvider);

    /// <summary>
    /// Returns the result of the provided asynchronous function if the first Result is a success, otherwise returns the first Result.
    /// </summary>
    /// <param name="self">The first Result.</param>
    /// <param name="resultProvider">The asynchronous function that produces the second Result.</param>
    /// <typeparam name="TIn">The type of the value in the first Result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the second Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Results.</typeparam>
    /// <returns>A Task of the result of the provided asynchronous function if the first Result is a success, otherwise the first Result.</returns>
    public static async Task<Result<TOut, TError>> AndThenAsync<TIn, TOut, TError>(this Result<TIn, TError> self, Func<TIn, Task<Result<TOut, TError>>> resultProvider)
        => self switch
        {
            { IsSuccess: true, Value: var value} => await resultProvider(value).ConfigureAwait(false),
            { Error: var error } => error!
        };

    /// <summary>
    /// Asynchronously returns the result of the provided asynchronous function if the first Result is a success, otherwise returns the first Result.
    /// </summary>
    /// <param name="self">The Task of the first Result.</param>
    /// <param name="resultProvider">The asynchronous function that produces the second Result.</param>
    /// <typeparam name="TIn">The type of the value in the first Result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the second Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Results.</typeparam>
    /// <returns>A Task of the result of the provided asynchronous function if the first Result is a success, otherwise the first Result.</returns>
    public static async Task<Result<TOut, TError>> AndThenAsync<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TIn, Task<Result<TOut, TError>>> resultProvider)
        => await (await self.ConfigureAwait(false)).AndThenAsync(resultProvider).ConfigureAwait(false);

    /// <summary>
    /// Returns the first Result if it is a success, otherwise returns the second Result.
    /// </summary>
    /// <param name="self">The first Result.</param>
    /// <param name="result">The second Result.</param>
    /// <typeparam name="TValue">The type of the value in the Results.</typeparam>
    /// <typeparam name="TIn">The type of the error in the first Result.</typeparam>
    /// <typeparam name="TOut">The type of the error in the second Result.</typeparam>
    /// <returns>The first Result if it is a success, otherwise the second Result.</returns>
    public static Result<TValue, TOut> Or<TValue, TIn, TOut>(this Result<TValue, TIn> self, Result<TValue, TOut> result)
        => self switch
        {
            { IsSuccess: true, Value: var value } => value,
            _ => result
        };

    /// <summary>
    /// Asynchronously returns the first Result if it is a success, otherwise returns the second Result.
    /// </summary>
    /// <param name="self">The Task of the first Result.</param>
    /// <param name="result">The second Result.</param>
    /// <typeparam name="TValue">The type of the value in the Results.</typeparam>
    /// <typeparam name="TIn">The type of the error in the first Result.</typeparam>
    /// <typeparam name="TOut">The type of the error in the second Result.</typeparam>
    /// <returns>A Task of the first Result if it is a success, otherwise the second Result.</returns>
    public static async Task<Result<TValue, TOut>> Or<TValue, TIn, TOut>(this Task<Result<TValue, TIn>> self, Result<TValue, TOut> result)
        => (await self.ConfigureAwait(false)).Or(result);

    /// <summary>
    /// Returns the first Result if it is a success, otherwise returns the result of the provided function.
    /// </summary>
    /// <param name="self">The first Result.</param>
    /// <param name="resultProvider">The function that produces the second Result.</param>
    /// <typeparam name="TValue">The type of the value in the Results.</typeparam>
    /// <typeparam name="TIn">The type of the error in the first Result.</typeparam>
    /// <typeparam name="TOut">The type of the error in the second Result.</typeparam>
    /// <returns>The first Result if it is a success, otherwise the result of the provided function.</returns>
    public static Result<TValue, TOut> OrElse<TValue, TIn, TOut>(this Result<TValue, TIn> self, Func<TIn?, Result<TValue, TOut>> resultProvider)
        => self switch
        {
            { IsSuccess: true, Value: var value } => value,
            { Error: var error } => resultProvider(error)
        };

    /// <summary>
    /// Asynchronously returns the first Result if it is a success, otherwise returns the result of the provided function.
    /// </summary>
    /// <param name="self">The Task of the first Result.</param>
    /// <param name="resultProvider">The function that produces the second Result.</param>
    /// <typeparam name="TValue">The type of the value in the Results.</typeparam>
    /// <typeparam name="TIn">The type of the error in the first Result.</typeparam>
    /// <typeparam name="TOut">The type of the error in the second Result.</typeparam>
    /// <returns>A Task of the first Result if it is a success, otherwise the result of the provided function.</returns>
    public static async Task<Result<TValue, TOut>> OrElse<TValue, TIn, TOut>(this Task<Result<TValue, TIn>> self, Func<TIn?, Result<TValue, TOut>> resultProvider)
        => (await self.ConfigureAwait(false)).OrElse(resultProvider);

    /// <summary>
    /// Returns the first Result if it is a success, otherwise returns the result of the provided asynchronous function.
    /// </summary>
    /// <param name="self">The first Result.</param>
    /// <param name="resultProvider">The asynchronous function that produces the second Result.</param>
    /// <typeparam name="TValue">The type of the value in the Results.</typeparam>
    /// <typeparam name="TIn">The type of the error in the first Result.</typeparam>
    /// <typeparam name="TOut">The type of the error in the second Result.</typeparam>
    /// <returns>A Task of the first Result if it is a success, otherwise the result of the provided asynchronous function.</returns>
    public static async Task<Result<TValue, TOut>> OrElseAsync<TValue, TIn, TOut>(this Result<TValue, TIn> self, Func<TIn?, Task<Result<TValue, TOut>>> resultProvider)
        => self switch
        {
            { IsSuccess: true, Value: var value } => value,
            { Error: var error } => await resultProvider(error).ConfigureAwait(false)
        };

    /// <summary>
    /// Asynchronously returns the first Result if it is a success, otherwise returns the result of the provided asynchronous function.
    /// </summary>
    /// <param name="self">The Task of the first Result.</param>
    /// <param name="resultProvider">The asynchronous function that produces the second Result.</param>
    /// <typeparam name="TValue">The type of the value in the Results.</typeparam>
    /// <typeparam name="TIn">The type of the error in the first Result.</typeparam>
    /// <typeparam name="TOut">The type of the error in the second Result.</typeparam>
    /// <returns>A Task of the first Result if it is a success, otherwise the result of the provided asynchronous function.</returns>
    public static async Task<Result<TValue, TOut>> OrElseAsync<TValue, TIn, TOut>(this Task<Result<TValue, TIn>> self, Func<TIn?, Task<Result<TValue, TOut>>> resultProvider)
        => await (await self.ConfigureAwait(false)).OrElseAsync(resultProvider).ConfigureAwait(false);
}