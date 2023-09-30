namespace Langutils.Core.Results;

public static partial class ResultExtensions
{
    public static Result<TOut, TError> And<TIn, TOut, TError>(this Result<TIn, TError> self, Result<TOut, TError> option)
        => self switch
        {
            { IsSuccess: true } => option,
            { Error: var error} => error!
        };

    public static async Task<Result<TOut, TError>> And<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Result<TOut, TError> option)
        => (await self).And(option);

    public static Result<TOut, TError> AndThen<TIn, TOut, TError>(this Result<TIn, TError> self, Func<TIn, Result<TOut, TError>> resultProvider)
        => self switch
        {
            { IsSuccess: true, Value: var value } => resultProvider(value),
            { Error: var error } => error!
        };

    public static async Task<Result<TOut, TError>> AndThen<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TIn, Result<TOut, TError>> optionProvider)
        => (await self).AndThen(optionProvider);

    public static async Task<Result<TOut, TError>> AndThenAsync<TIn, TOut, TError>(this Result<TIn, TError> self, Func<TIn, Task<Result<TOut, TError>>> optionProvider)
        => self switch
        {
            { IsSuccess: true, Value: var value} => await optionProvider(value).ConfigureAwait(false),
            { Error: var error } => error!
        };

    public static async Task<Result<TOut, TError>> AndThenAsync<TIn, TOut, TError>(this Task<Result<TIn, TError>> self, Func<TIn, Task<Result<TOut, TError>>> optionProvider)
        => await (await self).AndThenAsync(optionProvider).ConfigureAwait(false);

    public static Result<TValue, TOut> Or<TValue, TIn, TOut>(this Result<TValue, TIn> self, Result<TValue, TOut> option)
        => self switch
        {
            { IsSuccess: true, Value: var value } => value,
            _ => option
        };

    public static async Task<Result<TValue, TOut>> Or<TValue, TIn, TOut>(this Task<Result<TValue, TIn>> self, Result<TValue, TOut> option)
        => (await self).Or(option);

    public static Result<TValue, TOut> OrElse<TValue, TIn, TOut>(this Result<TValue, TIn> self, Func<TIn?, Result<TValue, TOut>> resultProvider)
        => self switch
        {
            { IsSuccess: true, Value: var value } => value,
            { Error: var error } => resultProvider(error)
        };

    public static async Task<Result<TValue, TOut>> OrElse<TValue, TIn, TOut>(this Task<Result<TValue, TIn>> self, Func<TIn?, Result<TValue, TOut>> optionProvider)
        => (await self).OrElse(optionProvider);

    public static async Task<Result<TValue, TOut>> OrElseAsync<TValue, TIn, TOut>(this Result<TValue, TIn> self, Func<TIn?, Task<Result<TValue, TOut>>> optionProvider)
        => self switch
        {
            { IsSuccess: true, Value: var value } => value,
            { Error: var error } => await optionProvider(error).ConfigureAwait(false)
        };

    public static async Task<Result<TValue, TOut>> OrElseAsync<TValue, TIn, TOut>(this Task<Result<TValue, TIn>> self, Func<TIn?, Task<Result<TValue, TOut>>> optionProvider)
        => await (await self).OrElseAsync(optionProvider).ConfigureAwait(false);
}