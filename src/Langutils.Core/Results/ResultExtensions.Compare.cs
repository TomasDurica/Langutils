namespace Langutils.Core.Results;

public static partial class ResultExtensions
{
    /// <summary>
    /// Compares the current Result with another Result.
    /// </summary>
    /// <param name="self">The current Result.</param>
    /// <param name="option">The Result to compare with the current Result.</param>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
    /// Less than zero: This object is less than the other parameter.
    /// Zero: This object is equal to other.
    /// Greater than zero: This object is greater than other.</returns>
    public static int CompareTo<TValue, TError>(this Result<TValue, TError> self, Result<TValue, TError> option)
        where TValue: IComparable<TValue>
        => (self, option) switch
        {
            ({ IsSuccess: false }, { IsSuccess: false }) => 0,
            ({ IsSuccess: false }, { IsSuccess: true }) => -1,
            ({ IsSuccess: true }, { IsSuccess: false }) => 1,
            ({ IsSuccess: true, Value: var left }, { IsSuccess: true, Value: var right }) => left!.CompareTo(right!)
        };
}