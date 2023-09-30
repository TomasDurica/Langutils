namespace Langutils.Core.Results;

public static partial class ResultExtensions
{
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