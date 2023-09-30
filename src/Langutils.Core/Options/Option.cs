using System.Diagnostics.CodeAnalysis;

namespace Langutils.Core.Options;

/// <summary>
/// This type is used to create options.
/// </summary>
public static class Option
{
    /// <summary>
    /// Returns a some option with a value.
    /// </summary>
    /// <param name="value">The underlying value</param>
    /// <typeparam name="TValue">The Value type</typeparam>
    /// <returns></returns>
    public static Option<TValue> Some<TValue>(TValue value) => new(value);

    /// <summary>
    /// Returns a none option.
    /// </summary>
    /// <typeparam name="TValue">The Value type</typeparam>
    /// <returns></returns>
    public static Option<TValue> None<TValue>() => new();
}

/// <summary>
/// This type is used to represent a none without specifying a value type.
/// </summary>
public readonly record struct None
{
    public static readonly None Instance = new();
}

/// <summary>
/// A type that represents either a value or nothing. It is similar to `Option` in functional languages.
/// </summary>
/// <typeparam name="TValue">The Value type</typeparam>
public readonly record struct Option<TValue>
{
    /// <summary>
    /// Indicates whether the option is some.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsSome { get; }

    /// <summary>
    /// Returns the value if the option is some.
    /// It is not null when <see cref="IsSome"/> is true.
    /// </summary>
    public TValue? Value { get; }

    /// <summary>
    /// Indicates whether the option is none.
    /// </summary>
    /// <returns>
    /// `true` when &lt;see cref="IsSome"/&gt; is false.
    /// </returns>
    public bool IsNone
        => !IsSome;

    /// <summary>
    /// Creates a new option `None`.
    /// </summary>
    public Option()
        => IsSome = false;

    /// <summary>
    /// Creates a new option `Some(value)`.
    /// </summary>
    /// <param name="value">The underlying value</param>
    public Option(TValue? value)
    {
        IsSome = true;
        Value = value;
    }

    /// <summary>
    /// Creates a new option
    /// </summary>
    /// <returns>
    /// `Some(value)` when value is not null
    /// `None` when value is null
    /// </returns>
    public static implicit operator Option<TValue>(TValue? value)
        => value switch
        {
            not null => new Option<TValue>(value),
            _ => new Option<TValue>()
        };

    /// <summary>
    /// Creates a new option `None`.
    /// </summary>
    public static implicit operator Option<TValue>(None _) => new();

    /// <returns>
    /// "Option.Some({Value})" when <see cref="IsSome"/> is true.
    /// "Option.None" when <see cref="IsSome"/> is false.
    /// </returns>
    public override string ToString()
        => IsSome
            ? $"Option.Some({Value})"
            : "Option.None";
}