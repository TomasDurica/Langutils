namespace Langutils.Core.Results;

public static partial class ResultExtensions
{
    /// <summary>
    /// Checks if the Result is a success and if the provided predicate returns true for the value of the Result.
    /// </summary>
    /// <param name="self">The Result to check.</param>
    /// <param name="predicate">The predicate to apply to the value of the Result.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>True if the Result is a success and the predicate returns true for its value, otherwise false.</returns>
    public static bool IsSuccessAnd<TValue, TError>(this Result<TValue, TError> self, Func<TValue, bool> predicate)
        => self switch
        {
            { IsSuccess: true, Value: var value } => predicate(value),
            _ => false
        };

    /// <summary>
    /// Asynchronously checks if the Result is a success and if the provided predicate returns true for the value of the Result.
    /// </summary>
    /// <param name="self">The Task of Result to check.</param>
    /// <param name="predicate">The predicate to apply to the value of the Result.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of bool representing if the Result is a success and the predicate returns true for its value.</returns>
    public static async Task<bool> IsSuccessAnd<TValue, TError>(this Task<Result<TValue, TError>> self, Func<TValue, bool> predicate)
        => (await self.ConfigureAwait(false)).IsSuccessAnd(predicate);

    /// <summary>
    /// Checks if the Result is a success and if the provided asynchronous predicate returns true for the value of the Result.
    /// </summary>
    /// <param name="self">The Result to check.</param>
    /// <param name="predicate">The asynchronous predicate to apply to the value of the Result.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of bool representing if the Result is a success and the predicate returns true for its value.</returns>
    public static async Task<bool> IsSuccessAndAsync<TValue, TError>(this Result<TValue, TError> self, Func<TValue, Task<bool>> predicate)
        => self switch
        {
            { IsSuccess: true, Value: var value } => await predicate(value).ConfigureAwait(false),
            _ => false
        };

    /// <summary>
    /// Asynchronously checks if the Result is a success and if the provided asynchronous predicate returns true for the value of the Result.
    /// </summary>
    /// <param name="self">The Task of Result to check.</param>
    /// <param name="predicate">The asynchronous predicate to apply to the value of the Result.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of bool representing if the Result is a success and the predicate returns true for its value.</returns>
    public static async Task<bool> IsSuccessAndAsync<TValue, TError>(this Task<Result<TValue, TError>> self, Func<TValue, Task<bool>> predicate)
        => await (await self.ConfigureAwait(false)).IsSuccessAndAsync(predicate).ConfigureAwait(false);

    /// <summary>
    /// Checks if the Result is a failure and if the provided predicate returns true for the error of the Result.
    /// </summary>
    /// <param name="self">The Result to check.</param>
    /// <param name="predicate">The predicate to apply to the error of the Result.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>True if the Result is a failure and the predicate returns true for its error, otherwise false.</returns>
    public static bool IsErrorAnd<TValue, TError>(this Result<TValue, TError> self, Func<TError?, bool> predicate)
        => self switch
        {
            { IsError: true, Error: var error } => predicate(error),
            _ => false
        };

    /// <summary>
    /// Asynchronously checks if the Result is a failure and if the provided predicate returns true for the error of the Result.
    /// </summary>
    /// <param name="self">The Task of Result to check.</param>
    /// <param name="predicate">The predicate to apply to the error of the Result.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of bool representing if the Result is a failure and the predicate returns true for its error.</returns>
    public static async Task<bool> IsErrorAnd<TValue, TError>(this Task<Result<TValue, TError>> self, Func<TError?, bool> predicate)
        => (await self.ConfigureAwait(false)).IsErrorAnd(predicate);

    /// <summary>
    /// Checks if the Result is a failure and if the provided asynchronous predicate returns true for the error of the Result.
    /// </summary>
    /// <param name="self">The Result to check.</param>
    /// <param name="predicate">The asynchronous predicate to apply to the error of the Result.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of bool representing if the Result is a failure and the predicate returns true for its error.</returns>
    public static async Task<bool> IsErrorAndAsync<TValue, TError>(this Result<TValue, TError> self, Func<TError?, Task<bool>> predicate)
        => self switch
        {
            { IsError: true, Error: var error } => await predicate(error).ConfigureAwait(false),
            _ => false
        };

