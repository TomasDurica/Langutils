namespace Langutils.Core.Results;

public static partial class ResultExtensions
{
    public static IEnumerable<TValue> AsEnumerable<TValue, TError>(this Result<TValue, TError> self) => self switch
    {
        { IsSuccess: true, Value: var value } => new [] { value },
        _ => Enumerable.Empty<TValue>()
    };

    public static async Task<IEnumerable<TValue>> AsEnumerable<TValue, TError>(this Task<Result<TValue, TError>> self)
        => (await self).AsEnumerable();

    public static List<TValue> ToList<TValue, TError>(this Result<TValue, TError> self) => self switch
    {
        { IsSuccess: true, Value: var value } => new List<TValue> { value },
        _ => new List<TValue>()
    };

    public static async Task<List<TValue>> ToList<TValue, TError>(this Task<Result<TValue, TError>> self)
        => (await self).ToList();

    public static TValue[] ToArray<TValue, TError>(this Result<TValue, TError> self) => self switch
    {
        { IsSuccess: true, Value: var value } => new [] { value },
        _ => Array.Empty<TValue>()
    };

    public static async Task<TValue[]> ToArray<TValue, TError>(this Task<Result<TValue, TError>> self)
        => (await self).ToArray();

    public static Result<List<TValue>, TError> Collect<TValue, TError>(this List<Result<TValue, TError>> results)
    {
        var collection = new List<TValue>(results.Count);

        foreach (var result in results)
        {
            switch (result)
            {
                case { IsSuccess: true, Value: var value }:
                    collection.Add(value);
                    break;
                case { Error: var error}:
                    return error!;
            }
        }

        return collection;
    }

    public static async Task<Result<List<TValue>, TError>> Collect<TValue, TError>(this Task<List<Result<TValue, TError>>> results)
        => (await results).Collect();

    public static Result<TValue[], TError> Collect<TValue, TError>(this Result<TValue, TError>[] results)
    {
        var collection = new List<TValue>(results.Length);

        foreach (var result in results)
        {
            switch (result)
            {
                case { IsSuccess: true, Value: var value }:
                    collection.Add(value);
                    break;
                case { Error: var error}:
                    return error!;
            }
        }

        return collection.ToArray();
    }

    public static async Task<Result<TValue[], TError>> Collect<TValue, TError>(this Task<Result<TValue, TError>[]> results)
        => (await results).Collect();

    public static Result<IEnumerable<TValue>, TError> Collect<TValue, TError>(this IEnumerable<Result<TValue, TError>> results)
    {
        var collection = results.TryGetNonEnumeratedCount(out var count)
            ? new List<TValue>(count)
            : new List<TValue>();

        foreach (var result in results)
        {
            switch (result)
            {
                case { IsSuccess: true, Value: var value }:
                    collection.Add(value);
                    break;
                case { Error: var error}:
                    return error!;
            }
        }

        return Result.Success<IEnumerable<TValue>, TError>(collection.AsEnumerable());
    }

    public static async Task<Result<IEnumerable<TValue>, TError>> Collect<TValue, TError>(this Task<IEnumerable<Result<TValue, TError>>> results)
        => (await results).Collect();

    public static Result<TValue, TError> Aggregate<TValue, TError>(this IEnumerable<Result<TValue, TError>> results, Func<TValue, TValue, TValue> selector)
    {
        var hasValue = false;
        var resultValue = default(TValue)!;

        foreach (var result in results)
        {
            switch (result)
            {
                case { IsSuccess: true, Value: var value } when !hasValue:
                    resultValue = value;
                    hasValue = true;
                    break;
                case { IsSuccess: true, Value: var value }:
                    resultValue = selector(resultValue, value);
                    break;
                case { Error: var error}:
                    return error!;
            }
        }

        return hasValue
            ? resultValue
            : throw new InvalidOperationException("Sequence contains no elements");
    }

    public static async Task<Result<TValue, TError>> Aggregate<TValue, TError>(this Task<IEnumerable<Result<TValue, TError>>> results, Func<TValue, TValue, TValue> selector)
        => (await results).Aggregate(selector);

    public static Result<TOut, TError> Aggregate<TIn, TOut, TError>(this IEnumerable<Result<TIn, TError>> results, TOut seed, Func<TOut, TIn, TOut> selector)
    {
        var resultValue = seed;

        foreach (var result in results)
        {
            switch (result)
            {
                case { IsSuccess: true, Value: var value }:
                    resultValue = selector(resultValue, value);
                    break;
                case { Error: var error}:
                    return error!;
            }
        }

        return resultValue;
    }

    public static async Task<Result<TOut, TError>> Aggregate<TIn, TOut, TError>(this Task<IEnumerable<Result<TIn, TError>>> results, TOut seed, Func<TOut, TIn, TOut> selector)
        => (await results).Aggregate(seed, selector);
}