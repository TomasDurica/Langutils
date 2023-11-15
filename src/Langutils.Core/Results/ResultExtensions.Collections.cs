namespace Langutils.Core.Results;

public static partial class ResultExtensions
{
    /// <summary>
    /// Converts the Result to an enumerable containing the value if the Result is a success, or an empty enumerable if the Result is a failure.
    /// </summary>
    /// <param name="self">The Result to convert.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>An enumerable containing the value if the Result is a success, or an empty enumerable if the Result is a failure.</returns>
    public static IEnumerable<TValue> AsEnumerable<TValue, TError>(this Result<TValue, TError> self)
        => self switch
        {
            { IsSuccess: true, Value: var value } => new [] { value },
            _ => Enumerable.Empty<TValue>()
        };


    /// <summary>
    /// Asynchronously converts the Result to an enumerable containing the value if the Result is a success, or an empty enumerable if the Result is a failure.
    /// </summary>
    /// <param name="self">The Task of Result to convert.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>An enumerable containing the value if the Result is a success, or an empty enumerable if the Result is a failure.</returns>
    public static async Task<IEnumerable<TValue>> AsEnumerable<TValue, TError>(this Task<Result<TValue, TError>> self)
        => (await self.ConfigureAwait(false)).AsEnumerable();

    /// <summary>
    /// Converts the Result to a list containing the value if the Result is a success, or an empty list if the Result is a failure.
    /// </summary>
    /// <param name="self">The Result to convert.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A list containing the value if the Result is a success, or an empty list if the Result is a failure.</returns>
    public static List<TValue> ToList<TValue, TError>(this Result<TValue, TError> self)
        => self switch
        {
            { IsSuccess: true, Value: var value } => new List<TValue> { value },
            _ => new List<TValue>()
        };

    /// <summary>
    /// Asynchronously converts the Result to a list containing the value if the Result is a success, or an empty list if the Result is a failure.
    /// </summary>
    /// <param name="self">The Task of Result to convert.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of a list containing the value if the Result is a success, or an empty list if the Result is a failure.</returns>
    public static async Task<List<TValue>> ToList<TValue, TError>(this Task<Result<TValue, TError>> self)
        => (await self.ConfigureAwait(false)).ToList();

    /// <summary>
    /// Converts the Result to an array containing the value if the Result is a success, or an empty array if the Result is a failure.
    /// </summary>
    /// <param name="self">The Result to convert.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>An array containing the value if the Result is a success, or an empty array if the Result is a failure.</returns>
    public static TValue[] ToArray<TValue, TError>(this Result<TValue, TError> self)
        => self switch
        {
            { IsSuccess: true, Value: var value } => new [] { value },
            _ => Array.Empty<TValue>()
        };

    /// <summary>
    /// Asynchronously converts the Result to an array containing the value if the Result is a success, or an empty array if the Result is a failure.
    /// </summary>
    /// <param name="self">The Task of Result to convert.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of an array containing the value if the Result is a success, or an empty array if the Result is a failure.</returns>
    public static async Task<TValue[]> ToArray<TValue, TError>(this Task<Result<TValue, TError>> self)
        => (await self.ConfigureAwait(false)).ToArray();

    /// <summary>
    /// Converts a list of Results into a Result of a list, returning the first error if any of the Results are failures.
    /// </summary>
    /// <param name="results">The list of Results to collect.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Result of a list containing the values of the successful Results, or the first error if any Result is a failure.</returns>
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

    /// <summary>
    /// Converts a list of Results into a Result of a list, returning the first error if any of the Results are failures.
    /// </summary>
    /// <param name="results">The list of Results to collect.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Result of a list containing the values of the successful Results, or the first error if any Result is a failure.</returns>
    public static async Task<Result<List<TValue>, TError>> Collect<TValue, TError>(this Task<List<Result<TValue, TError>>> results)
        => (await results.ConfigureAwait(false)).Collect();

    /// <summary>
    /// Converts an array of Results into a Result of an array, returning the first error if any of the Results are failures.
    /// </summary>
    /// <param name="results">The array of Results to collect.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Result of an array containing the values of the successful Results, or the first error if any Result is a failure.</returns>
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

    /// <summary>
    /// Asynchronously converts an array of Results into a Result of an array, returning the first error if any of the Results are failures.
    /// </summary>
    /// <param name="results">The Task of array of Results to collect.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of Result of an array containing the values of the successful Results, or the first error if any Result is a failure.</returns>
    public static async Task<Result<TValue[], TError>> Collect<TValue, TError>(this Task<Result<TValue, TError>[]> results)
        => (await results.ConfigureAwait(false)).Collect();

    /// <summary>
    /// Converts an enumerable of Results into a Result of an enumerable, returning the first error if any of the Results are failures.
    /// </summary>
    /// <param name="results">The enumerable of Results to collect.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Result of an enumerable containing the values of the successful Results, or the first error if any Result is a failure.</returns>
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

    /// <summary>
    /// Asynchronously converts an enumerable of Results into a Result of an enumerable, returning the first error if any of the Results are failures.
    /// </summary>
    /// <param name="results">The Task of enumerable of Results to collect.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Task of Result of an enumerable containing the values of the successful Results, or the first error if any Result is a failure.</returns>
    public static async Task<Result<IEnumerable<TValue>, TError>> Collect<TValue, TError>(this Task<IEnumerable<Result<TValue, TError>>> results)
        => (await results.ConfigureAwait(false)).Collect();

    /// <summary>
    /// Aggregates a sequence of Results into a single Result by applying a function over a sequence.
    /// </summary>
    /// <param name="results">The sequence of Results to aggregate.</param>
    /// <param name="selector">A function to apply over the sequence.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Result containing the final result of the aggregation.</returns>
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

    /// <summary>
    /// Aggregates a sequence of Results into a single Result by applying a function over a sequence.
    /// </summary>
    /// <param name="results">The sequence of Results to aggregate.</param>
    /// <param name="selector">A function to apply over the sequence.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Result containing the final result of the aggregation.</returns>
    public static async Task<Result<TValue, TError>> Aggregate<TValue, TError>(this Task<IEnumerable<Result<TValue, TError>>> results, Func<TValue, TValue, TValue> selector)
        => (await results.ConfigureAwait(false)).Aggregate(selector);

    /// <summary>
    /// Aggregates a sequence of Results into a single Result by applying a function over a sequence.
    /// The specified seed value is used as the initial accumulator value.
    /// </summary>
    /// <param name="results">The sequence of Results to aggregate.</param>
    /// <param name="seed">The initial accumulator value.</param>
    /// <param name="selector">A function to apply over the sequence.</param>
    /// <typeparam name="TIn">The type of the input values in the Result.</typeparam>
    /// <typeparam name="TOut">The type of the output values in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Result containing the final result of the aggregation.</returns>
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

    /// <summary>
    /// Aggregates a sequence of Results into a single Result by applying a function over a sequence.
    /// The specified seed value is used as the initial accumulator value.
    /// </summary>
    /// <param name="results">The sequence of Results to aggregate.</param>
    /// <param name="seed">The initial accumulator value.</param>
    /// <param name="selector">A function to apply over the sequence.</param>
    /// <typeparam name="TIn">The type of the input values in the Result.</typeparam>
    /// <typeparam name="TOut">The type of the output values in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A Result containing the final result of the aggregation.</returns>
    public static async Task<Result<TOut, TError>> Aggregate<TIn, TOut, TError>(this Task<IEnumerable<Result<TIn, TError>>> results, TOut seed, Func<TOut, TIn, TOut> selector)
        => (await results.ConfigureAwait(false)).Aggregate(seed, selector);
}