using Langutils.Core.Results;

namespace Langutils.Core.Tests.Asserts;

public static class AssertResult
{
    public static void Success<TValue, TError>(TValue expectedValue, Result<TValue, TError> actual)
    {
        Assert.True(actual.IsSuccess);
        Assert.Equal(expectedValue, actual.Unwrap());
    }

    public static void Error<TValue, TError>(TError expectedError, Result<TValue, TError> actual)
    {
        Assert.True(actual.IsError);
        Assert.Equal(expectedError, actual.UnwrapError());
    }
}