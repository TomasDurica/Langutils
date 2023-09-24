using System.Diagnostics.CodeAnalysis;
using Langutils.Core.Options;

namespace Langutils.Core.Results;

public static class ResultExtensions
{
    public static bool IsSuccessAnd<TValue, TError>(this Result<TValue, TError> self, Func<TValue, bool> predicate) => self switch
    {
        { IsSuccess: true, Value: var value } => predicate(value),
        _ => false
    };

    public static bool IsErrorAnd<TValue, TError>(this Result<TValue, TError> self, Func<TError?, bool> predicate) => self switch
    {
        { IsError: true, Error: var error } => predicate(error),
        _ => false
    };

    public static TValue Expect<TValue, TError>(this Result<TValue, TError> self, string message) => self switch
    {
        { IsSuccess: true, Value: var value } => value,
        _ => throw new InvalidOperationException(message)
    };

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

    public static TValue Unwrap<TValue, TError>(this Result<TValue, TError> self) => self switch
    {
        { IsSuccess: true, Value: var value } => value,
        _ => throw new InvalidOperationException($"Called `{nameof(Unwrap)}()` on a `{nameof(Result.Error)}` value")
    };

    public static TValue UnwrapOr<TValue, TError>(this Result<TValue, TError> self, TValue defaultValue) => self switch
    {
        { IsSuccess: true, Value: var value } => value,
        _ => defaultValue
    };

    public static TValue? UnwrapOrDefault<TValue, TError>(this Result<TValue, TError> self) => self switch
    {
        { IsSuccess: true, Value: var value } => value,
        _ => default
    };

    public static TValue UnwrapOrElse<TValue, TError>(this Result<TValue, TError> self, Func<TError?, TValue> defaultValueProvider) => self switch
    {
        { IsSuccess: true, Value: var value } => value,
        { Error: var error } => defaultValueProvider(error)
    };

    public static TError? ExpectError<TValue, TError>(this Result<TValue, TError> self, string message) => self switch
    {
        { IsError: true, Error: var error } => error,
        _ => throw new InvalidOperationException(message)
    };

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

    public static TError? UnwrapError<TValue, TError>(this Result<TValue, TError> self) => self switch
    {
        { IsError: true, Error: var error } => error,
        _ => throw new InvalidOperationException($"Called `{nameof(UnwrapError)}()` on a `{nameof(Result.Success)}` value")
    };

    public static Option<TValue> Success<TValue, TError>(this Result<TValue, TError> self) => self switch
    {
        { IsSuccess: true, Value: var value } => value,
        _ => None.Instance
    };

    public static Option<TError> Error<TValue, TError>(this Result<TValue, TError> self) => self switch
    {
        { IsError: true, Error: var error } => error,
        _ => None.Instance
    };

    public static Option<Result<TValue, TError>> Transpose<TValue, TError>(this Result<Option<TValue>, TError> self) => self switch
    {
        { IsSuccess: true, Value.IsSome: false } => None.Instance,
        { IsSuccess: true, Value: { IsSome: true, Value: var value }} => Result.Success<TValue, TError>(value),
        { Error: var error } => Result.Error<TValue, TError>(error!)
    };

    public static Result<TValue, TError> Tap<TValue, TError>(this Result<TValue, TError> self, Action<TValue> onSuccess)
    {
        if (self is { IsSuccess: true, Value: var value })
        {
            onSuccess(value);
        }

        return self;
    }

    public static Result<TValue, TError> TapError<TValue, TError>(this Result<TValue, TError> self, Action<TError?> onError)
    {
        if (self is { IsError: true, Error: var error })
        {
            onError(error);
        }

        return self;
    }

    public static Result<TOut, TError> SelectMany<TIn, TOut, TError>(this Result<TIn, TError> self, Func<TIn, Result<TOut, TError>> selector) => self switch
    {
        { IsSuccess: true, Value: var value } => selector(value),
        { Error: var error } => error!
    };

    public static Result<TValue, TError> Flatten<TValue, TError>(this Result<Result<TValue, TError>, TError> self) => self switch
    {
        { IsSuccess: true, Value: var option} => option,
        { Error: var error } => error!
    };

    public static Result<TOut, TError> Select<TIn, TOut, TError>(this Result<TIn, TError> self, Func<TIn, TOut> selector) => self switch
    {
        { IsSuccess: true, Value: var value } => selector(value),
        { Error: var error } => error!
    };