    /// <summary>
    /// Asynchronously checks if the Result is a failure and if the provided asynchronous predicate returns true for the error of the Result.
    /// </summary>
    /// <param name="self">The Task of Result to check.</param>
    /// <param name="predicate">The asynchronous predicate to apply to the error of the Result.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of bool representing if the Result is a failure and the predicate returns true for its error.</returns>
    public static async Task<bool> IsErrorAndAsync<TValue, TError>(this Task<Result<TValue, TError>> self, Func<TError?, Task<bool>> predicate)
        => await (await self.ConfigureAwait(false)).IsErrorAndAsync(predicate).ConfigureAwait(false);

    /// <summary>
    /// Performs the provided action on the value of a successful Result.
    /// </summary>
    /// <param name="self">The Task of Result to perform the action on.</param>
    /// <param name="onSuccess">The action to perform on the value of the Result.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of the original Result.</returns>
    public static Result<TValue, TError> Tap<TValue, TError>(this Result<TValue, TError> self, Action<TValue> onSuccess)
    {
        if (self is { IsSuccess: true, Value: var value })
        {
            onSuccess(value);
        }

        return self;
    }

    /// <summary>
    /// Asynchronously performs the provided action on the value of a successful Result.
    /// </summary>
    /// <param name="self">The Task of Result to perform the action on.</param>
    /// <param name="onSuccess">The action to perform on the value of the Result.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of the original Result.</returns>
    public static async Task<Result<TValue, TError>> Tap<TValue, TError>(this Task<Result<TValue, TError>> self, Action<TValue> onSuccess)
        => (await self.ConfigureAwait(false)).Tap(onSuccess);

    /// <summary>
    /// Performs the provided asynchronous action on the value of a successful Result.
    /// </summary>
    /// <param name="self">The Result to perform the action on.</param>
    /// <param name="onSuccess">The asynchronous action to perform on the value of the Result.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of the original Result.</returns>
    public static async Task<Result<TValue, TError>> TapAsync<TValue, TError>(this Result<TValue, TError> self, Func<TValue, Task> onSuccess)
    {
        if (self is { IsSuccess: true, Value: var value })
        {
            await onSuccess(value).ConfigureAwait(false);
        }

        return self;
    }

    /// <summary>
    /// Asynchronously performs the provided asynchronous action on the value of a successful Result.
    /// </summary>
    /// <param name="self">The Task of Result to perform the action on.</param>
    /// <param name="onSuccess">The asynchronous action to perform on the value of the Result.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of the original Result.</returns>
    public static async Task<Result<TValue, TError>> TapAsync<TValue, TError>(this Task<Result<TValue, TError>> self, Func<TValue, Task> onSuccess)
        => await (await self.ConfigureAwait(false)).TapAsync(onSuccess).ConfigureAwait(false);

    /// <summary>
    /// Performs the provided action on the error of a failed Result.
    /// </summary>
    /// <param name="self">The Result to perform the action on.</param>
    /// <param name="onError">The action to perform on the error of the Result.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>The original Result.</returns>
    public static Result<TValue, TError> TapError<TValue, TError>(this Result<TValue, TError> self, Action<TError?> onError)
    {
        if (self is { IsError: true, Error: var error })
        {
            onError(error);
        }

        return self;
    }

    /// <summary>
    /// Asynchronously performs the provided action on the error of a failed Result.
    /// </summary>
    /// <param name="self">The Task of Result to perform the action on.</param>
    /// <param name="onError">The action to perform on the error of the Result.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of the original Result.</returns>
    public static async Task<Result<TValue, TError>> TapError<TValue, TError>(this Task<Result<TValue, TError>> self, Action<TError?> onError)
        => (await self.ConfigureAwait(false)).TapError(onError);

    /// <summary>
    /// Performs the provided asynchronous action on the error of a failed Result.
    /// </summary>
    /// <param name="self">The Result to perform the action on.</param>
    /// <param name="onError">The asynchronous action to perform on the error of the Result.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of the original Result.</returns>
    public static async Task<Result<TValue, TError>> TapErrorAsync<TValue, TError>(this Result<TValue, TError> self, Func<TError?, Task> onError)
    {
        if (self is { IsError: true, Error: var error })
        {
            await onError(error).ConfigureAwait(false);
        }

        return self;
    }

    /// <summary>
    /// Asynchronously performs the provided asynchronous action on the error of a failed Result.
    /// </summary>
    /// <param name="self">The Task of Result to perform the action on.</param>
    /// <param name="onError">The asynchronous action to perform on the error of the Result.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of the original Result.</returns>
    public static async Task<Result<TValue, TError>> TapErrorAsync<TValue, TError>(this Task<Result<TValue, TError>> self, Func<TError?, Task> onError)
        => await (await self.ConfigureAwait(false)).TapErrorAsync(onError).ConfigureAwait(false);
}