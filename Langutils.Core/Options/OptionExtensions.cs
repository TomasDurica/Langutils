using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Langutils.Core.Results;

namespace Langutils.Core.Options;

public static class OptionExtensions
{
    public static bool IsSomeAnd<TValue>(this Option<TValue> self, Func<TValue, bool> predicate) => self switch
    {
        { IsSome: true, Value: var value } => predicate(value),
        _ => false
    };

    public static TValue Expect<TValue>(this Option<TValue> self, string message) => self switch
    {
        { IsSome: true, Value: var value } => value,
        _ => throw new InvalidOperationException(message)
    };

    public static bool TryUnwrap<TValue>(this Option<TValue> self, [NotNullWhen(true)] out TValue? value)
    {
        if (self is { IsSome: true, Value: var innerValue })
        {
            value = innerValue;
            return true;
        }

        value = default;
        return false;
    }

    public static TValue UnwrapOr<TValue>(this Option<TValue> self, TValue defaultValue) => self switch
    {
        { IsSome: true, Value: var value } => value,
        _ => defaultValue
    };

    public static TValue? UnwrapOrDefault<TValue>(this Option<TValue> self) => self switch
    {
        { IsSome: true, Value: var value } => value,
        _ => default
    };

    public static TValue UnwrapOrElse<TValue>(this Option<TValue> self, Func<TValue> defaultValueProvider) => self switch
    {
        { IsSome: true, Value: var value } => value,
        _ => defaultValueProvider()
    };

    public static Option<TValue> Tap<TValue>(this Option<TValue> self, Action<TValue> onSome)
    {
        if (self is { IsSome: true, Value: var value })
        {
            onSome(value);
        }

        return self;
    }

    public static Option<TValue> Filter<TValue>(this Option<TValue> self, Func<TValue, bool> predicate) => self switch
    {
        { IsSome: true, Value: var value } when predicate(value) => self,
        _ => None.Instance
    };

    public static Option<TOut> Map<TIn, TOut>(this Option<TIn> self, Func<TIn, TOut> selector) => self switch
    {
        { IsSome: true, Value: var value } => selector(value),
        _ => None.Instance
    };

    public static TOut MapOr<TIn, TOut>(this Option<TIn> self, TOut defaultValue, Func<TIn, TOut> selector) => self switch
    {
        { IsSome: true, Value: var value } => selector(value),
        _ => defaultValue
    };

    public static TOut MapOrElse<TIn, TOut>(this Option<TIn> self, Func<TOut> defaultValueProvider, Func<TIn, TOut> selector) => self switch
    {
        { IsSome: true, Value: var value } => selector(value),
        _ => defaultValueProvider()
    };

    public static Result<TValue, TError> SomeOr<TValue, TError>(this Option<TValue> self, TError error) => self switch
    {
        { IsSome: true, Value: var value } => value,
        _ => error
    };

    public static Result<TValue, TError> SomeOrElse<TValue, TError>(this Option<TValue> self, Func<TError> errorProvider) => self switch
    {
        { IsSome: true, Value: var value } => value,
        _ => errorProvider()
    };

    public static Result<Option<TValue>, TError> Transpose<TValue, TError>(this Option<Result<TValue, TError>> self) => self switch
    {
        { IsSome: true, Value: { IsSuccess: true, Value: var value }} => Option.Some(value),
        { IsSome: true, Value: { IsError: true, Error: var error }} => error!,
        _ => Option.None<TValue>()
    };

    public static Option<TValue> Flatten<TValue>(this Option<Option<TValue>> self) => self switch
    {
        { IsSome: true, Value: var option} => option,
        _ => None.Instance
    };

    public static Option<TOut> And<TIn, TOut>(this Option<TIn> self, Option<TOut> option) => self switch
    {
        { IsSome: true } => option,
        _ => None.Instance
    };

    public static Option<TOut> AndThen<TIn, TOut>(this Option<TIn> self, Func<TIn, Option<TOut>> optionProvider) => self switch
    {
        { IsSome: true, Value: var value } => optionProvider(value),
        _ => None.Instance
    };

    public static Option<TValue> Or<TValue>(this Option<TValue> self, Option<TValue> option) => self switch
    {
        { IsSome: true } => self,
        _ => option
    };

    public static Option<TValue> OrElse<TValue>(this Option<TValue> self, Func<Option<TValue>> optionProvider) => self switch
    {
        { IsSome: true } => self,
        _ => optionProvider()
    };

    public static Option<TValue> Xor<TValue>(this Option<TValue> self, Option<TValue> option) => (self, option) switch
    {
        (self: { IsSome: true }, option: { IsSome: false }) => self,
        (self: { IsSome: false }, option: { IsSome: true }) => option,
        _ => None.Instance
    };

    public static Option<(TValue1 Left, TValue2 Right)> Zip<TValue1, TValue2>(this Option<TValue1> self, Option<TValue2> option) => (self, option) switch
    {
        ({ IsSome: true, Value: var left }, { IsSome: true, Value: var right}) => (left!, right!),
        _ => None.Instance
    };

    public static Option<TOut> ZipWith<TIn1, TIn2, TOut>(this Option<TIn1> self, Option<TIn2> option, Func<TIn1, TIn2, TOut> selector) => (self, option) switch
    {
        ({ IsSome: true, Value: var left }, { IsSome: true, Value: var right}) => selector(left!, right!),
        _ => None.Instance
    };

    public static (Option<TValue1> Left, Option<TValue2> Right) Unzip<TValue1, TValue2>(this Option<(TValue1 Left, TValue2 Right)> self) => self switch
    {
        { IsSome: true, Value: var (left, right) } => (left, right),
        _ => (None.Instance, None.Instance)
    };

    public static int CompareTo<TValue>(this Option<TValue> self, Option<TValue> option) where TValue: IComparable<TValue> => (self, option) switch
    {
        ({ IsSome: false }, { IsSome: false }) => 0,
        ({ IsSome: false }, { IsSome: true }) => -1,
        ({ IsSome: true }, { IsSome: false }) => 1,
        ({ Value: var left }, { Value: var right }) => left!.CompareTo(right!)
    };

    public static IEnumerable<TValue> AsEnumerable<TValue>(this Option<TValue> self) => self switch
    {
        { IsSome: true, Value: var value } => new [] { value },
        _ => Enumerable.Empty<TValue>()
    };

    public static List<TValue> ToList<TValue>(this Option<TValue> self) => self switch
    {
        { IsSome: true, Value: var value } => new List<TValue> { value },
        _ => new List<TValue>()
    };

    public static TValue[] ToArray<TValue>(this Option<TValue> self) => self switch
    {
        { IsSome: true, Value: var value } => new [] { value },
        _ => Array.Empty<TValue>()
    };

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
}