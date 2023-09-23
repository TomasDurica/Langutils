using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Langutils.Core.Options;
using Langutils.Core.Results;
using Langutils.Core.Tests.Asserts;
using NSubstitute;

namespace Langutils.Core.Tests.Results;

public class ResultExtensionsTests
{
    private static readonly object Value = new();
    private static readonly object OtherValue = new();
    private static readonly object ErrorMessage = new();
    private static readonly object OtherErrorMessage = new();
    private static readonly Result<object, object> Success = Result.Success<object, object>(Value);
    private static readonly Result<object, object> OtherSuccess = Result.Success<object, object>(OtherValue);
    private static readonly Result<object, object> Error = Result.Error<object, object>(ErrorMessage);
    private static readonly Result<object, object> OtherError = Result.Error<object, object>(OtherErrorMessage);

    [Fact]
    public void IsSuccessAnd_OnSuccess_WhenMatchesPredicate_ReturnsTrue()
    {
        var result = Success.IsSuccessAnd(v => v == Value);

        Assert.True(result);
    }

    [Fact]
    public void IsSuccessAnd_OnSuccess_WhenDoesNotMatchPredicate_ReturnsFalse()
    {
        var result = Success.IsSuccessAnd(v => v != Value);

        Assert.False(result);
    }

    [Fact]
    public void IsSuccessAnd_OnError_ReturnsFalse()
    {
        var result = Error.IsSuccessAnd(_ => true);

        Assert.False(result);
    }

    [Fact]
    public void IsErrorAnd_OnSuccess_ReturnsFalse()
    {
        var result = Success.IsErrorAnd(_ => true);

        Assert.False(result);
    }

    [Fact]
    public void IsErrorAnd_OnError_WhenMatchesPredicate_ReturnsTrue()
    {
        var result = Error.IsErrorAnd(v => v == ErrorMessage);

        Assert.True(result);
    }

    [Fact]
    public void IsErrorAnd_OnError_WhenDoesNotMatchPredicate_ReturnsFalse()
    {
        var result = Error.IsErrorAnd(v => v != ErrorMessage);

        Assert.False(result);
    }

