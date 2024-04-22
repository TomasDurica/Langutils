namespace Langutils.Core.Options;

public static partial class OptionExtensions
{
    /// <summary>
    /// Compares the current Option with another Option.
    /// </summary>
    /// <param name="self">The current Option.</param>
    /// <param name="option">The Option to compare with the current Option.</param>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
    /// Less than zero: This object is less than the other parameter.
    /// Zero: This object is equal to other.
    /// Greater than zero: This object is greater than other.
    /// </returns>
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