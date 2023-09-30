namespace Langutils.Core.Options;

public static partial class OptionExtensions
{
    public static int CompareTo<TValue>(this Option<TValue> self, Option<TValue> option)
        where TValue: IComparable<TValue>
        => (self, option) switch
        {
            ({ IsSome: false }, { IsSome: false }) => 0,
            ({ IsSome: false }, { IsSome: true }) => -1,
            ({ IsSome: true }, { IsSome: false }) => 1,
            ({ Value: var left }, { Value: var right }) => left!.CompareTo(right!)
        };
}