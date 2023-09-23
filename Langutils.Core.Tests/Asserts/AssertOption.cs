using Langutils.Core.Options;

namespace Langutils.Core.Tests.Asserts;

public static class AssertOption
{
    public static void Some<TValue>(TValue expectedValue, Option<TValue> actual)
    {
        Assert.True(actual.IsSome);
        Assert.Equal(expectedValue, actual.Unwrap());
    }

    public static void None<TValue>(Option<TValue> actual)
        => Assert.True(actual.IsNone);

    public static void Equal<TValue>(Option<TValue> expected, Option<TValue> actual)
        => Assert.StrictEqual(expected, actual);
}