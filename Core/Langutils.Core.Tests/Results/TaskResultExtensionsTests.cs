using System.Collections;
using Langutils.Core.Options;
using Langutils.Core.Results;
using Langutils.Core.Tests.Asserts;
using NSubstitute;

namespace Langutils.Core.Tests.Results;

public class TaskResultExtensionsTests
{
    private static readonly object Value = new();
    private static readonly object OtherValue = new();
    private static readonly object ErrorMessage = new();
    private static readonly object OtherErrorMessage = new();
    private static readonly Task<Result<object, object>> TaskSuccess = Task.FromResult(Result.Success<object, object>(Value));
    private static readonly Task<Result<object, object>> TaskError = Task.FromResult(Result.Error<object, object>(ErrorMessage));
    private static readonly Result<object, object> Success = Result.Success<object, object>(Value);
    private static readonly Result<object, object> OtherSuccess = Result.Success<object, object>(OtherValue);
    private static readonly Result<object, object> Error = Result.Error<object, object>(ErrorMessage);
    private static readonly Result<object, object> OtherError = Result.Error<object, object>(OtherErrorMessage);

    [Fact]
    public async Task IsSuccessAnd_OnSuccess_WhenMatchesPredicate_ReturnsTrue()
    {
        var result = await TaskSuccess.IsSuccessAnd(v => v == Value);

        Assert.True(result);
    }

    [Fact]
    public async Task IsSuccessAnd_OnSuccess_WhenDoesNotMatchPredicate_ReturnsFalse()
    {
        var result = await TaskSuccess.IsSuccessAnd(v => v != Value);

        Assert.False(result);
    }

    [Fact]
    public async Task IsSuccessAnd_OnError_ReturnsFalse()
    {
        var result = await TaskError.IsSuccessAnd(_ => true);

        Assert.False(result);
    }

    [Fact]
    public async Task IsErrorAnd_OnSuccess_ReturnsFalse()
    {
        var result = await TaskSuccess.IsErrorAnd(_ => true);

        Assert.False(result);
    }

    [Fact]
    public async Task IsErrorAnd_OnError_WhenMatchesPredicate_ReturnsTrue()
    {
        var result = await TaskError.IsErrorAnd(v => v == ErrorMessage);

        Assert.True(result);
    }

    [Fact]
    public async Task IsErrorAnd_OnError_WhenDoesNotMatchPredicate_ReturnsFalse()
    {
        var result = await TaskError.IsErrorAnd(v => v != ErrorMessage);

        Assert.False(result);
    }