    public static Result<TValue, TOut> SelectError<TValue, TIn, TOut>(this Result<TValue, TIn> self, Func<TIn?, TOut> selector) => self switch
    {
        { IsError: true, Error: var error } => selector(error),
        { Value: var value } => value
    };

    public static TOut SelectOr<TIn, TOut, TError>(this Result<TIn, TError> self, TOut defaultValue, Func<TIn, TOut> selector) => self switch
    {
        { IsSuccess: true, Value: var value } => selector(value),
        _ => defaultValue
    };

    public static TOut SelectOrElse<TIn, TOut, TError>(this Result<TIn, TError> self, Func<TOut> defaultValueProvider, Func<TIn, TOut> selector) => self switch
    {
        { IsSuccess: true, Value: var value } => selector(value),
        _ => defaultValueProvider()
    };

    public static IEnumerable<TValue> AsEnumerable<TValue, TError>(this Result<TValue, TError> self) => self switch
    {
        { IsSuccess: true, Value: var value } => new [] { value },
        _ => Enumerable.Empty<TValue>()
    };

    public static List<TValue> ToList<TValue, TError>(this Result<TValue, TError> self) => self switch
    {
        { IsSuccess: true, Value: var value } => new List<TValue> { value },
        _ => new List<TValue>()
    };

    public static TValue[] ToArray<TValue, TError>(this Result<TValue, TError> self) => self switch
    {
        { IsSuccess: true, Value: var value } => new [] { value },
        _ => Array.Empty<TValue>()
    };

    public static Result<TValue, TError> And<TValue, TError>(this Result<TValue, TError> self, Result<TValue, TError> option) => self switch
    {
        { IsSuccess: true } => option,
        _ => self
    };

    public static Result<TValue, TError> AndThen<TValue, TError>(this Result<TValue, TError> self, Func<Result<TValue, TError>> resultProvider) => self switch
    {
        { IsSuccess: true } => resultProvider(),
        _ => self
    };

    public static Result<TValue, TError> Or<TValue, TError>(this Result<TValue, TError> self, Result<TValue, TError> option) => self switch
    {
        { IsSuccess: true } => self,
        _ => option
    };

    public static Result<TValue, TError> OrElse<TValue, TError>(this Result<TValue, TError> self, Func<Result<TValue, TError>> resultProvider) => self switch
    {
        { IsSuccess: true } => self,
        _ => resultProvider()
    };

    public static int CompareTo<TValue, TError>(this Result<TValue, TError> self, Result<TValue, TError> option)
        where TValue: IComparable<TValue> => (self, option) switch
    {
        ({ IsSuccess: false }, { IsSuccess: false }) => 0,
        ({ IsSuccess: false }, { IsSuccess: true }) => -1,
        ({ IsSuccess: true }, { IsSuccess: false }) => 1,
        ({ IsSuccess: true, Value: var left }, { IsSuccess: true, Value: var right }) => left!.CompareTo(right!)
    };

    public static Result<List<TValue>, TError> Collect<TValue, TError>(this List<Result<TValue, TError>> options)
    {
        var result = new List<TValue>(options.Count);

        foreach (var option in options)
        {
            switch (option)
            {
                case { IsSuccess: true, Value: var value }:
                    result.Add(value);
                    break;
                case { Error: var error}:
                    return error!;
            }
        }

        return result;
    }

    public static Result<TValue[], TError> Collect<TValue, TError>(this Result<TValue, TError>[] options)
    {
        var result = new List<TValue>(options.Length);

        foreach (var option in options)
        {
            switch (option)
            {
                case { IsSuccess: true, Value: var value }:
                    result.Add(value);
                    break;
                case { Error: var error}:
                    return error!;
            }
        }

        return result.ToArray();
    }

    public static Result<IEnumerable<TValue>, TError> Collect<TValue, TError>(this IEnumerable<Result<TValue, TError>> options)
    {
        var result = options.TryGetNonEnumeratedCount(out var count)
            ? new List<TValue>(count)
            : new List<TValue>();

        foreach (var option in options)
        {
            switch (option)
            {
                case { IsSuccess: true, Value: var value }:
                    result.Add(value);
                    break;
                case { Error: var error}:
                    return error!;
            }
        }

        return Result.Success<IEnumerable<TValue>, TError>(result.AsEnumerable());
    }
}