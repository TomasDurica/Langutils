using System;
using System.Collections;
using System.Threading.Tasks;
using Langutils.Core.Results;
using Langutils.Core.Tests.Asserts;
using NSubstitute;

namespace Langutils.Core.Tests.Results;

public class TaskResultAsyncExtensionsTests
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
        var result = await TaskSuccess.IsSuccessAndAsync(v => Task.FromResult(v == Value));

        Assert.True(result);
    }

    [Fact]
    public async Task IsSuccessAndAsync_OnSuccess_WhenDoesNotMatchPredicate_ReturnsFalse()
    {
        var result = await TaskSuccess.IsSuccessAndAsync(v => Task.FromResult(v != Value));

        Assert.False(result);
    }

    [Fact]
    public async Task IsSuccessAndAsync_OnError_ReturnsFalse()
    {
        var result = await TaskError.IsSuccessAndAsync(_ => Task.FromResult(true));

        Assert.False(result);
    }

    [Fact]
    public async Task IsErrorAndAsync_OnSuccess_ReturnsFalse()
    {
        var result = await TaskSuccess.IsErrorAndAsync(_ => Task.FromResult(true));

        Assert.False(result);
    }

    [Fact]
    public async Task IsErrorAndAsync_OnError_WhenMatchesPredicate_ReturnsTrue()
    {
        var result = await TaskError.IsErrorAndAsync(v => Task.FromResult(v == ErrorMessage));

        Assert.True(result);
    }

    [Fact]
    public async Task IsErrorAndAsync_OnError_WhenDoesNotMatchPredicate_ReturnsFalse()
    {
        var result = await TaskError.IsErrorAndAsync(v => Task.FromResult(v != ErrorMessage));

        Assert.False(result);
    }

    [Fact]
    public async Task UnwrapOrElseAsync_OnSuccess_ReturnsValue()
    {
        var result = await TaskSuccess.UnwrapOrElseAsync(_ => TaskOtherValue);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task UnwrapOrElseAsync_OnError_ReturnsDefaultValue()
    {
        var result = await TaskError.UnwrapOrElseAsync(_ => TaskOtherValue);

        Assert.Same(OtherValue, result);
    }

    [Fact]
    public async Task TapAsync_OnSuccess_CallsOnSuccess()
    {
        var onSuccess = Substitute.For<Func<object, Task>>();
        await TaskSuccess.TapAsync(onSuccess);

        Assert.Single((IEnumerable)onSuccess.ReceivedCalls());
    }

    [Fact]
    public async Task TapAsync_OnError_DoesNotCallOnSuccess()
    {
        var onSuccess = Substitute.For<Func<object, Task>>();
        await TaskError.TapAsync(onSuccess);

        Assert.Empty(onSuccess.ReceivedCalls());
    }

    [Fact]
    public async Task TapErrorAsync_OnSuccess_DoesNotCallOnError()
    {
        var onError = Substitute.For<Func<object?, Task>>();
        await TaskSuccess.TapErrorAsync(onError);

        Assert.Empty(onError.ReceivedCalls());
    }

    [Fact]
    public async Task TapErrorAsync_OnError_CallsOnError()
    {
        var onError = Substitute.For<Func<object?, Task>>();
        await TaskError.TapErrorAsync(onError);

        Assert.Single((IEnumerable)onError.ReceivedCalls());
    }

    [Fact]
    public async Task SelectManyAsync_OnSuccessSuccess_ReturnsSuccess()
    {
        var result = await TaskSuccess.SelectManyAsync(_ => TaskOtherSuccess);

        AssertResult.Success(OtherValue, result);
    }

    [Fact]
    public async Task SelectManyAsync_OnSuccessError_ReturnsError()
    {
        var result = await TaskSuccess.SelectManyAsync(_ => TaskError);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task SelectManyAsync_OnError_ReturnsError()
    {
        var result = await TaskError.SelectManyAsync(_ => TaskSuccess);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task SelectAsync_OnSuccess_ReturnsSuccess()
    {
        var result = await TaskSuccess.SelectAsync(_ => TaskOtherValue);

        AssertResult.Success(OtherValue, result);
    }

    [Fact]
    public async Task SelectAsync_OnError_ReturnsError()
    {
        var result = await TaskError.SelectAsync(_ => TaskValue);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task SelectErrorAsync_OnSuccess_ReturnsSuccess()
    {
        var result = await TaskSuccess.SelectErrorAsync(_ => TaskErrorMessage);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task SelectErrorAsync_OnError_ReturnsError()
    {
        var result = await TaskError.SelectErrorAsync(_ => TaskOtherErrorMessage);

        AssertResult.Error(OtherErrorMessage, result);
    }

    [Fact]
    public async Task SelectOrAsync_OnSuccess_ReturnsSuccess()
    {
        var result = await TaskSuccess.SelectOrAsync(OtherValue, Task.FromResult);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task SelectOrAsync_OnError_ReturnsError()
    {
        var result = await TaskError.SelectOrAsync(Value, Task.FromResult);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task SelectOrElseAsyncWithSelectorAsync_OnSuccess_ReturnsSuccess()
    {
        var result = await TaskSuccess.SelectOrElseAsync(() => OtherValue, Task.FromResult);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task SelectOrElseAsyncWithSelectorAsync_OnError_ReturnsError()
    {
        var result = await TaskError.SelectOrElseAsync(() => Value, Task.FromResult);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task SelectOrElseAsyncWithSelectorAsync_OnSuccess_DoesNotCallDefaultValueProvider()
    {
        var defaultValueProvider = Substitute.For<Func<object>>();
        defaultValueProvider.Invoke().Returns(OtherValue);

        await TaskSuccess.SelectOrElseAsync(defaultValueProvider, Task.FromResult);

        Assert.Empty(defaultValueProvider.ReceivedCalls());
    }

    [Fact]
    public async Task SelectOrElseAsyncWithDefaultValueProviderAsync_OnSuccess_ReturnsSuccess()
    {
        var result = await TaskSuccess.SelectOrElseAsync(() => TaskOtherValue, v => v);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task SelectOrElseAsyncWithDefaultValueProviderAsync_OnError_ReturnsError()
    {
        var result = await TaskError.SelectOrElseAsync(() => TaskValue, v => v);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task SelectOrElseAsyncWithDefaultValueProviderAsync_OnSuccess_DoesNotCallDefaultValueProvider()
    {
        var defaultValueProvider = Substitute.For<Func<Task<object>>>();
        defaultValueProvider.Invoke().Returns(TaskOtherValue);

        await TaskSuccess.SelectOrElseAsync(defaultValueProvider, v => v);

        Assert.Empty(defaultValueProvider.ReceivedCalls());
    }

    [Fact]
    public async Task SelectOrElseAsyncWithSelectorAndDefaultValueProviderAsync_OnSuccess_ReturnsSuccess()
    {
        var result = await TaskSuccess.SelectOrElseAsync(() => TaskOtherValue, Task.FromResult);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task SelectOrElseAsyncWithSelectorAndDefaultValueProviderAsync_OnError_ReturnsError()
    {
        var result = await TaskError.SelectOrElseAsync(() => TaskValue, Task.FromResult);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task SelectOrElseAsyncWithSelectorAndDefaultValueProviderAsync_OnSuccess_DoesNotCallDefaultValueProvider()
    {
        var defaultValueProvider = Substitute.For<Func<Task<object>>>();
        defaultValueProvider.Invoke().Returns(TaskOtherValue);

        await TaskSuccess.SelectOrElseAsync(defaultValueProvider, Task.FromResult);

        Assert.Empty(defaultValueProvider.ReceivedCalls());
    }

    [Fact]
    public async Task AndThenAsync_OnSuccessAndSuccess_ReturnsRightSuccess()
    {
        var result = await TaskSuccess.AndThenAsync(() => TaskOtherSuccess);

        AssertResult.Success(OtherValue, result);
    }

    [Fact]
    public async Task AndThenAsync_OnSuccessAndNone_ReturnsError()
    {
        var result = await TaskSuccess.AndThenAsync(() => TaskError);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task AndThenAsync_OnErrorAndSuccess_ReturnsError()
    {
        var result = await TaskError.AndThenAsync(() => TaskSuccess);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task AndThenAsync_OnErrorAndError_ReturnsLeftError()
    {
        var result = await TaskError.AndThenAsync(() => TaskOtherError);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task AndThenAsync_OnError_DoesNotCallResultProvider()
    {
        var resultProvider = Substitute.For<Func<Task<Result<object, object>>>>();
        resultProvider.Invoke().Returns(TaskOtherSuccess);

        await TaskError.AndThenAsync(resultProvider);

        Assert.Empty(resultProvider.ReceivedCalls());
    }

    [Fact]
    public async Task OrElseAsync_OnSuccessAndSuccess_ReturnsLeftSuccess()
    {
        var result = await TaskSuccess.OrElseAsync(() => TaskOtherSuccess);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task OrElseAsync_OnSuccessAndNone_ReturnsSuccess()
    {
        var result = await TaskSuccess.OrElseAsync(() => TaskError);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task OrElseAsync_OnErrorAndSuccess_ReturnsSuccess()
    {
        var result = await TaskError.OrElseAsync(() => TaskSuccess);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task OrElseAsync_OnErrorAndError_ReturnsRightError()
    {
        var result = await TaskError.OrElseAsync(() => TaskOtherError);

        AssertResult.Error(OtherErrorMessage, result);
    }

    [Fact]
    public async Task OrElseAsync_OnSuccess_DoesNotCallResultProvider()
    {
        var resultProvider = Substitute.For<Func<Task<Result<object, object>>>>();
        resultProvider.Invoke().Returns(TaskOtherSuccess);

        await TaskSuccess.OrElseAsync(resultProvider);

        Assert.Empty(resultProvider.ReceivedCalls());
    }
}