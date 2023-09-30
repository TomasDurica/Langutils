namespace Langutils.Core.Results;

public static partial class ResultExtensions
{
    public static bool IsSuccessAnd<TValue, TError>(this Result<TValue, TError> self, Func<TValue, bool> predicate)
        => self switch
        {
            { IsSuccess: true, Value: var value } => predicate(value),
            _ => false
        };

    public static async Task<bool> IsSuccessAnd<TValue, TError>(this Task<Result<TValue, TError>> self, Func<TValue, bool> predicate)
        => (await self).IsSuccessAnd(predicate);

    public static async Task<bool> IsSuccessAndAsync<TValue, TError>(this Result<TValue, TError> self, Func<TValue, Task<bool>> predicate)
        => self switch
        {
            { IsSuccess: true, Value: var value } => await predicate(value).ConfigureAwait(false),
            _ => false
        };

    public static async Task<bool> IsSuccessAndAsync<TValue, TError>(this Task<Result<TValue, TError>> self, Func<TValue, Task<bool>> predicate)
        => await (await self).IsSuccessAndAsync(predicate).ConfigureAwait(false);

    public static bool IsErrorAnd<TValue, TError>(this Result<TValue, TError> self, Func<TError?, bool> predicate)
        => self switch
        {
            { IsError: true, Error: var error } => predicate(error),
            _ => false
        };

    public static async Task<bool> IsErrorAnd<TValue, TError>(this Task<Result<TValue, TError>> self, Func<TError?, bool> predicate)
        => (await self).IsErrorAnd(predicate);

    public static async Task<bool> IsErrorAndAsync<TValue, TError>(this Result<TValue, TError> self, Func<TError?, Task<bool>> predicate)
        => self switch
        {
            { IsError: true, Error: var error } => await predicate(error).ConfigureAwait(false),
            _ => false
        };

    public static async Task<bool> IsErrorAndAsync<TValue, TError>(this Task<Result<TValue, TError>> self, Func<TError?, Task<bool>> predicate)
        => await (await self).IsErrorAndAsync(predicate).ConfigureAwait(false);

    public static Result<TValue, TError> Tap<TValue, TError>(this Result<TValue, TError> self, Action<TValue> onSuccess)
    {
        if (self is { IsSuccess: true, Value: var value })
        {
            onSuccess(value);
        }

        return self;
    }

    public static async Task<Result<TValue, TError>> Tap<TValue, TError>(this Task<Result<TValue, TError>> self, Action<TValue> onSuccess)
        => (await self).Tap(onSuccess);

    public static async Task<Result<TValue, TError>> TapAsync<TValue, TError>(this Result<TValue, TError> self, Func<TValue, Task> onSuccess)
    {
        if (self is { IsSuccess: true, Value: var value })
        {
            await onSuccess(value).ConfigureAwait(false);
        }

        return self;
    }

    public static async Task<Result<TValue, TError>> TapAsync<TValue, TError>(this Task<Result<TValue, TError>> self, Func<TValue, Task> onSuccess)
        => await (await self).TapAsync(onSuccess).ConfigureAwait(false);

    public static Result<TValue, TError> TapError<TValue, TError>(this Result<TValue, TError> self, Action<TError?> onError)
    {
        if (self is { IsError: true, Error: var error })
        {
            onError(error);
        }

        return self;
    }

    public static async Task<Result<TValue, TError>> TapError<TValue, TError>(this Task<Result<TValue, TError>> self, Action<TError?> onError)
        => (await self).TapError(onError);

    public static async Task<Result<TValue, TError>> TapErrorAsync<TValue, TError>(this Result<TValue, TError> self, Func<TError?, Task> onError)
    {
        if (self is { IsError: true, Error: var error })
        {
            await onError(error).ConfigureAwait(false);
        }

        return self;
    }

    public static async Task<Result<TValue, TError>> TapErrorAsync<TValue, TError>(this Task<Result<TValue, TError>> self, Func<TError?, Task> onError)
        => await (await self).TapErrorAsync(onError).ConfigureAwait(false);
}