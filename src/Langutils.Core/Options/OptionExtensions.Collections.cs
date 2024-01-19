using System.Numerics;

namespace Langutils.Core.Options;

public static partial class OptionExtensions
{
    /// <summary>
    /// Converts the Option to an enumerable containing the value if it exists.
    /// </summary>
    /// <param name="self">The Option to convert.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>An enumerable containing the value if it exists, otherwise an empty enumerable.</returns>
    public static IEnumerable<TValue> AsEnumerable<TValue>(this Option<TValue> self)
        => self switch
        {
            { IsSome: true, Value: var value } => new [] { value },
            _ => Enumerable.Empty<TValue>()
        };

    /// <summary>
    /// Asynchronously converts the Option to an enumerable containing the value if it exists.
    /// </summary>
    /// <param name="self">The Option to convert.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>An enumerable containing the value if it exists, otherwise an empty enumerable.</returns>
    public static async Task<IEnumerable<TValue>> AsEnumerable<TValue>(this Task<Option<TValue>> self)
        => (await self.ConfigureAwait(false)).AsEnumerable();

    /// <summary>
    /// Converts the Option to an enumerable containing the value if it exists.
    /// </summary>
    /// <param name="self">The Option to convert.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>An enumerable containing the value if it exists, otherwise an empty enumerable.</returns>
    public static List<TValue> ToList<TValue>(this Option<TValue> self)
        => self switch
        {
            { IsSome: true, Value: var value } => new List<TValue> { value },
            _ => new List<TValue>()
        };

    /// <summary>
    /// Asynchronously converts the Option to an enumerable containing the value if it exists.
    /// </summary>
    /// <param name="self">The Option to convert.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>An enumerable containing the value if it exists, otherwise an empty enumerable.</returns>
    public static async Task<List<TValue>> ToList<TValue>(this Task<Option<TValue>> self)
        => (await self.ConfigureAwait(false)).ToList();

    /// <summary>
    /// Converts the Option to an array containing the value if it exists.
    /// </summary>
    /// <param name="self">The Option to convert.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>An array containing the value if it exists, otherwise an empty array.</returns>
    public static TValue[] ToArray<TValue>(this Option<TValue> self)
        => self switch
        {
            { IsSome: true, Value: var value } => new [] { value },
            _ => Array.Empty<TValue>()
        };

    /// <summary>
    /// Asynchronously converts the Option to an array containing the value if it exists.
    /// </summary>
    /// <param name="self">The Option to convert.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>An array containing the value if it exists, otherwise an empty array.</returns>
    public static async Task<TValue[]> ToArray<TValue>(this Task<Option<TValue>> self)
        => (await self.ConfigureAwait(false)).ToArray();

    /// <summary>
    /// Collects the values from a list of Options into a new Option of a list.
    /// </summary>
    /// <param name="options">The list of Options to collect.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>An Option of a list containing the values if all Options have values, otherwise None.</returns>
    public static Option<List<TValue>> Collect<TValue>(this List<Option<TValue>> options)
    {
        var result = new List<TValue>(options.Count);

        foreach (var option in options)
        {
            if (option is { IsSome: true, Value: var value })
            {
                result.Add(value);
            }
            else
            {
                return None.Instance;
            }
        }

        return result;
    }

    /// <summary>
    /// Asynchronously collects the values from a list of Options into a new Option of a list.
    /// </summary>
    /// <param name="options">The list of Options to collect.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>An Option of a list containing the values if all Options have values, otherwise None.</returns>
    public static async Task<Option<List<TValue>>> Collect<TValue>(this Task<List<Option<TValue>>> options)
        => (await options.ConfigureAwait(false)).Collect();

