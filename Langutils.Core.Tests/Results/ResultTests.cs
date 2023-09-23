using Langutils.Core.Results;
using NSubstitute;

namespace Langutils.Core.Tests.Results;

public class ResultTests
{
    [Fact]
    public void IsSuccess_OnSuccess_ReturnsTrue()
        => Assert.True(Result.Success(0).IsSuccess);

    [Fact]
    public void IsSuccess_OnError_ReturnsFalse()
        => Assert.False(Result.Error<int>("").IsSuccess);

    [Fact]
    public void IsError_OnSuccess_ReturnsFalse()
        => Assert.False(Result.Success(0).IsError);

    [Fact]
    public void IsError_OnError_ShouldBeTrue()
        => Assert.True(Result.Error<int>("").IsError);

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