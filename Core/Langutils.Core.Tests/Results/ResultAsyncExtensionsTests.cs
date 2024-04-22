using System.Collections;
using Langutils.Core.Results;
using Langutils.Core.Tests.Asserts;
using NSubstitute;

namespace Langutils.Core.Tests.Results;

public class ResultAsyncExtensionsTests
{
    private static readonly object Value = new();
    private static readonly object OtherValue = new();
    private static readonly object ErrorMessage = new();
    private static readonly object OtherErrorMessage = new();
    private static readonly Task<object> TaskValue = Task.FromResult(Value);
    private static readonly Task<object> TaskOtherValue = Task.FromResult(OtherValue);
    private static readonly Task<object> TaskErrorMessage = Task.FromResult(ErrorMessage);
    private static readonly Task<object> TaskOtherErrorMessage = Task.FromResult(OtherErrorMessage);
    private static readonly Result<object, object> Success = Result.Success<object, object>(Value);
    private static readonly Result<object, object> OtherSuccess = Result.Success<object, object>(OtherValue);
    private static readonly Result<object, object> Error = Result.Error<object, object>(ErrorMessage);
    private static readonly Result<object, object> OtherError = Result.Error<object, object>(OtherErrorMessage);
    private static readonly Task<Result<object, object>> TaskSuccess = Task.FromResult(Success);
    private static readonly Task<Result<object, object>> TaskOtherSuccess = Task.FromResult(OtherSuccess);
    private static readonly Task<Result<object, object>> TaskError = Task.FromResult(Error);
    private static readonly Task<Result<object, object>> TaskOtherError = Task.FromResult(OtherError);

    [Fact]
    public async Task IsSuccessAndAsync_OnSuccess_WhenMatchesPredicate_ReturnsTrue()
    {
        var result = await Success.IsSuccessAndAsync(v => Task.FromResult(v == Value));

        Assert.True(result);
    }

    [Fact]
    public async Task IsSuccessAndAsync_OnSuccess_WhenDoesNotMatchPredicate_ReturnsFalse()
    {
        var result = await Success.IsSuccessAndAsync(v => Task.FromResult(v != Value));

        Assert.False(result);
    }

    [Fact]
    public async Task IsSuccessAndAsync_OnError_ReturnsFalse()
    {
        var result = await Error.IsSuccessAndAsync(_ => Task.FromResult(true));

        Assert.False(result);
    }

    [Fact]
    public async Task IsErrorAndAsync_OnSuccess_ReturnsFalse()
    {
        var result = await Success.IsErrorAndAsync(_ => Task.FromResult(true));

        Assert.False(result);
    }

    [Fact]
    public async Task IsErrorAndAsync_OnError_WhenMatchesPredicate_ReturnsTrue()
    {
        var result = await Error.IsErrorAndAsync(v => Task.FromResult(v == ErrorMessage));

        Assert.True(result);
    }

    [Fact]
    public async Task IsErrorAndAsync_OnError_WhenDoesNotMatchPredicate_ReturnsFalse()
    {
        var result = await Error.IsErrorAndAsync(v => Task.FromResult(v != ErrorMessage));

        Assert.False(result);
    }