    /// <summary>
    /// Collects the values from an array of Options into a new Option of an array.
    /// </summary>
    /// <param name="options">The array of Options to collect.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>An Option of an array containing the values if all Options have values, otherwise None.</returns>
    public static Option<TValue[]> Collect<TValue>(this Option<TValue>[] options)
    {
        var result = new List<TValue>(options.Length);

        foreach (var option in options)
        {
            if (option is { IsSome: true, Value: var value })
            {
                result.Add(value);
            }
            else
            {
                return None.Instance;
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Asynchronously collects the values from an array of Options into a new Option of an array.
    /// </summary>
    /// <param name="options">The array of Options to collect.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>An Option of an array containing the values if all Options have values, otherwise None.</returns>
    public static async Task<Option<TValue[]>> Collect<TValue>(this Task<Option<TValue>[]> options)
        => (await options.ConfigureAwait(false)).Collect();

    /// <summary>
    /// Collects the values from an enumerable of Options into a new Option of an enumerable.
    /// </summary>
    /// <param name="options">The enumerable of Options to collect.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>An Option of an enumerable containing the values if all Options have values, otherwise None.</returns>
    public static Option<IEnumerable<TValue>> Collect<TValue>(this IEnumerable<Option<TValue>> options)
    {
        var result = options.TryGetNonEnumeratedCount(out var count)
            ? new List<TValue>(count)
            : new List<TValue>();

        foreach (var option in options)
        {
            if (option is { IsSome: true, Value: var value })
            {
                result.Add(value);
            }
            else
            {
                return None.Instance;
            }
        }

        return Option.Some(result.AsEnumerable());
    }

    /// <summary>
    /// Asynchronously collects the values from an enumerable of Options into a new Option of an enumerable.
    /// </summary>
    /// <param name="options">The enumerable of Options to collect.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>An Option of an enumerable containing the values if all Options have values, otherwise None.</returns>
    public static async Task<Option<IEnumerable<TValue>>> Collect<TValue>(this Task<IEnumerable<Option<TValue>>> options)
        => (await options.ConfigureAwait(false)).Collect();

    /// <summary>
    /// Aggregates the values from an enumerable of Options using a selector function.
    /// </summary>
    /// <param name="options">The sequence of Options to aggregate.</param>
    /// <param name="selector">A function to apply over the sequence.</param>
    /// <typeparam name="TValue">The type of the value in the Options.</typeparam>
    /// <returns>An Option of the aggregated value if all Options have values, otherwise None.</returns>
    public static Option<TValue> Aggregate<TValue>(this IEnumerable<Option<TValue>> options, Func<TValue, TValue, TValue> selector)
    {
        var hasValue = false;
        var result = default(TValue);

        foreach (var option in options)
        {
            switch (option)
            {
                case { IsSome: true, Value: var value } when !hasValue:
                    result = value;
                    hasValue = true;
                    break;
                case { IsSome: true, Value: var value }:
                    result = selector(result!, value);
                    break;
                default:
                    return None.Instance;
            }
        }

        return hasValue
            ? result
            : throw new InvalidOperationException("Sequence contains no elements");
    }

    /// <summary>
    /// Asynchronously aggregates the values from an enumerable of Options using a selector function.
    /// </summary>
    /// <param name="options">The sequence of Options to aggregate.</param>
    /// <param name="selector">A function to apply over the sequence.</param>
    /// <typeparam name="TValue">The type of the value in the Options.</typeparam>
    /// <returns>An Option of the aggregated value if all Options have values, otherwise None.</returns>
    public static async Task<Option<TValue>> Aggregate<TValue>(this Task<IEnumerable<Option<TValue>>> options, Func<TValue, TValue, TValue> selector)
        => (await options.ConfigureAwait(false)).Aggregate(selector);

    /// <summary>
    /// Aggregates the values from an enumerable of Options using a seed value and a selector function.
    /// </summary>
    /// <param name="options">The sequence of Options to aggregate.</param>
    /// <param name="seed">The initial accumulator value.</param>
    /// <param name="selector">A function to apply over the sequence.</param>
    /// <typeparam name="TIn">The type of the input values in the Options.</typeparam>
    /// <typeparam name="TOut">The type of the output values in the Option.</typeparam>
    /// <returns>An Option of the aggregated value if all Options have values, otherwise None.</returns>
    public static Option<TOut> Aggregate<TIn, TOut>(this IEnumerable<Option<TIn>> options, TOut seed, Func<TOut, TIn, TOut> selector)
    {
        var result = seed;

        foreach (var option in options)
        {
            switch (option)
            {
                case { IsSome: true, Value: var value }:
                    result = selector(result, value);
                    break;
                default:
                    return None.Instance;
            }
        }

        return result;
    }

    /// <summary>
    /// Asynchronously aggregates the values from an enumerable of Options using a seed value and a selector function.
    /// </summary>
    /// <param name="options">The sequence of Options to aggregate.</param>
    /// <param name="seed">The initial accumulator value.</param>
    /// <param name="selector">A function to apply over the sequence.</param>
    /// <typeparam name="TIn">The type of the input values in the Options.</typeparam>
    /// <typeparam name="TOut">The type of the output values in the Option.</typeparam>
    /// <returns>An Option of the aggregated value if all Options have values, otherwise None.</returns>
    public static async Task<Option<TOut>> Aggregate<TIn, TOut>(this Task<IEnumerable<Option<TIn>>> options, TOut seed, Func<TOut, TIn, TOut> selector)
        => (await options.ConfigureAwait(false)).Aggregate(seed, selector);

    /// <summary>
    /// Sums the values from an enumerable of Options.
    /// </summary>
    /// <param name="options">The sequence of Options to sum.</param>
    /// <typeparam name="TValue">The type of the input values in the Options.</typeparam>
    /// <returns>An Option of the sum if all Options have values, otherwise None.</returns>
    public static Option<TValue> Sum<TValue>(this IEnumerable<Option<TValue>> options)
        where TValue : INumber<TValue>
    {
        var sum = TValue.Zero;

        foreach (var option in options)
        {
            switch (option)
            {
                case { IsSome: true, Value: var value }:
                    sum += value;
                    break;
                default:
                    return None.Instance;
            }
        }

        return sum;
    }

    /// <summary>
    /// Asynchronously sums the values from an enumerable of Options.
    /// </summary>
    /// <param name="options">The sequence of Options to sum.</param>
    /// <typeparam name="TValue">The type of the input values in the Options.</typeparam>
    /// <returns>An Option of the sum if all Options have values, otherwise None.</returns>
    public static async Task<Option<TValue>> Sum<TValue>(this Task<IEnumerable<Option<TValue>>> options)
        where TValue : INumber<TValue>
        => (await options.ConfigureAwait(false)).Sum();

    /// <summary>
    /// Multiplies the values from an enumerable of Options.
    /// </summary>
    /// <param name="options">The sequence of Options to multiply.</param>
    /// <typeparam name="TValue">The type of the input values in the Options.</typeparam>
    /// <returns>An Option of the product if all Options have values, otherwise None.</returns>
    public static Option<TValue> Product<TValue>(this IEnumerable<Option<TValue>> options)
        where TValue : INumber<TValue>
    {
        var product = TValue.One;

        foreach (var option in options)
        {
            switch (option)
            {
                case { IsSome: true, Value: var value }:
                    product *= value;
                    break;
                default:
                    return None.Instance;
            }
        }

        return product;
    }

    /// <summary>
    /// Asynchronously multiplies the values from an enumerable of Options.
    /// </summary>
    /// <param name="options">The sequence of Options to multiply.</param>
    /// <typeparam name="TValue">The type of the input values in the Options.</typeparam>
    /// <returns>An Option of the product if all Options have values, otherwise None.</returns>
    public static async Task<Option<TValue>> Product<TValue>(this Task<IEnumerable<Option<TValue>>> options)
        where TValue : INumber<TValue>
        => (await options.ConfigureAwait(false)).Product();
}