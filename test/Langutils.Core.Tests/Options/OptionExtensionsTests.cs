using System.Collections;
using Langutils.Core.Options;
using Langutils.Core.Results;
using Langutils.Core.Tests.Asserts;
using NSubstitute;

namespace Langutils.Core.Tests.Options;

public class OptionExtensionsTests
{
    private static readonly object Value = new();
    private static readonly object OtherValue = new();
    private static readonly Option<object> Some = Option.Some(Value);
    private static readonly Option<object> OtherSome = Option.Some(OtherValue);
    private static readonly Option<object> None = Option.None<object>();

    [Fact]
    public void IsSomeAnd_OnSome_WhenMatchesPredicate_ReturnsTrue()
    {
        var result = Some.IsSomeAnd(v => v == Value);

        Assert.True(result);
    }

    [Fact]
    public void IsSomeAnd_OnSome_WhenDoesNotMatchPredicate_ReturnsFalse()
    {
        var result = Some.IsSomeAnd(v => v != Value);

        Assert.False(result);
    }

    [Fact]
    public void IsSomeAnd_OnNone_ReturnsFalse()
    {
        var result = None.IsSomeAnd(_ => true);

        Assert.False(result);
    }

    [Fact]
    public void Expect_OnSome_ReturnsValue()
    {
        var result = Some.Expect(string.Empty);

        Assert.Same(Value, result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Error")]
    public void Expect_OnNone_ThrowsInvalidOperationExceptionWithMessage(string message)
    {
        var exception = Assert.Throws<InvalidOperationException>(() => None.Expect(message));

        Assert.Same(message, exception.Message);
    }

    [Fact]
    public void TryUnwrap_OnSome_ReturnsTrueWithOutValue()
    {
        var result = Some.TryUnwrap(out var unwrappedValue);

        Assert.True(result);
        Assert.Same(Value, unwrappedValue);
    }

    [Fact]
    public void TryUnwrap_OnNone_ReturnsFalse()
    {
        var result = None.TryUnwrap(out var unwrappedValue);

        Assert.False(result);
        Assert.Same(default, unwrappedValue);
    }

    [Fact]
    public void Unwrap_OnSome_ShouldReturnValue()
    {
        var result = Some.Unwrap();

        Assert.Equal(Value, result);
    }

    [Fact]
    public void Unwrap_OnNone_ShouldThrow()
        => Assert.Throws<InvalidOperationException>(() => None.Unwrap());

    [Fact]
    public void UnwrapOr_OnSome_ReturnsValue()
    {
        var result = Some.UnwrapOr(OtherValue);

        Assert.Same(Value, result);
    }

    [Fact]
    public void UnwrapOr_OnNone_ReturnsDefaultValue()
    {
        var result = None.UnwrapOr(OtherValue);

        Assert.Same(OtherValue, result);
    }

    [Fact]
    public void UnwrapOrDefault_OnSome_ReturnsValue()
    {
        var result = Some.UnwrapOrDefault();

        Assert.Same(Value, result);
    }

    [Fact]
    public void UnwrapOrDefault_OnNone_ReturnsDefaultValue()
    {
        var result = None.UnwrapOrDefault();

        Assert.Same(default, result);
    }

    [Fact]
    public void UnwrapOrElse_OnSome_ReturnsValue()
    {
        var result = Some.UnwrapOrElse(() => OtherValue);

        Assert.Same(Value, result);
    }

    [Fact]
    public void UnwrapOrElse_OnSome_DoesNotCallDefaultValueProvider()
    {
        var defaultValueProvider = Substitute.For<Func<object>>();
        defaultValueProvider.Invoke().Returns(OtherValue);

        Some.UnwrapOrElse(defaultValueProvider);

        Assert.Empty(defaultValueProvider.ReceivedCalls());
    }

    [Fact]
    public void UnwrapOrElse_OnNone_ReturnsValueFromDefaultValueProvider()
    {
        var result = None.UnwrapOrElse(() => Value);

        Assert.Same(Value, result);
    }

    [Fact]
    public void Tap_OnSome_ShouldCallOnSome()
    {
        var onSome = Substitute.For<Action<object>>();

        Some.Tap(onSome);

        Assert.Single((IEnumerable)onSome.ReceivedCalls());
        Assert.StrictEqual(Value, onSome.ReceivedCalls().First().GetArguments().First());
    }

    [Fact]
    public void Tap_OnNone_ShouldNotCallOnSome()
    {
        var onSome = Substitute.For<Action<object>>();

        None.Tap(onSome);

        Assert.Empty(onSome.ReceivedCalls());
    }

    [Fact]
    public void Filter_OnSome_WhenMatchesPredicate_ReturnsSome()
    {
        var result = Some.Filter(v => v == Value);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public void Filter_OnSome_WhenDoesNotMatchPredicate_ReturnsNone()
    {
        var result = Some.Filter(v => v != Value);

        AssertOption.None(result);
    }

    [Fact]
    public void Filter_OnNone_ReturnsNone()
    {
        var result = None.Filter(_ => true);

        AssertOption.None(result);
    }

    [Fact]
    public void Flatten_OnSomeSome_ReturnsSome()
    {
        var result = Option.Some(Some).Flatten();

        AssertOption.Equal(Some, result);
    }

    [Fact]
    public void Flatten_OnSomeNone_ReturnsNone()
    {
        var result = Option.Some(None).Flatten();

        AssertOption.None(result);
    }

    [Fact]
    public void Flatten_OnNone_ReturnsNone()
    {
        var result = Option.None<Option<object>>().Flatten();

        AssertOption.None(result);
    }

    [Fact]
    public void Map_OnSome_ReturnsSome()
    {
        var result = Some.Map(v => v);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public void Map_OnNone_ReturnsNone()
    {
        var result = None.Map(v => v);

        AssertOption.None(result);
    }

    [Fact]
    public void MapOr_OnSome_ReturnsSome()
    {
        var result = Some.MapOr(OtherValue, v => v);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public void MapOr_OnNone_ReturnsDefaultValue()
    {
        var result = None.MapOr(OtherValue, v => v);

        AssertOption.Some(OtherValue, result);
    }

    [Fact]
    public void MapOrElse_OnSome_ReturnsSome()
    {
        var result = Some.MapOrElse(() => OtherValue, v => v);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public void MapOrElse_OnSome_DoesNotCallDefaultValueProvider()
    {
        var defaultValueProvider = Substitute.For<Func<object>>();
        defaultValueProvider.Invoke().Returns(OtherValue);

        Some.MapOrElse(defaultValueProvider, v => v);

        Assert.Empty(defaultValueProvider.ReceivedCalls());
    }

    [Fact]
    public void MapOrElse_OnNone_ReturnsDefaultValue()
    {
        var result = None.MapOrElse(() => Value, v => v);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public void SomeOr_OnSome_ReturnsSome()
    {
        var result = Some.SomeOr(OtherValue);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public void SomeOr_OnNone_ReturnsError()
    {
        var result = None.SomeOr(OtherValue);

        AssertResult.Error(OtherValue, result);
    }

    [Fact]
    public void SomeOrElse_OnSome_ReturnsSome()
    {
        var result = Some.SomeOrElse(() => OtherValue);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public void SomeOrElse_OnSome_DoesNotCallDefaultValueProvider()
    {
        var defaultValueProvider = Substitute.For<Func<object>>();
        defaultValueProvider.Invoke().Returns(OtherValue);

        Some.SomeOrElse(defaultValueProvider);

        Assert.Empty(defaultValueProvider.ReceivedCalls());
    }

    [Fact]
    public void SomeOrElse_OnNone_ReturnsError()
    {
        var result = None.SomeOrElse(() => OtherValue);

        AssertResult.Error(OtherValue, result);
    }

    [Fact]
    public void Transpose_OnSomeSuccess_ReturnsSuccessSome()
    {
        var result = Option.Some(Result.Success<object, object>(Value)).Transpose();

        AssertResult.Success(Some, result);
    }

    [Fact]
    public void Transpose_OnSomeError_ReturnsError()
    {
        var result = Option.Some(Result.Error<object, object>(OtherValue)).Transpose();

        AssertResult.Error(OtherValue, result);
    }

    [Fact]
    public void Transpose_OnNone_ReturnsSuccessNone()
    {
        var result = Option.None<Result<object, object>>().Transpose();

        AssertResult.Success(None, result);
    }

    [Fact]
    public void AsEnumerable_OnSome_ReturnsEnumerableWithOneElement()
    {
        var result = Some.AsEnumerable();

        Assert.Equivalent(new [] { Value }, result);
    }

    [Fact]
    public void AsEnumerable_OnNone_ReturnsEmptyEnumerable()
    {
        var result = None.AsEnumerable();

        Assert.Empty(result);
    }

    [Fact]
    public void ToList_OnSome_ReturnsListWithOneElement()
    {
        var result = Some.ToList();

        Assert.Equivalent(new [] { Value }, result);
    }

    [Fact]
    public void ToList_OnNone_ReturnsEmptyList()
    {
        var result = None.ToList();

        Assert.Empty(result);
    }

    [Fact]
    public void ToArray_OnSome_ReturnsArrayWithOneElement()
    {
        var result = Some.ToArray();

        Assert.Equivalent(new [] { Value }, result);
    }

    [Fact]
    public void ToArray_OnNone_ReturnsEmptyArray()
    {
        var result = None.ToArray();

        Assert.Empty(result);
    }

    [Fact]
    public void And_OnSomeSome_ReturnsSomeWithRightValue()
    {
        var result = Some.And(OtherSome);

        AssertOption.Some(OtherValue, result);
    }

    [Fact]
    public void And_OnSomeNone_ReturnsNone()
    {
        var result = Some.And(None);

        AssertOption.None(result);
    }

    [Fact]
    public void And_OnNoneSome_ReturnsNone()
    {
        var result = None.And(Some);

        AssertOption.None(result);
    }

    [Fact]
    public void And_OnNoneNone_ReturnsNone()
    {
        var result = None.And(None);

        AssertOption.None(result);
    }

    [Fact]
    public void AndThen_OnSomeSome_ReturnsSomeWithRightValue()
    {
        var result = Some.AndThen(_ => OtherSome);

        AssertOption.Equal(OtherSome, result);
    }

    [Fact]
    public void AndThen_OnSomeNone_ReturnsNone()
    {
        var result = Some.AndThen(_ => None);

        AssertOption.None(result);
    }

    [Fact]
    public void AndThen_OnNoneSome_ReturnsNone()
    {
        var result = None.AndThen(_ => Some);

        AssertOption.None(result);
    }

    [Fact]
    public void AndThen_OnNoneNone_ReturnsNone()
    {
        var result = None.AndThen(_ => None);

        AssertOption.None(result);
    }

    [Fact]
    public void AndThen_OnNone_DoesNotCallOptionProvider()
    {
        var optionProvider = Substitute.For<Func<object, Option<object>>>();
        optionProvider.Invoke(Arg.Any<object>()).Returns(Some);

        None.AndThen(optionProvider);

        Assert.Empty(optionProvider.ReceivedCalls());
    }

    [Fact]
    public void Or_OnSomeSome_ReturnsSomeWithLeftValue()
    {
        var result = Some.Or(OtherSome);

        AssertOption.Equal(Some, result);
    }

    [Fact]
    public void Or_OnSomeNone_ReturnsSomeWithLeftValue()
    {
        var result = Some.Or(None);

        AssertOption.Equal(Some, result);
    }

    [Fact]
    public void Or_OnNoneSome_ReturnsSomeWithRightValue()
    {
        var result = None.Or(OtherSome);

        AssertOption.Equal(OtherSome, result);
    }

    [Fact]
    public void Or_OnNoneNone_ReturnsNone()
    {
        var result = None.Or(None);

        AssertOption.None(result);
    }

    [Fact]
    public void OrElse_OnSomeSome_ReturnsSomeWithLeftValue()
    {
        var result = Some.OrElse(() => OtherSome);

        AssertOption.Equal(Some, result);
    }

    [Fact]
    public void OrElse_OnSomeNone_ReturnsSomeWithLeftValue()
    {
        var result = Some.OrElse(() => None);

        AssertOption.Equal(Some, result);
    }

    [Fact]
    public void OrElse_OnNoneSome_ReturnsSomeWithRightValue()
    {
        var result = None.OrElse(() => Some);

        AssertOption.Equal(Some, result);
    }

    [Fact]
    public void OrElse_OnNoneNone_ReturnsNone()
    {
        var result = None.OrElse(() => None);

        AssertOption.None(result);
    }

    [Fact]
    public void OrElse_OnSome_DoesNotCallOptionProvider()
    {
        var optionProvider = Substitute.For<Func<Option<object>>>();
        optionProvider.Invoke().Returns(OtherSome);

        Some.OrElse(optionProvider);

        Assert.Empty(optionProvider.ReceivedCalls());
    }

    [Fact]
    public void Xor_OnSomeSome_ReturnsNone()
    {
        var result = Some.Xor(OtherSome);

        AssertOption.None(result);
    }

    [Fact]
    public void Xor_OnSomeNone_ReturnsSomeWithLefValue()
    {
        var result = Some.Xor(None);

        AssertOption.Equal(Some, result);
    }

    [Fact]
    public void Xor_OnNoneSome_ReturnsSomeWithRightValue()
    {
        var result = None.Xor(Some);

        AssertOption.Equal(Some, result);
    }

    [Fact]
    public void Xor_OnNoneNone_ReturnsNone()
    {
        var result = None.Xor(None);

        AssertOption.None(result);
    }

    [Fact]
    public void Zip_OnSomeSome_ReturnsSomeWithTuple()
    {
        var result = Some.Zip(OtherSome);

        AssertOption.Some((Value, OtherValue), result);
    }

    [Fact]
    public void Zip_OnSomeNone_ReturnsNone()
    {
        var result = Some.Zip(None);

        AssertOption.None(result);
    }

    [Fact]
    public void Zip_OnNoneSome_ReturnsNone()
    {
        var result = None.Zip(Some);

        AssertOption.None(result);
    }

    [Fact]
    public void Zip_OnNoneNone_ReturnsNone()
    {
        var result = None.Zip(None);

        AssertOption.None(result);
    }

    [Fact]
    public void ZipWith_OnSomeSome_ReturnsSomeWithResultOfSelector()
    {
        var result = Some.ZipWith(OtherSome, (l, r) => (l, r));

        AssertOption.Some((Value, OtherValue), result);
    }

    [Fact]
    public void ZipWith_OnSomeNone_ReturnsNone()
    {
        var result = Some.ZipWith(None, (l, r) => (l, r));

        AssertOption.None(result);
    }

    [Fact]
    public void ZipWith_OnNoneSome_ReturnsNone()
    {
        var result = None.ZipWith(Some, (l, r) => (l, r));

        AssertOption.None(result);
    }

    [Fact]
    public void ZipWith_OnNoneNone_ReturnsNone()
    {
        var result = None.ZipWith(None, (l, r) => (l, r));

        AssertOption.None(result);
    }

    [Fact]
    public void Unzip_OnSome_ReturnsTupleWithSome()
    {
        var (left, right) = Option.Some((Value, OtherValue)).Unzip();

        AssertOption.Some(Value, left);
        AssertOption.Some(OtherValue, right);
    }

    [Fact]
    public void Unzip_OnNone_ReturnsTupleWithNone()
    {
        var (left, right) = Option.None<(object, object)>().Unzip();

        AssertOption.None(left);
        AssertOption.None(right);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(0, 0)]
    public void CompareTo_OnSomeSome_ReturnsValueComparison(int leftValue, int rightValue)
    {
        var result = Option.Some(leftValue).CompareTo(rightValue);

        Assert.Equal(leftValue.CompareTo(rightValue), result);
    }

    [Fact]
    public void CompareTo_OnSomeNone_Returns1()
    {
        var result = Option.Some(0).CompareTo(Option.None<int>());

        Assert.Equal(1, result);
    }

    [Fact]
    public void CompareTo_OnNoneSome_ReturnsMinus1()
    {
        var result = Option.None<int>().CompareTo(Option.Some(0));

        Assert.Equal(-1, result);
    }

    [Fact]
    public void CompareTo_OnNoneNone_Returns0()
    {
        var result = Option.None<int>().CompareTo(Option.None<int>());

        Assert.Equal(0, result);
    }

    [Fact]
    public void CollectArray_OnAllSome_ReturnsSomeWithList()
    {
        var result = new[]
            {
                Some,
                OtherSome
            }
            .Collect();

        AssertOption.Some(new[] { Value, OtherValue }, result);
    }

    [Fact]
    public void CollectArray_OnMixed_ReturnsNone()
    {
        var result = new[]
            {
                Some,
                None
            }
            .Collect();

        AssertOption.None(result);
    }

    [Fact]
    public void CollectArray_OnAllNone_ReturnsNone()
    {
        var result = new[]
            {
                None,
                None
            }
            .Collect();

        AssertOption.None(result);
    }

    [Fact]
    public void CollectArray_OnEmpty_ReturnsSomeWithEmptyList()
    {
        var result = Array.Empty<Option<object>>()
            .Collect();

        AssertOption.Some(Array.Empty<object>(), result);
    }

    [Fact]
    public void CollectList_OnAllSome_ReturnsSomeWithList()
    {
        var result = new List<Option<object>>
            {
                Some,
                OtherSome
            }
            .Collect();

        AssertOption.Some(new List<object> { Value, OtherValue }, result);
    }

    [Fact]
    public void CollectList_OnMixed_ReturnsNone()
    {
        var result = new List<Option<object>>
            {
                Some,
                None
            }
            .Collect();

        AssertOption.None(result);
    }

    [Fact]
    public void CollectList_OnAllNone_ReturnsNone()
    {
        var result = new List<Option<object>>
            {
                None,
                None,
            }
            .Collect();

        AssertOption.None(result);
    }

    [Fact]
    public void CollectList_OnEmpty_ReturnsSomeWithEmptyList()
    {
        var result = new List<Option<object>>().Collect();

        AssertOption.Some(new List<object>(), result);
    }

    [Fact]
    public void CollectEnumerable_OnAllSome_ReturnsSomeWithList()
    {
        var result = new[]
            {
                Some,
                OtherSome
            }
            .AsEnumerable()
            .Collect();

        AssertOption.Some(new[] { Value, OtherValue }.AsEnumerable(), result);
    }

    [Fact]
    public void CollectEnumerable_OnMixed_ReturnsNone()
    {
        var result = new[]
            {
                Some,
                None
            }
            .AsEnumerable()
            .Collect();

        AssertOption.None(result);
    }

    [Fact]
    public void CollectEnumerable_OnAllNone_ReturnsNone()
    {
        var result = new[]
            {
                None,
                None
            }
            .AsEnumerable()
            .Collect();

        AssertOption.None(result);
    }

    [Fact]
    public void CollectEnumerable_OnEmpty_ReturnsSomeWithEmptyList()
    {
        var result = Array.Empty<Option<object>>()
            .AsEnumerable()
            .Collect();

        AssertOption.Some(Array.Empty<object>().AsEnumerable(), result);
    }

    [Fact]
    public void Aggregate_OnAllSome_ReturnsSomeWithAggregate()
    {
        var result = new[]
            {
                Option.Some(0),
                Option.Some(1),
                Option.Some(2)
            }
            .Aggregate((a, b) => a + b);

        AssertOption.Some(3, result);
    }

    [Fact]
    public void Aggregate_OnMixed_ReturnsNone()
    {
        var result = new[]
            {
                Option.Some(0),
                Option.None<int>()
            }
            .Aggregate((a, b) => a + b);

        AssertOption.None(result);
    }

    [Fact]
    public void Aggregate_OnNoneSome_ReturnsNone()
    {
        var result = new[]
            {
                Option.None<int>(),
                Option.Some(0)
            }
            .Aggregate((a, b) => a + b);

        AssertOption.None(result);
    }

    [Fact]
    public void Aggregate_OnAllNone_ReturnsNone()
    {
        var result = new[]
            {
                Option.None<int>(),
                Option.None<int>()
            }
            .Aggregate((a, b) => a + b);

        AssertOption.None(result);
    }

    [Fact]
    public void Aggregate_OnEmpty_ThrowsInvalidOperationException()
    {
        var result = Assert.Throws<InvalidOperationException>(() => Array
            .Empty<Option<int>>()
            .Aggregate((a, b) => a + b));

        Assert.Equal("Sequence contains no elements", result.Message);
    }

    [Fact]
    public void AggregateWithDefault_OnAllSome_ReturnsSomeWithAggregate()
    {
        var result = new[]
            {
                Option.Some(0),
                Option.Some(1),
                Option.Some(2)
            }
            .Aggregate(1, (a, b) => a + b);

        AssertOption.Some(4, result);
    }

    [Fact]
    public void AggregateWithDefault_OnMixed_ReturnsNone()
    {
        var result = new[]
            {
                Option.Some(0),
                Option.None<int>()
            }
            .Aggregate(1, (a, b) => a + b);

        AssertOption.None(result);
    }

    [Fact]
    public void AggregateWithDefault_OnNoneSome_ReturnsNone()
    {
        var result = new[]
            {
                Option.None<int>(),
                Option.Some(0)
            }
            .Aggregate(1, (a, b) => a + b);

        AssertOption.None(result);
    }

    [Fact]
    public void AggregateWithDefault_OnAllNone_ReturnsNone()
    {
        var result = new[]
            {
                Option.None<int>(),
                Option.None<int>()
            }
            .Aggregate(1, (a, b) => a + b);

        AssertOption.None(result);
    }

    [Fact]
    public void AggregateWithDefault_OnEmpty_ReturnsSomeWithZero()
    {
        var result = Array.Empty<Option<int>>()
            .Aggregate(1, (a, b) => a + b);

        AssertOption.Some(1, result);
    }

    [Fact]
    public void Sum_OnAllSome_ReturnsSomeWithSum()
    {
        var result = new[]
            {
                Option.Some(0),
                Option.Some(1),
                Option.Some(2)
            }
            .Sum();

        AssertOption.Some(3, result);
    }

    [Fact]
    public void Sum_OnMixed_ReturnsNone()
    {
        var result = new[]
            {
                Option.Some(0),
                Option.None<int>()
            }
            .Sum();

        AssertOption.None(result);
    }

    [Fact]
    public void Sum_OnNoneSome_ReturnsNone()
    {
        var result = new[]
            {
                Option.None<int>(),
                Option.Some(0)
            }
            .Sum();

        AssertOption.None(result);
    }

    [Fact]
    public void Sum_OnAllNone_ReturnsNone()
    {
        var result = new[]
            {
                Option.None<int>(),
                Option.None<int>()
            }
            .Sum();

        AssertOption.None(result);
    }

    [Fact]
    public void Sum_OnEmpty_ReturnsSomeWithZero()
    {
        var result = Array.Empty<Option<int>>()
            .Sum();

        AssertOption.Some(0, result);
    }

    [Fact]
    public void Product_OnAllSome_ReturnsSomeWithProduct()
    {
        var result = new[]
            {
                Option.Some(1),
                Option.Some(2),
                Option.Some(3)
            }
            .Product();

        AssertOption.Some(6, result);
    }

    [Fact]
    public void Product_OnMixed_ReturnsNone()
    {
        var result = new[]
            {
                Option.Some(0),
                Option.None<int>()
            }
            .Product();

        AssertOption.None(result);
    }

    [Fact]
    public void Product_OnNoneSome_ReturnsNone()
    {
        var result = new[]
            {
                Option.None<int>(),
                Option.Some(0)
            }
            .Product();

        AssertOption.None(result);
    }

    [Fact]
    public void Product_OnAllNone_ReturnsNone()
    {
        var result = new[]
            {
                Option.None<int>(),
                Option.None<int>()
            }
            .Product();

        AssertOption.None(result);
    }

    [Fact]
    public void Product_OnEmpty_ReturnsSomeWithOne()
    {
        var result = Array.Empty<Option<int>>()
            .Product();

        AssertOption.Some(1, result);
    }
}