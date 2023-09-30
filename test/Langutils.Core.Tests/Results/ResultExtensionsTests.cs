using System.Collections;
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
    public void Map_OnSuccess_ReturnsSuccess()
    {
        var result = Success.Map(v => v);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public void Map_OnError_ReturnsError()
    {
        var result = Error.Map(_ => OtherValue);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void MapError_OnSuccess_ReturnsSuccess()
    {
        var result = Success.MapError(_ => OtherErrorMessage);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public void MapError_OnError_ReturnsError()
    {
        var result = Error.MapError(_ => OtherErrorMessage);

        AssertResult.Error(OtherErrorMessage, result);
    }

    [Fact]
    public void MapOr_OnSuccess_ReturnsSuccess()
    {
        var result = Success.MapOr(OtherValue, v => v);

        Assert.Same(Value, result);
    }

    [Fact]
    public void MapOr_OnError_ReturnsError()
    {
        var result = Error.MapOr(Value, _ => OtherValue);

        Assert.Same(Value, result);
    }

    [Fact]
    public void MapOrElse_OnSuccess_ReturnsSuccess()
    {
        var result = Success.MapOrElse(_ => OtherValue, v => v);

        Assert.Same(Value, result);
    }

    [Fact]
    public void MapOrElse_OnError_ReturnsError()
    {
        var result = Error.MapOrElse(_ => Value, _ => OtherValue);

        Assert.Same(Value, result);
    }

    [Fact]
    public void MapOrElse_OnSuccess_DoesNotCallDefaultValueProvider()
    {
        var defaultValueProvider = Substitute.For<Func<object?, object>>();
        defaultValueProvider.Invoke(Arg.Any<object?>()).Returns(OtherValue);

        Success.MapOrElse(defaultValueProvider, v => v);

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
        var result = Success.AndThen(_ => OtherSuccess);

        AssertResult.Success(OtherValue, result);
    }

    [Fact]
    public void AndThen_OnSuccessAndNone_ReturnsError()
    {
        var result = Success.AndThen(_ => Error);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void AndThen_OnErrorAndSuccess_ReturnsError()
    {
        var result = Error.AndThen(_ => Success);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void AndThen_OnErrorAndError_ReturnsLeftError()
    {
        var result = Error.AndThen(_ => OtherError);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void AndThen_OnError_DoesNotCallResultProvider()
    {
        var resultProvider = Substitute.For<Func<object, Result<object, object>>>();
        resultProvider.Invoke(Arg.Any<object>()).Returns(OtherSuccess);

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
        var result = Success.OrElse(_ => OtherSuccess);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public void OrElse_OnSuccessAndNone_ReturnsSuccess()
    {
        var result = Success.OrElse(_ => Error);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public void OrElse_OnErrorAndSuccess_ReturnsSuccess()
    {
        var result = Error.OrElse(_ => Success);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public void OrElse_OnErrorAndError_ReturnsRightError()
    {
        var result = Error.OrElse(_ => OtherError);

        AssertResult.Error(OtherErrorMessage, result);
    }

    [Fact]
    public void OrElse_OnSuccess_DoesNotCallResultProvider()
    {
        var resultProvider = Substitute.For<Func<object?, Result<object, object>>>();
        resultProvider.Invoke(Arg.Any<object>()).Returns(OtherSuccess);

        Success.OrElse(resultProvider);

        Assert.Empty(resultProvider.ReceivedCalls());
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(0, 0)]
    public void CompareTo_OnSuccessAndSuccess_ReturnsValueComparison(int left, int right)
    {
        var result = Result.Success<int, object>(left).CompareTo(Result.Success<int, object>(right));

        Assert.Equal(left.CompareTo(right), result);
    }

    [Fact]
    public void CompareTo_OnSuccessAndError_Returns1()
    {
        var result = Result.Success<int, object>(1).CompareTo(Result.Error<int, object>(ErrorMessage));

        Assert.Equal(1, result);
    }

    [Fact]
    public void CompareTo_OnErrorAndSuccess_ReturnsMinus1()
    {
        var result = Result.Error<int, object>(ErrorMessage).CompareTo(Result.Success<int, object>(1));

        Assert.Equal(-1, result);
    }

    [Fact]
    public void CompareTo_OnErrorAndError_Returns0()
    {
        var result = Result.Error<int, object>(ErrorMessage).CompareTo(Result.Error<int, object>(ErrorMessage));

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

    [Fact]
    public void Aggregate_OnAllSuccess_ReturnsSuccessWithAggregate()
    {
        var result = new[]
            {
                Result.Success<int, object>(0),
                Result.Success<int, object>(1),
                Result.Success<int, object>(2)
            }
            .Aggregate((a, b) => a + b);

        AssertResult.Success(3, result);
    }

    [Fact]
    public void Aggregate_OnMixed_ReturnsError()
    {
        var result = new[]
            {
                Result.Success<int, object>(0),
                Result.Error<int, object>(ErrorMessage)
            }
            .Aggregate((a, b) => a + b);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void Aggregate_OnErrorSuccess_ReturnsError()
    {
        var result = new[]
            {
                Result.Error<int, object>(ErrorMessage),
                Result.Success<int, object>(0)
            }
            .Aggregate((a, b) => a + b);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void Aggregate_OnAllError_ReturnsError()
    {
        var result = new[]
            {
                Result.Error<int, object>(ErrorMessage),
                Result.Error<int, object>(OtherErrorMessage)
            }
            .Aggregate((a, b) => a + b);

        AssertResult.Error(ErrorMessage,result);
    }

    [Fact]
    public void Aggregate_OnEmpty_ThrowsInvalidOperationException()
    {
        var result = Assert.Throws<InvalidOperationException>(() => Array
            .Empty<Result<int, object>>()
            .Aggregate((a, b) => a + b));

        Assert.Equal("Sequence contains no elements", result.Message);
    }

    [Fact]
    public void AggregateWithDefault_OnAllSuccess_ReturnsSuccessWithAggregate()
    {
        var result = new[]
            {
                Result.Success<int, object>(0),
                Result.Success<int, object>(1),
                Result.Success<int, object>(2)
            }
            .Aggregate(1, (a, b) => a + b);

        AssertResult.Success(4, result);
    }

    [Fact]
    public void AggregateWithDefault_OnMixed_ReturnsError()
    {
        var result = new[]
            {
                Result.Success<int, object>(0),
                Result.Error<int, object>(ErrorMessage)
            }
            .Aggregate(1, (a, b) => a + b);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void AggregateWithDefault_OnErrorSuccess_ReturnsError()
    {
        var result = new[]
            {
                Result.Error<int, object>(ErrorMessage),
                Result.Success<int, object>(0)
            }
            .Aggregate(1, (a, b) => a + b);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void AggregateWithDefault_OnAllError_ReturnsError()
    {
        var result = new[]
            {
                Result.Error<int, object>(ErrorMessage),
                Result.Error<int, object>(OtherErrorMessage)
            }
            .Aggregate(1, (a, b) => a + b);

        AssertResult.Error(ErrorMessage, result);
    }

    [Fact]
    public void AggregateWithDefault_OnEmpty_ReturnsSuccessWithZero()
    {
        var result = Array.Empty<Result<int, object>>()
            .Aggregate(1, (a, b) => a + b);

        AssertResult.Success(1, result);
    }
}