    [Fact]
    public async Task Expect_OnSuccess_ReturnsValue()
    {
        var result = await TaskSuccess.Expect("");

        Assert.Same(Value, result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Error")]
    public async Task Expect_OnError_Throws(string message)
    {
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => TaskError.Expect(message));

        Assert.Same(message, exception.Message);
    }

    [Fact]
    public async Task Unwrap_OnSuccess_ReturnsValue()
    {
        var result = await TaskSuccess.Unwrap();

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task Unwrap_OnError_Throws()
        => await Assert.ThrowsAsync<InvalidOperationException>(() => TaskError.Unwrap());

    [Fact]
    public async Task UnwrapOr_OnSuccess_ReturnsValue()
    {
        var result = await TaskSuccess.UnwrapOr(OtherValue);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task UnwrapOr_OnError_ReturnsDefaultValue()
    {
        var result = await TaskError.UnwrapOr(OtherValue);

        Assert.Same(OtherValue, result);
    }

    [Fact]
    public async Task UnwrapOrDefault_OnSuccess_ReturnsValue()
    {
        var result = await TaskSuccess.UnwrapOrDefault();

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task UnwrapOrDefault_OnError_ReturnsDefaultValue()
    {
        var result = await TaskError.UnwrapOrDefault();

        Assert.Same(default, result);
    }

    [Fact]
    public async Task UnwrapOrElse_OnSuccess_ReturnsValue()
    {
        var result = await TaskSuccess.UnwrapOrElse(_ => OtherValue);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task UnwrapOrElse_OnError_ReturnsDefaultValue()
    {
        var result = await TaskError.UnwrapOrElse(_ => OtherValue);

        Assert.Same(OtherValue, result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Error")]
    public async Task ExpectError_OnSuccess_Throws(string message)
    {
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => TaskSuccess.ExpectError(message));

        Assert.Same(message, exception.Message);
    }

    [Fact]
    public async Task ExpectError_OnError_ReturnsError()
    {
        var result = await TaskError.ExpectError("");

        Assert.Same(ErrorMessage, result);
    }

    [Fact]
    public async Task UnwrapError_OnSuccess_Throws()
        => await Assert.ThrowsAsync<InvalidOperationException>(() => TaskSuccess.UnwrapError());

    [Fact]
    public async Task UnwrapError_OnError_ReturnsError()
    {
        var result = await TaskError.UnwrapError();

        Assert.Same(ErrorMessage, result);
    }

    [Fact]
    public async Task Success_OnSuccess_ReturnsSome()
    {
        var result = await TaskSuccess.Success();

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task Success_OnError_ReturnsNone()
    {
        var result = await TaskError.Success();

        AssertOption.None(result);
    }

    [Fact]
    public async Task Error_OnSuccess_ReturnsNone()
    {
        var result = await TaskSuccess.Error();

        AssertOption.None(result);
    }

    [Fact]
    public async Task Error_OnError_ReturnsSome()
    {
        var result = await TaskError.Error();

        AssertOption.Some(ErrorMessage, result);
    }

    [Fact]
    public async Task Transpose_OnSuccessSome_ReturnsSome()
    {
        var result = await Task.FromResult(Result.Success<Option<object>, object>(Option.Some(Value)))
            .Transpose();

        AssertOption.Some(Success, result);
    }

    [Fact]
    public async Task Transpose_OnSuccessNone_ReturnsNone()
    {
        var result = await Task.FromResult(Result.Success<Option<object>, object>(Option.None<object>()))
            .Transpose();

        AssertOption.None(result);
    }

    [Fact]
    public async Task Transpose_OnError_ReturnsSome()
    {
        var result = await Task.FromResult(Result.Error<Option<object>, object>(ErrorMessage))
            .Transpose();

        AssertOption.Some(Error, result);
    }

    [Fact]
    public async Task Tap_OnSuccess_CallsOnSuccess()
    {
        var onSuccess = Substitute.For<Action<object>>();
        await TaskSuccess.Tap(onSuccess);

        Assert.Single((IEnumerable)onSuccess.ReceivedCalls());
    }

    [Fact]
    public async Task Tap_OnError_DoesNotCallOnSuccess()
    {
        var onSuccess = Substitute.For<Action<object>>();
        await TaskError.Tap(onSuccess);

        Assert.Empty(onSuccess.ReceivedCalls());
    }

    [Fact]
    public async Task TapError_OnSuccess_DoesNotCallOnError()
    {
        var onError = Substitute.For<Action<object?>>();
        await TaskSuccess.TapError(onError);

        Assert.Empty(onError.ReceivedCalls());
    }

    [Fact]
    public async Task TapError_OnError_CallsOnError()
    {
        var onError = Substitute.For<Action<object?>>();
        await TaskError.TapError(onError);

        Assert.Single((IEnumerable)onError.ReceivedCalls());
    }

    [Fact]
    public async Task Flatten_OnSuccessSuccess_ReturnsSuccess()
    {
        var result = await Task.FromResult(Result.Success<Result<object, object>, object>(Success))
            .Flatten();

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task Flatten_OnSuccessError_ReturnsError()
    {
        var result = await Task.FromResult(Result.Success<Result<object, object>, object>(Error))
            .Flatten();

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task Flatten_OnError_ReturnsError()
    {
        var result = await Task.FromResult(Result.Error<Result<object, object>, object>(ErrorMessage))
            .Flatten();

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task Map_OnSuccess_ReturnsSuccess()
    {
        var result = await TaskSuccess.Map(v => v);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task Map_OnError_ReturnsError()
    {
        var result = await TaskError.Map(_ => Value);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task MapError_OnSuccess_ReturnsSuccess()
    {
        var result = await TaskSuccess.MapError(_ => OtherErrorMessage);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task MapError_OnError_ReturnsError()
    {
        var result = await TaskError.MapError(_ => OtherErrorMessage);

        AssertResult.Error(OtherErrorMessage, result);
    }

    [Fact]
    public async Task MapOr_OnSuccess_ReturnsSuccess()
    {
        var result = await TaskSuccess.MapOr(OtherValue, v => v);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task MapOr_OnError_ReturnsError()
    {
        var result = await TaskError.MapOr(Value, _ => OtherValue);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task MapOrElse_OnSuccess_ReturnsSuccess()
    {
        var result = await TaskSuccess.MapOrElse(_ => OtherValue, v => v);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task MapOrElse_OnError_ReturnsError()
    {
        var result = await TaskError.MapOrElse(_ => Value, _ => OtherValue);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task MapOrElse_OnSuccess_DoesNotCallDefaultValueProvider()
    {
        var defaultValueProvider = Substitute.For<Func<object?, object>>();
        defaultValueProvider.Invoke(Arg.Any<object?>()).Returns(OtherValue);

        await TaskSuccess.MapOrElse(defaultValueProvider, v => v);

        Assert.Empty(defaultValueProvider.ReceivedCalls());
    }

    [Fact]
    public async Task AsEnumerable_OnSuccess_ReturnsEnumerable()
    {
        var result = await TaskSuccess.AsEnumerable();

        Assert.Collection(result, v => Assert.Same(Value, v));
    }

    [Fact]
    public async Task AsEnumerable_OnError_ReturnsEmptyEnumerable()
    {
        var result = await TaskError.AsEnumerable();

        Assert.Empty(result);
    }

    [Fact]
    public async Task ToList_OnSuccess_ReturnsList()
    {
        var result = await TaskSuccess.ToList();

        Assert.Collection(result, v => Assert.Same(Value, v));
    }

    [Fact]
    public async Task ToList_OnError_ReturnsEmptyList()
    {
        var result = await TaskError.ToList();

        Assert.Empty(result);
    }

    [Fact]
    public async Task ToArray_OnSuccess_ReturnsArray()
    {
        var result = await TaskSuccess.ToArray();

        Assert.Collection(result, v => Assert.Same(Value, v));
    }

    [Fact]
    public async Task ToArray_OnError_ReturnsEmptyArray()
    {
        var result = await TaskError.ToArray();

        Assert.Empty(result);
    }

    [Fact]
    public async Task And_OnSuccessAndSuccess_ReturnsRightSuccess()
    {
        var result = await TaskSuccess.And(OtherSuccess);

        AssertResult.Success(OtherValue, result);
    }

    [Fact]
    public async Task And_OnSuccessAndNone_ReturnsError()
    {
        var result = await TaskSuccess.And(Error);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task And_OnErrorAndSuccess_ReturnsError()
    {
        var result = await TaskError.And(Success);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task And_OnErrorAndError_ReturnsLeftError()
    {
        var result = await TaskError.And(OtherError);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task AndThen_OnSuccessAndSuccess_ReturnsRightSuccess()
    {
        var result = await TaskSuccess.AndThen(_ => OtherSuccess);

        AssertResult.Success(OtherValue, result);
    }

    [Fact]
    public async Task AndThen_OnSuccessAndNone_ReturnsError()
    {
        var result = await TaskSuccess.AndThen(_ => Error);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task AndThen_OnErrorAndSuccess_ReturnsError()
    {
        var result = await TaskError.AndThen(_ => Success);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task AndThen_OnErrorAndError_ReturnsLeftError()
    {
        var result = await TaskError.AndThen(_ => OtherError);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task AndThen_OnError_DoesNotCallResultProvider()
    {
        var resultProvider = Substitute.For<Func<object, Result<object, object>>>();
        resultProvider.Invoke(Arg.Any<object>()).Returns(OtherSuccess);

        await TaskError.AndThen(resultProvider);

        Assert.Empty(resultProvider.ReceivedCalls());
    }

    [Fact]
    public async Task Or_OnSuccessAndSuccess_ReturnsLeftSuccess()
    {
        var result = await TaskSuccess.Or(OtherSuccess);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task Or_OnSuccessAndNone_ReturnsSuccess()
    {
        var result = await TaskSuccess.Or(Error);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task Or_OnErrorAndSuccess_ReturnsSuccess()
    {
        var result = await TaskError.Or(Success);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task Or_OnErrorAndError_ReturnsRightError()
    {
        var result = await TaskError.Or(OtherError);

        AssertResult.Error(OtherErrorMessage, result);
    }

    [Fact]
    public async Task OrElse_OnSuccessAndSuccess_ReturnsLeftSuccess()
    {
        var result = await TaskSuccess.OrElse(_ => OtherSuccess);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task OrElse_OnSuccessAndNone_ReturnsSuccess()
    {
        var result = await TaskSuccess.OrElse(_ => Error);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task OrElse_OnErrorAndSuccess_ReturnsSuccess()
    {
        var result = await TaskError.OrElse(_ => Success);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task OrElse_OnErrorAndError_ReturnsRightError()
    {
        var result = await TaskError.OrElse(_ => OtherError);

        AssertResult.Error(OtherErrorMessage, result);
    }

    [Fact]
    public async Task OrElse_OnSuccess_DoesNotCallResultProvider()
    {
        var resultProvider = Substitute.For<Func<object?, Result<object, object>>>();
        resultProvider.Invoke(Arg.Any<object>()).Returns(OtherSuccess);

        await TaskSuccess.OrElse(resultProvider);

        Assert.Empty(resultProvider.ReceivedCalls());
    }

    [Fact]
    public async Task CollectArray_OnAllSuccess_ReturnsSuccess()
    {
        var result = await Task.FromResult(new[]
            {
                Success,
                OtherSuccess
            })
            .Collect();

        AssertResult.Success(new[] { Value, OtherValue }, result);
    }

    [Fact]
    public async Task CollectArray_OnAllError_ReturnsError()
    {
        var result = await Task.FromResult(new[]
            {
                Error,
                OtherError
            })
            .Collect();

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task CollectArray_OnMixed_ReturnsError()
    {
        var result = await Task.FromResult(new[]
            {
                Success,
                Error
            })
            .Collect();

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task CollectArray_OnEmpty_ReturnsSuccess()
    {
        var result = await Task.FromResult(Array.Empty<Result<object, object>>())
            .Collect();

        AssertResult.Success(Array.Empty<object>(), result);
    }

    [Fact]
    public async Task CollectList_OnAllSuccess_ReturnsSuccess()
    {
        var result = await Task.FromResult(new List<Result<object, object>>
            {
                Success,
                OtherSuccess
            })
            .Collect();

        AssertResult.Success(new List<object> { Value, OtherValue }, result);
    }

    [Fact]
    public async Task CollectList_OnAllError_ReturnsError()
    {
        var result = await Task.FromResult(new List<Result<object, object>>
            {
                Error,
                OtherError
            })
            .Collect();

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task CollectList_OnMixed_ReturnsError()
    {
        var result = await Task.FromResult(new List<Result<object, object>>
            {
                Success,
                Error
            })
            .Collect();

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task CollectList_OnEmpty_ReturnsSuccess()
    {
        var result = await Task.FromResult(new List<Result<object, object>>())
            .Collect();

        AssertResult.Success(new List<object>(), result);
    }

    [Fact]
    public async Task CollectEnumerable_OnAllSuccess_ReturnsSuccess()
    {
        var result = await Task.FromResult(new[]
            {
                Success,
                OtherSuccess
            }.AsEnumerable())
            .Collect();

        AssertResult.Success(new [] { Value, OtherValue }.AsEnumerable(), result);
    }

    [Fact]
    public async Task CollectEnumerable_OnAllError_ReturnsError()
    {
        var result = await Task.FromResult(new[]
            {
                Error,
                OtherError
            }.AsEnumerable())
            .Collect();

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task CollectEnumerable_OnMixed_ReturnsError()
    {
        var result = await Task.FromResult(new[]
            {
                Success,
                Error
            }.AsEnumerable())
            .Collect();

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public async Task CollectEnumerable_OnEmpty_ReturnsSuccess()
    {
        var result = await Task.FromResult(Array.Empty<Result<object, object>>().AsEnumerable())
            .Collect();

        AssertResult.Success(Array.Empty<object>().AsEnumerable(), result);
    }
}