    [Fact]
    public async Task UnwrapOrElseAsync_OnSuccess_ReturnsValue()
    {
        var result = await Success.UnwrapOrElseAsync(_ => TaskOtherValue);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task UnwrapOrElseAsync_OnError_ReturnsDefaultValue()
    {
        var result = await Error.UnwrapOrElseAsync(_ => TaskOtherValue);

        Assert.Same(OtherValue, result);
    }

    [Fact]
    public async Task TapAsync_OnSuccess_CallsOnSuccess()
    {
        var onSuccess = Substitute.For<Func<object, Task>>();
        await Success.TapAsync(onSuccess);

        Assert.Single((IEnumerable)onSuccess.ReceivedCalls());
    }

    [Fact]
    public async Task TapAsync_OnError_DoesNotCallOnSuccess()
    {
        var onSuccess = Substitute.For<Func<object, Task>>();
        await Error.TapAsync(onSuccess);

        Assert.Empty(onSuccess.ReceivedCalls());
    }

    [Fact]
    public async Task TapErrorAsync_OnSuccess_DoesNotCallOnError()
    {
        var onError = Substitute.For<Func<object?, Task>>();
        await Success.TapErrorAsync(onError);

        Assert.Empty(onError.ReceivedCalls());
    }

    [Fact]
    public async Task TapErrorAsync_OnError_CallsOnError()
    {
        var onError = Substitute.For<Func<object?, Task>>();
        await Error.TapErrorAsync(onError);

        Assert.Single((IEnumerable)onError.ReceivedCalls());
    }

    [Fact]
    public async Task FilterOrAsync_OnSuccessWithPredicateTrue_ReturnsSuccess()
    {
        var result = await Success.FilterOrAsync(v =>
        {
            Assert.Equal(Value, v);
            return Task.FromResult(true);
        }, Error);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task FilterOrAsync_OnSuccessWithPredicateFalse_ReturnsError()
    {
        var result = await Success.FilterOrAsync(v =>
        {
            Assert.Equal(Value, v);
            return Task.FromResult(false);
        }, ErrorMessage);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task FilterOrAsync_OnError_ReturnsError()
    {
        var result = await Error.FilterOrAsync(_ =>
        {
            Assert.Fail();
            return Task.FromResult(true);
        }, OtherErrorMessage);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task FilterOrElseAsync_OnSuccessWithPredicateTaskTrue_ReturnsSuccess()
    {
        var result = await Success.FilterOrElseAsync(v =>
        {
            Assert.Equal(Value, v);
            return Task.FromResult(true);
        }, _ =>
        {
            Assert.Fail();
            return ErrorMessage;
        });

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task FilterOrElseAsync_OnSuccessWithPredicateTaskFalse_ReturnsError()
    {
        var result = await Success.FilterOrElseAsync(v =>
        {
            Assert.Equal(Value, v);
            return Task.FromResult(false);
        }, v =>
        {
            Assert.Equal(Value, v);
            return ErrorMessage;
        });

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task FilterOrElseAsync_OnErrorWithPredicateTask_ReturnsError()
    {
        var result = await Error.FilterOrElseAsync(v =>
        {
            Assert.Fail();
            return Task.FromResult(false);
        }, v =>
        {
            Assert.Fail();
            return OtherErrorMessage;
        });

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task FilterOrElseAsync_OnSuccessWithPredicateTrueAndErrorProviderTask_ReturnsSuccess()
    {
        var result = await Success.FilterOrElseAsync(v =>
        {
            Assert.Equal(Value, v);
            return true;
        }, _ =>
        {
            Assert.Fail();
            return TaskErrorMessage;
        });

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task FilterOrElseAsync_OnSuccessWithPredicateFalseAndErrorProviderTask_ReturnsError()
    {
        var result = await Success.FilterOrElseAsync(v =>
        {
            Assert.Equal(Value, v);
            return false;
        }, v =>
        {
            Assert.Equal(Value, v);
            return TaskErrorMessage;
        });

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task FilterOrElseAsync_OnErrorAndErrorProviderTask_ReturnsError()
    {
        var result = await Error.FilterOrElseAsync(v =>
        {
            Assert.Fail();
            return false;
        }, v =>
        {
            Assert.Fail();
            return TaskOtherErrorMessage;
        });

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task FilterOrElseAsync_OnSuccessWithPredicateTaskTrueAndErrorProviderTask_ReturnsSuccess()
    {
        var result = await Success.FilterOrElseAsync(v =>
        {
            Assert.Equal(Value, v);
            return Task.FromResult(true);
        }, _ =>
        {
            Assert.Fail();
            return TaskErrorMessage;
        });

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task FilterOrElseAsync_OnSuccessWithPredicateTaskFalseAndErrorProviderTask_ReturnsError()
    {
        var result = await Success.FilterOrElseAsync(v =>
        {
            Assert.Equal(Value, v);
            return Task.FromResult(false);
        }, v =>
        {
            Assert.Equal(Value, v);
            return TaskErrorMessage;
        });

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task FilterOrElseAsync_OnErrorWithPredicateAndErrorProviderTask_ReturnsError()
    {
        var result = await Error.FilterOrElseAsync(v =>
        {
            Assert.Fail();
            return Task.FromResult(false);
        }, v =>
        {
            Assert.Fail();
            return TaskOtherError;
        });

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task MapAsync_OnSuccess_ReturnsSuccess()
    {
        var result = await Success.MapAsync(_ => TaskOtherValue);

        AssertResult.Success(OtherValue, result);
    }

    [Fact]
    public async Task MapAsync_OnError_ReturnsError()
    {
        var result = await Error.MapAsync(_ => TaskValue);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task MapErrorAsync_OnSuccess_ReturnsSuccess()
    {
        var result = await Success.MapErrorAsync(_ => TaskErrorMessage);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task MapErrorAsync_OnError_ReturnsError()
    {
        var result = await Error.MapErrorAsync(_ => TaskOtherErrorMessage);

        AssertResult.Error(OtherErrorMessage, result);
    }

    [Fact]
    public async Task MapOrAsync_OnSuccess_ReturnsSuccess()
    {
        var result = await Success.MapOrAsync(OtherValue, Task.FromResult);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task MapOrAsync_OnError_ReturnsError()
    {
        var result = await Error.MapOrAsync(Value, Task.FromResult);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task MapOrElseAsyncWithSelectorAsync_OnSuccess_ReturnsSuccess()
    {
        var result = await Success.MapOrElseAsync(_ => OtherValue, Task.FromResult);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task MapOrElseAsyncWithSelectorAsync_OnError_ReturnsError()
    {
        var result = await Error.MapOrElseAsync(_ => Value, Task.FromResult);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task MapOrElseAsyncWithSelectorAsync_OnSuccess_DoesNotCallDefaultValueProvider()
    {
        var defaultValueProvider = Substitute.For<Func<object?, object>>();
        defaultValueProvider.Invoke(Arg.Any<object?>()).Returns(OtherValue);

        await Success.MapOrElseAsync(defaultValueProvider, Task.FromResult);

        Assert.Empty(defaultValueProvider.ReceivedCalls());
    }

    [Fact]
    public async Task MapOrElseAsyncWithDefaultValueProviderAsync_OnSuccess_ReturnsSuccess()
    {
        var result = await Success.MapOrElseAsync(_ => TaskOtherValue, v => v);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task MapOrElseAsyncWithDefaultValueProviderAsync_OnError_ReturnsError()
    {
        var result = await Error.MapOrElseAsync(_ => TaskValue, v => v);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task MapOrElseAsyncWithDefaultValueProviderAsync_OnSuccess_DoesNotCallDefaultValueProvider()
    {
        var defaultValueProvider = Substitute.For<Func<object?, Task<object>>>();
        defaultValueProvider.Invoke(Arg.Any<object?>()).Returns(TaskOtherValue);

        await Success.MapOrElseAsync(defaultValueProvider, v => v);

        Assert.Empty(defaultValueProvider.ReceivedCalls());
    }

    [Fact]
    public async Task MapOrElseAsyncWithSelectorAndDefaultValueProviderAsync_OnSuccess_ReturnsSuccess()
    {
        var result = await Success.MapOrElseAsync(_ => TaskOtherValue, Task.FromResult);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task MapOrElseAsyncWithSelectorAndDefaultValueProviderAsync_OnError_ReturnsError()
    {
        var result = await Error.MapOrElseAsync(_ => TaskValue, Task.FromResult);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task MapOrElseAsyncWithSelectorAndDefaultValueProviderAsync_OnSuccess_DoesNotCallDefaultValueProvider()
    {
        var defaultValueProvider = Substitute.For<Func<object?, Task<object>>>();
        defaultValueProvider.Invoke(Arg.Any<object?>()).Returns(TaskOtherValue);

        await Success.MapOrElseAsync(defaultValueProvider, Task.FromResult);

        Assert.Empty(defaultValueProvider.ReceivedCalls());
    }

    [Fact]
    public async Task AndThenAsync_OnSuccessAndSuccess_ReturnsRightSuccess()
    {
        var result = await Success.AndThenAsync(_ => TaskOtherSuccess);

        AssertResult.Success(OtherValue, result);
    }

    [Fact]
    public async Task AndThenAsync_OnSuccessAndNone_ReturnsError()
    {
        var result = await Success.AndThenAsync(_ => TaskError);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task AndThenAsync_OnErrorAndSuccess_ReturnsError()
    {
        var result = await Error.AndThenAsync(_ => TaskSuccess);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task AndThenAsync_OnErrorAndError_ReturnsLeftError()
    {
        var result = await Error.AndThenAsync(_ => TaskOtherError);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task AndThenAsync_OnError_DoesNotCallResultProvider()
    {
        var resultProvider = Substitute.For<Func<object, Task<Result<object, object>>>>();
        resultProvider.Invoke(Arg.Any<object>()).Returns(TaskOtherSuccess);

        await Error.AndThenAsync(resultProvider);

        Assert.Empty(resultProvider.ReceivedCalls());
    }

    [Fact]
    public async Task OrElseAsync_OnSuccessAndSuccess_ReturnsLeftSuccess()
    {
        var result = await Success.OrElseAsync(_ => TaskOtherSuccess);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task OrElseAsync_OnSuccessAndNone_ReturnsSuccess()
    {
        var result = await Success.OrElseAsync(_ => TaskError);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task OrElseAsync_OnErrorAndSuccess_ReturnsSuccess()
    {
        var result = await Error.OrElseAsync(_ => TaskSuccess);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task OrElseAsync_OnErrorAndError_ReturnsRightError()
    {
        var result = await Error.OrElseAsync(_ => TaskOtherError);

        AssertResult.Error(OtherErrorMessage, result);
    }

    [Fact]
    public async Task OrElseAsync_OnSuccess_DoesNotCallResultProvider()
    {
        var resultProvider = Substitute.For<Func<object?, Task<Result<object, object>>>>();
        resultProvider.Invoke(Arg.Any<object>()).Returns(TaskOtherSuccess);

        await Success.OrElseAsync(resultProvider);

        Assert.Empty(resultProvider.ReceivedCalls());
    }
}