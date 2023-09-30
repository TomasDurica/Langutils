using System.Diagnostics.CodeAnalysis;

namespace Langutils.Core.Options;

public static class Option
{
    public static Option<TValue> Some<TValue>(TValue value) => new(value);
    public static Option<TValue> None<TValue>() => new();
}

public readonly record struct None
{
    public static readonly None Instance = new();
}

public readonly record struct Option<TValue>
{
    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsSome { get; }

    public TValue? Value { get; }

    public bool IsNone
        => !IsSome;

    public Option()
        => IsSome = false;

    public Option(TValue? value)
    {
        IsSome = true;
        Value = value;
    }

    public static implicit operator Option<TValue>(TValue? value)
        => value switch
        {
            not null => new Option<TValue>(value),
            _ => new Option<TValue>()
        };

    public static implicit operator Option<TValue>(None _) => new();

    public override string ToString()
        => IsSome
            ? $"Option.Some({Value})"
            : "Option.None";
}