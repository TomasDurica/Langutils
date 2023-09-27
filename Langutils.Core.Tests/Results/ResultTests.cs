using Langutils.Core.Results;

namespace Langutils.Core.Tests.Results;

public class ResultTests
{
    [Fact]
    public void IsSuccess_OnSuccess_ReturnsTrue()
        => Assert.True(Result.Success<object, object>(new object()).IsSuccess);

    [Fact]
    public void IsSuccess_OnError_ReturnsFalse()
        => Assert.False(Result.Error<object, object>(new object()).IsSuccess);

    [Fact]
    public void IsError_OnSuccess_ReturnsFalse()
        => Assert.False(Result.Success<object, object>(new object()).IsError);

    [Fact]
    public void IsError_OnError_ShouldBeTrue()
        => Assert.True(Result.Error<object, object>(new object()).IsError);

    [Fact]
    public void ImplicitCast_AssignableToValue_ReturnsSuccess()
    {
        Result<int, string> result = 0;

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void ImplicitCast_AssignableToError_ReturnsError()
    {
        Result<int, string> option = "";

        Assert.True(option.IsError);
    }
}