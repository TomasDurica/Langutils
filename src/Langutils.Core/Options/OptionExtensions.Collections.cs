using System.Numerics;

namespace Langutils.Core.Options;

public static partial class OptionExtensions
{
    public static IEnumerable<TValue> AsEnumerable<TValue>(this Option<TValue> self)
        => self switch
        {
            { IsSome: true, Value: var value } => new [] { value },
            _ => Enumerable.Empty<TValue>()
        };

    public static async Task<IEnumerable<TValue>> AsEnumerable<TValue>(this Task<Option<TValue>> self)
        => (await self).AsEnumerable();

    public static List<TValue> ToList<TValue>(this Option<TValue> self)
        => self switch
        {
            { IsSome: true, Value: var value } => new List<TValue> { value },
            _ => new List<TValue>()
        };

    public static async Task<List<TValue>> ToList<TValue>(this Task<Option<TValue>> self)
        => (await self).ToList();

    public static TValue[] ToArray<TValue>(this Option<TValue> self)
        => self switch
        {
            { IsSome: true, Value: var value } => new [] { value },
            _ => Array.Empty<TValue>()
        };

    public static async Task<TValue[]> ToArray<TValue>(this Task<Option<TValue>> self)
        => (await self).ToArray();

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

    public static async Task<Option<List<TValue>>> Collect<TValue>(this Task<List<Option<TValue>>> options)
        => (await options).Collect();

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

    public static async Task<Option<TValue[]>> Collect<TValue>(this Task<Option<TValue>[]> options)
        => (await options).Collect();

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

    public static async Task<Option<IEnumerable<TValue>>> Collect<TValue>(this Task<IEnumerable<Option<TValue>>> options)
        => (await options).Collect();

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

    public static async Task<Option<TValue>> Aggregate<TValue>(this Task<IEnumerable<Option<TValue>>> options, Func<TValue, TValue, TValue> selector)
        => (await options).Aggregate(selector);

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

    public static async Task<Option<TOut>> Aggregate<TIn, TOut>(this Task<IEnumerable<Option<TIn>>> options, TOut seed, Func<TOut, TIn, TOut> selector)
        => (await options).Aggregate(seed, selector);

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

    public static async Task<Option<TValue>> Sum<TValue>(this Task<IEnumerable<Option<TValue>>> options)
        where TValue : INumber<TValue>
        => (await options).Sum();

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

    public static async Task<Option<TValue>> Product<TValue>(this Task<IEnumerable<Option<TValue>>> options)
        where TValue : INumber<TValue>
        => (await options).Product();

}