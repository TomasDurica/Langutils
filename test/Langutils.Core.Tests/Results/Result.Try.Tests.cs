using Langutils.Core.Results;
using Langutils.Core.Tests.Asserts;

namespace Langutils.Core.Tests.Results;

public class Result_Try_Tests
{
    private const int Value = 1;
    private const int Error = -1;

    [Fact]
    public void Try_OnSuccess_ShouldReturnResultWithSuccess()
    {
        var result = Result.Try(() => Success());
        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task Try_OnSuccessAsync_ShouldReturnResultWithSuccess()
    {
        var result = await Result.TryAsync(async () => await SuccessAsync());
        AssertResult.Success(Value, result);
    }

    [Fact]
    public void Try_OnFailure_ShouldReturnResultWithError()
    {
        var result = Result.Try(() => Failure());
        AssertResult.Error(e => e is ConcreteException ce && ce.ErrorCode == Error, result);
    }

    [Fact]
    public async Task Try_OnFailureAsync_ShouldReturnResultWithError()
    {
        var result = await Result.TryAsync(async () => await FailureAsync());
        AssertResult.Error(e => e is ConcreteException ce && ce.ErrorCode == Error, result);
    }

    [Fact]
    public void Try_OnFailureConcrete_ShouldReturnResultWithConcreteError()
    {
        var result = Result.Try<int, ConcreteException>(() => Failure());
        AssertResult.Error(e => e!.ErrorCode == Error, result);
    }

    [Fact]
    public async Task Try_OnFailureConcreteAsync_ShouldReturnResultWithConcreteError()
    {
        var result = await Result.TryAsync<int, ConcreteException>(async () => await FailureAsync());
        AssertResult.Error(e => e!.ErrorCode == Error, result);
    }

    [Fact]
    public void Try_OnFailureGeneric_ShouldThrow()
    {
        Assert.Throws<Exception>(() =>
        {
            Result.Try<int, ConcreteException>(() => throw new Exception());
        });
    }

    [Fact]
    public async Task Try_OnFailureGenericAsync_ShouldThrow()
    {
        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await Result.TryAsync<int, ConcreteException>(() => throw new Exception());
        });
    }

    private int Success()
        => Value;

    private  Task<int> SuccessAsync()
        => Task.FromResult(Value);

    private  int Failure()
        => throw new ConcreteException(Error);

    private  Task<int> FailureAsync()
        => throw new ConcreteException(Error);

    private class ConcreteException(int errorCode) : Exception
    {
        public int ErrorCode { get; } = errorCode;
    };
}