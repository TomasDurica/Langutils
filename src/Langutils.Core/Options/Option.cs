﻿using System.Diagnostics.CodeAnalysis;

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

    /// <summary>
    /// Creates a new option
    /// </summary>
    /// <returns>
    /// `Some(value)` when value is not null
    /// `None` when value is null
    /// </returns>
    public static Option<TValue> From<TValue>(TValue? value) where TValue: struct => value switch
    {
        not null => new Option<TValue>(value.Value),
        _ => new Option<TValue>()
    };

    /// <summary>
    /// Creates a new option
    /// </summary>
    /// <returns>
    /// `Some(value)` when value is not null
    /// `None` when value is null
    /// </returns>
    public static Option<TValue> From<TValue>(TValue? value) where TValue: class => value;
}

/// <summary>
/// This type is used to represent a none without specifying a value type.
/// </summary>
public readonly record struct None
{
    /// <summary>
    /// The singleton instance of None.
    /// </summary>
    public static readonly None Instance = new();
}

/// <summary>
/// This type is used to represent Some in ambiguous cases.
/// </summary>
public readonly record struct Some<TValue>(TValue Value);

/// <summary>
/// This type is used to simplify the creation of Some options.
/// </summary>
public static class Some
{
    /// <summary>
    /// Creates a new some option.
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public static Some<TValue> From<TValue>(TValue value) => new(value);
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

    /// <summary>
    /// Creates a new option `Some`.
    /// </summary>
    public static implicit operator Option<TValue>(Some<TValue> some) => new(some.Value);

    /// <returns>
    /// "Option.Some({Value})" when <see cref="IsSome"/> is true.
    /// "Option.None" when <see cref="IsSome"/> is false.
    /// </returns>
    public override string ToString()
        => IsSome
            ? $"Option.Some({Value})"
            : "Option.None";
}