    [Fact]
    public void Expect_OnSuccess_ReturnsValue()
    {
        var result = Success.Expect("");

        Assert.Same(Value, result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Error")]
    public void Expect_OnError_Throws(string message)
    {
        var exception = Assert.Throws<InvalidOperationException>(() => Error.Expect(message));

        Assert.Same(message, exception.Message);
    }

    [Fact]
    public void TryUnwrap_OnSuccess_ReturnsTrue()
    {
        var result = Success.TryUnwrap(out var value);

        Assert.True(result);
        Assert.Same(Value, value);
    }

    [Fact]
    public void TryUnwrap_OnError_ReturnsFalse()
    {
        var result = Error.TryUnwrap(out _);

        Assert.False(result);
    }

    [Fact]
    public void Unwrap_OnSuccess_ReturnsValue()
    {
        var result = Success.Unwrap();

        Assert.Same(Value, result);
    }

    [Fact]
    public void Unwrap_OnError_Throws()
        => Assert.Throws<InvalidOperationException>(() => Error.Unwrap());

    [Fact]
    public void UnwrapOr_OnSuccess_ReturnsValue()
    {
        var result = Success.UnwrapOr(OtherValue);

        Assert.Same(Value, result);
    }

    [Fact]
    public void UnwrapOr_OnError_ReturnsDefaultValue()
    {
        var result = Error.UnwrapOr(OtherValue);

        Assert.Same(OtherValue, result);
    }

    [Fact]
    public void UnwrapOrDefault_OnSuccess_ReturnsValue()
    {
        var result = Success.UnwrapOrDefault();

        Assert.Same(Value, result);
    }

    [Fact]
    public void UnwrapOrDefault_OnError_ReturnsDefaultValue()
    {
        var result = Error.UnwrapOrDefault();

        Assert.Same(default, result);
    }

    [Fact]
    public void UnwrapOrElse_OnSuccess_ReturnsValue()
    {
        var result = Success.UnwrapOrElse(_ => OtherValue);

        Assert.Same(Value, result);
    }

    [Fact]
    public void UnwrapOrElse_OnError_ReturnsDefaultValue()
    {
        var result = Error.UnwrapOrElse(_ => OtherValue);

        Assert.Same(OtherValue, result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Error")]
    public void ExpectError_OnSuccess_Throws(string message)
    {
        var exception = Assert.Throws<InvalidOperationException>(() => Success.ExpectError(message));

        Assert.Same(message, exception.Message);
    }

    [Fact]
    public void ExpectError_OnError_ReturnsError()
    {
        var result = Error.ExpectError("");

        Assert.Same(ErrorMessage, result);
    }

    [Fact]
    public void TryUnwrapError_OnSuccess_ReturnsFalse()
    {
        var result = Success.TryUnwrapError(out _);

        Assert.False(result);
    }

    [Fact]
    public void TryUnwrapError_OnError_ReturnsTrue()
    {
        var result = Error.TryUnwrapError(out var error);

        Assert.True(result);
        Assert.Same(ErrorMessage, error);
    }

    [Fact]
    public void UnwrapError_OnSuccess_Throws()
        => Assert.Throws<InvalidOperationException>(() => Success.UnwrapError());

    [Fact]
    public void UnwrapError_OnError_ReturnsError()
    {
        var result = Error.UnwrapError();

        Assert.Same(ErrorMessage, result);
    }

    [Fact]
    public void Success_OnSuccess_ReturnsSome()
    {
        var result = Success.Success();

        AssertOption.Some(Value, result);
    }

    [Fact]
    public void Success_OnError_ReturnsNone()
    {
        var result = Error.Success();

        AssertOption.None(result);
    }

    [Fact]
    public void Error_OnSuccess_ReturnsNone()
    {
        var result = Success.Error();

        AssertOption.None(result);
    }

    [Fact]
    public void Error_OnError_ReturnsSome()
    {
        var result = Error.Error();

        AssertOption.Some(ErrorMessage, result);
    }

    [Fact]
    public void Transpose_OnSuccessSome_ReturnsSome()
    {
        var result = Result.Success<Option<object>, object>(Option.Some(Value)).Transpose();

        AssertOption.Some(Success, result);
    }

    [Fact]
    public void Transpose_OnSuccessNone_ReturnsNone()
    {
        var result = Result.Success<Option<object>, object>(Option.None<object>()).Transpose();

        AssertOption.None(result);
    }

    [Fact]
    public void Transpose_OnError_ReturnsSome()
    {
        var result = Result.Error<Option<object>, object>(ErrorMessage).Transpose();

        AssertOption.Some(Error, result);
    }

    [Fact]
    public void Tap_OnSuccess_CallsOnSuccess()
    {
        var onSuccess = Substitute.For<Action<object>>();
        Success.Tap(onSuccess);

        Assert.Single((IEnumerable)onSuccess.ReceivedCalls());
    }

    [Fact]
    public void Tap_OnError_DoesNotCallOnSuccess()
    {
        var onSuccess = Substitute.For<Action<object>>();
        Error.Tap(onSuccess);

        Assert.Empty(onSuccess.ReceivedCalls());
    }

    [Fact]
    public void TapError_OnSuccess_DoesNotCallOnError()
    {
        var onError = Substitute.For<Action<object?>>();
        Success.TapError(onError);

        Assert.Empty(onError.ReceivedCalls());
    }

    [Fact]
    public void TapError_OnError_CallsOnError()
    {
        var onError = Substitute.For<Action<object?>>();
        Error.TapError(onError);

        Assert.Single((IEnumerable)onError.ReceivedCalls());
    }

    [Fact]
    public void SelectMany_OnSuccessSuccess_ReturnsSuccess()
    {
        var result = Success.SelectMany(_ => OtherSuccess);

        AssertResult.Success(OtherValue, result);
    }

    [Fact]
    public void SelectMany_OnSuccessError_ReturnsError()
    {
        var result = Success.SelectMany(_ => OtherError);

        AssertResult.Error(OtherErrorMessage, result);
    }

    [Fact]
    public void SelectMany_OnError_ReturnsError()
    {
        var result = Error.SelectMany(_ => OtherSuccess);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void Flatten_OnSuccessSuccess_ReturnsSuccess()
    {
        var result = Result.Success<Result<object, object>, object>(Success).Flatten();

        AssertResult.Success(Value, result);
    }

    [Fact]
    public void Flatten_OnSuccessError_ReturnsError()
    {
        var result = Result.Success<Result<object, object>, object>(Error).Flatten();

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void Flatten_OnError_ReturnsError()
    {
        var result = Result.Error<Result<object, object>, object>(ErrorMessage).Flatten();

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void Select_OnSuccess_ReturnsSuccess()
    {
        var result = Success.Select(Result.Success);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public void Select_OnError_ReturnsError()
    {
        var result = Error.Select(_ => OtherValue);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void SelectError_OnSuccess_ReturnsSuccess()
    {
        var result = Success.SelectError(_ => OtherErrorMessage);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public void SelectError_OnError_ReturnsError()
    {
        var result = Error.SelectError(_ => OtherErrorMessage);

        AssertResult.Error(OtherErrorMessage, result);
    }

    [Fact]
    public void SelectOr_OnSuccess_ReturnsSuccess()
    {
        var result = Success.SelectOr(OtherValue, v => v);

        Assert.Same(Value, result);
    }

    [Fact]
    public void SelectOr_OnError_ReturnsError()
    {
        var result = Error.SelectOr(Value, _ => OtherValue);

        Assert.Same(Value, result);
    }

    [Fact]
    public void SelectOrElse_OnSuccess_ReturnsSuccess()
    {
        var result = Success.SelectOrElse(() => OtherValue, v => v);

        Assert.Same(Value, result);
    }

    [Fact]
    public void SelectOrElse_OnError_ReturnsError()
    {
        var result = Error.SelectOrElse(() => Value, _ => OtherValue);

        Assert.Same(Value, result);
    }

    [Fact]
    public void SelectOrElse_OnSuccess_DoesNotCallDefaultValueProvider()
    {
        var defaultValueProvider = Substitute.For<Func<object>>();
        defaultValueProvider.Invoke().Returns(OtherValue);

        Success.SelectOrElse(defaultValueProvider, v => v);

        Assert.Empty(defaultValueProvider.ReceivedCalls());
    }

    [Fact]
    public void AsEnumerable_OnSuccess_ReturnsEnumerable()
    {
        var result = Success.AsEnumerable();

        Assert.Collection(result, v => Assert.Same(Value, v));
    }

    [Fact]
    public void AsEnumerable_OnError_ReturnsEmptyEnumerable()
    {
        var result = Error.AsEnumerable();

        Assert.Empty(result);
    }

    [Fact]
    public void ToList_OnSuccess_ReturnsList()
    {
        var result = Success.ToList();

        Assert.Collection(result, v => Assert.Same(Value, v));
    }

    [Fact]
    public void ToList_OnError_ReturnsEmptyList()
    {
        var result = Error.ToList();

        Assert.Empty(result);
    }

    [Fact]
    public void ToArray_OnSuccess_ReturnsArray()
    {
        var result = Success.ToArray();

        Assert.Collection(result, v => Assert.Same(Value, v));
    }

    [Fact]
    public void ToArray_OnError_ReturnsEmptyArray()
    {
        var result = Error.ToArray();

        Assert.Empty(result);
    }

    [Fact]
    public void And_OnSuccessAndSuccess_ReturnsRightSuccess()
    {
        var result = Success.And(OtherSuccess);

        AssertResult.Success(OtherValue, result);
    }

    [Fact]
    public void And_OnSuccessAndNone_ReturnsError()
    {
        var result = Success.And(Error);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void And_OnErrorAndSuccess_ReturnsError()
    {
        var result = Error.And(Success);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void And_OnErrorAndError_ReturnsLeftError()
    {
        var result = Error.And(OtherError);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void AndThen_OnSuccessAndSuccess_ReturnsRightSuccess()
    {
        var result = Success.AndThen(() => OtherSuccess);

        AssertResult.Success(OtherValue, result);
    }

    [Fact]
    public void AndThen_OnSuccessAndNone_ReturnsError()
    {
        var result = Success.AndThen(() => Error);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void AndThen_OnErrorAndSuccess_ReturnsError()
    {
        var result = Error.AndThen(() => Success);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void AndThen_OnErrorAndError_ReturnsLeftError()
    {
        var result = Error.AndThen(() => OtherError);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void AndThen_OnError_DoesNotCallResultProvider()
    {
        var resultProvider = Substitute.For<Func<Result<object, object>>>();
        resultProvider.Invoke().Returns(OtherSuccess);

        Error.AndThen(resultProvider);

        Assert.Empty(resultProvider.ReceivedCalls());
    }

    [Fact]
    public void Or_OnSuccessAndSuccess_ReturnsLeftSuccess()
    {
        var result = Success.Or(OtherSuccess);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public void Or_OnSuccessAndNone_ReturnsSuccess()
    {
        var result = Success.Or(Error);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public void Or_OnErrorAndSuccess_ReturnsSuccess()
    {
        var result = Error.Or(Success);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public void Or_OnErrorAndError_ReturnsRightError()
    {
        var result = Error.Or(OtherError);

        AssertResult.Error(OtherErrorMessage, result);
    }

    [Fact]
    public void OrElse_OnSuccessAndSuccess_ReturnsLeftSuccess()
    {
        var result = Success.OrElse(() => OtherSuccess);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public void OrElse_OnSuccessAndNone_ReturnsSuccess()
    {
        var result = Success.OrElse(() => Error);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public void OrElse_OnErrorAndSuccess_ReturnsSuccess()
    {
        var result = Error.OrElse(() => Success);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public void OrElse_OnErrorAndError_ReturnsRightError()
    {
        var result = Error.OrElse(() => OtherError);

        AssertResult.Error(OtherErrorMessage, result);
    }

    [Fact]
    public void OrElse_OnSuccess_DoesNotCallResultProvider()
    {
        var resultProvider = Substitute.For<Func<Result<object, object>>>();
        resultProvider.Invoke().Returns(OtherSuccess);

        Success.OrElse(resultProvider);

        Assert.Empty(resultProvider.ReceivedCalls());
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(0, 0)]
    public void CompareTo_OnSuccessAndSuccess_ReturnsValueComparison(int left, int right)
    {
        var result = Result.Success(left).CompareTo(Result.Success(right));

        Assert.Equal(left.CompareTo(right), result);
    }

    [Fact]
    public void CompareTo_OnSuccessAndError_Returns1()
    {
        var result = Result.Success(1).CompareTo(Result.Error<int>(""));

        Assert.Equal(1, result);
    }

    [Fact]
    public void CompareTo_OnErrorAndSuccess_ReturnsMinus1()
    {
        var result = Result.Error<int>("").CompareTo(Result.Success(1));

        Assert.Equal(-1, result);
    }

    [Fact]
    public void CompareTo_OnErrorAndError_Returns0()
    {
        var result = Result.Error<int>("").CompareTo(Result.Error<int>(""));

        Assert.Equal(0, result);
    }

    [Fact]
    public void CollectArray_OnAllSuccess_ReturnsSuccess()
    {
        var result = new[]
            {
                Success,
                OtherSuccess
            }
            .Collect();

        AssertResult.Success(new[] { Value, OtherValue }, result);
    }

    [Fact]
    public void CollectArray_OnAllError_ReturnsError()
    {
        var result = new[]
            {
                Error,
                OtherError
            }
            .Collect();

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void CollectArray_OnMixed_ReturnsError()
    {
        var result = new[]
            {
                Success,
                Error
            }
            .Collect();

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void CollectArray_OnEmpty_ReturnsSuccess()
    {
        var result = Array.Empty<Result<object, object>>()
            .Collect();

        AssertResult.Success(Array.Empty<object>(), result);
    }

    [Fact]
    public void CollectList_OnAllSuccess_ReturnsSuccess()
    {
        var result = new List<Result<object, object>>
            {
                Success,
                OtherSuccess
            }
            .Collect();

        AssertResult.Success(new List<object> { Value, OtherValue }, result);
    }

    [Fact]
    public void CollectList_OnAllError_ReturnsError()
    {
        var result = new List<Result<object, object>>
            {
                Error,
                OtherError
            }
            .Collect();

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void CollectList_OnMixed_ReturnsError()
    {
        var result = new List<Result<object, object>>
            {
                Success,
                Error
            }
            .Collect();

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void CollectList_OnEmpty_ReturnsSuccess()
    {
        var result = new List<Result<object, object>>()
            .Collect();

        AssertResult.Success(new List<object>(), result);
    }

    [Fact]
    public void CollectEnumerable_OnAllSuccess_ReturnsSuccess()
    {
        var result = new[]
            {
                Success,
                OtherSuccess
            }
            .AsEnumerable()
            .Collect();

        AssertResult.Success(new [] { Value, OtherValue }.AsEnumerable(), result);
    }

    [Fact]
    public void CollectEnumerable_OnAllError_ReturnsError()
    {
        var result = new[]
            {
                Error,
                OtherError
            }
            .AsEnumerable()
            .Collect();

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void CollectEnumerable_OnMixed_ReturnsError()
    {
        var result = new[]
            {
                Success,
                Error
            }
            .AsEnumerable()
            .Collect();

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void CollectEnumerable_OnEmpty_ReturnsSuccess()
    {
        var result = Array.Empty<Result<object, object>>()
            .AsEnumerable()
            .Collect();

        AssertResult.Success(Array.Empty<object>().AsEnumerable(), result);
    }
}