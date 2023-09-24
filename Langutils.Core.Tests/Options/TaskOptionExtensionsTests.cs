using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Langutils.Core.Options;
using Langutils.Core.Results;
using Langutils.Core.Tests.Asserts;
using NSubstitute;

namespace Langutils.Core.Tests.Options;

public class TaskOptionExtensionsTests
{
    private static readonly object Value = new();
    private static readonly object OtherValue = new();
    private static readonly Option<object> Some = Option.Some(Value);
    private static readonly Option<object> OtherSome = Option.Some(OtherValue);
    private static readonly Option<object> None = Option.None<object>();
    private static readonly Task<Option<object>> TaskSome = Task.FromResult(Option.Some(Value));
    private static readonly Task<Option<object>> TaskOtherSome = Task.FromResult(Option.Some(OtherValue));
    private static readonly Task<Option<object>> TaskNone = Task.FromResult(Option.None<object>());

    [Fact]
    public async Task IsSomeAnd_OnSome_WhenMatchesPredicate_ReturnsTrue()
    {
        var result = await TaskSome.IsSomeAnd(v => v == Value);

        Assert.True(result);
    }

    [Fact]
    public async Task IsSomeAnd_OnSome_WhenDoesNotMatchPredicate_ReturnsFalse()
    {
        var result = await TaskSome.IsSomeAnd(v => v != Value);

        Assert.False(result);
    }

    [Fact]
    public async Task IsSomeAnd_OnNone_ReturnsFalse()
    {
        var result = await TaskNone.IsSomeAnd(_ => true);

        Assert.False(result);
    }

    [Fact]
    public async Task Expect_OnSome_ReturnsValue()
    {
        var result = await TaskSome.Expect(string.Empty);

        Assert.Same(Value, result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Error")]
    public async Task Expect_OnNone_ThrowsInvalidOperationExceptionWithMessage(string message)
    {
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await TaskNone.Expect(message));

        Assert.Same(message, exception.Message);
    }

    [Fact]
    public async Task UnwrapOr_OnSome_ReturnsValue()
    {
        var result = await TaskSome.UnwrapOr(OtherValue);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task UnwrapOr_OnNone_ReturnsDefaultValue()
    {
        var result = await TaskNone.UnwrapOr(OtherValue);

        Assert.Same(OtherValue, result);
    }

    [Fact]
    public async Task UnwrapOrDefault_OnSome_ReturnsValue()
    {
        var result = await TaskSome.UnwrapOrDefault();

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task UnwrapOrDefault_OnNone_ReturnsDefaultValue()
    {
        var result = await TaskNone.UnwrapOrDefault();

        Assert.Same(default, result);
    }

    [Fact]
    public async Task UnwrapOrElse_OnSome_ReturnsValue()
    {
        var result = await TaskSome.UnwrapOrElse(() => OtherValue);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task UnwrapOrElse_OnSome_DoesNotCallDefaultValueProvider()
    {
        var defaultValueProvider = Substitute.For<Func<object>>();
        defaultValueProvider.Invoke().Returns(OtherValue);

        await TaskSome.UnwrapOrElse(defaultValueProvider);

        Assert.Empty(defaultValueProvider.ReceivedCalls());
    }

    [Fact]
    public async Task UnwrapOrElse_OnNone_ReturnsValueFromDefaultValueProvider()
    {
        var result = await TaskNone.UnwrapOrElse(() => Value);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task Tap_OnSome_ShouldCallOnSome()
    {
        var onSome = Substitute.For<Action<object>>();

        await TaskSome.Tap(onSome);

        Assert.Single((IEnumerable)onSome.ReceivedCalls());
        Assert.StrictEqual(Value, onSome.ReceivedCalls().First().GetArguments().First());
    }

    [Fact]
    public async Task Tap_OnNone_ShouldNotCallOnSome()
    {
        var onSome = Substitute.For<Action<object>>();

        await TaskNone.Tap(onSome);

        Assert.Empty(onSome.ReceivedCalls());
    }

    [Fact]
    public async Task Filter_OnSome_WhenMatchesPredicate_ReturnsSome()
    {
        var result = await TaskSome.Filter(v => v == Value);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task Filter_OnSome_WhenDoesNotMatchPredicate_ReturnsNone()
    {
        var result = await TaskSome.Filter(v => v != Value);

        AssertOption.None(result);
    }

    [Fact]
    public async Task Filter_OnNone_ReturnsNone()
    {
        var result = await TaskNone.Filter(_ => true);

        AssertOption.None(result);
    }

    [Fact]
    public async Task Flatten_OnSomeSome_ReturnsSome()
    {
        var result = await Task.FromResult(Option.Some(Some).Flatten());

        AssertOption.Equal(Some, result);
    }

    [Fact]
    public async Task Flatten_OnSomeNone_ReturnsNone()
    {
        var result = await Task.FromResult(Option.Some(None).Flatten());

        AssertOption.None(result);
    }

    [Fact]
    public async Task Flatten_OnNone_ReturnsNone()
    {
        var result = await Task.FromResult(Option.None<Option<object>>().Flatten());

        AssertOption.None(result);
    }

    [Fact]
    public async Task Map_OnSome_ReturnsSome()
    {
        var result = await TaskSome.Map(v => v);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task Map_OnNone_ReturnsNone()
    {
        var result = await TaskNone.Map(v => v);

        AssertOption.None(result);
    }

    [Fact]
    public async Task MapOr_OnSome_ReturnsSome()
    {
        var result = await TaskSome.MapOr(OtherValue, v => v);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task MapOr_OnNone_ReturnsDefaultValue()
    {
        var result = await TaskNone.MapOr(OtherValue, v => v);

        AssertOption.Some(OtherValue, result);
    }

    [Fact]
    public async Task MapOrElse_OnSome_ReturnsSome()
    {
        var result = await TaskSome.MapOrElse(() => OtherValue, v => v);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task MapOrElse_OnSome_DoesNotCallDefaultValueProvider()
    {
        var defaultValueProvider = Substitute.For<Func<object>>();
        defaultValueProvider.Invoke().Returns(OtherValue);

        await TaskSome.MapOrElse(defaultValueProvider, v => v);

        Assert.Empty(defaultValueProvider.ReceivedCalls());
    }

    [Fact]
    public async Task MapOrElse_OnNone_ReturnsDefaultValue()
    {
        var result = await TaskNone.MapOrElse(() => Value, v => v);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task SomeOr_OnSome_ReturnsSome()
    {
        var result = await TaskSome.SomeOr(OtherValue);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task SomeOr_OnNone_ReturnsError()
    {
        var result = await TaskNone.SomeOr(OtherValue);

        AssertResult.Error(OtherValue, result);
    }

    [Fact]
    public async Task SomeOrElse_OnSome_ReturnsSome()
    {
        var result = await TaskSome.SomeOrElse(() => OtherValue);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task SomeOrElse_OnSome_DoesNotCallDefaultValueProvider()
    {
        var defaultValueProvider = Substitute.For<Func<object>>();
        defaultValueProvider.Invoke().Returns(OtherValue);

        await TaskSome.SomeOrElse(defaultValueProvider);

        Assert.Empty(defaultValueProvider.ReceivedCalls());
    }

    [Fact]
    public async Task SomeOrElse_OnNone_ReturnsError()
    {
        var result = await TaskNone.SomeOrElse(() => OtherValue);

        AssertResult.Error(OtherValue, result);
    }

    [Fact]
    public async Task Transpose_OnSomeSuccess_ReturnsSuccessSome()
    {
        var result = await Task.FromResult(Option.Some(Result.Success(Value))).Transpose();

        AssertResult.Success(Some, result);
    }

    [Fact]
    public async Task Transpose_OnSomeError_ReturnsError()
    {
        var result = await Task.FromResult(Option.Some(Result.Error<object, object>(OtherValue))).Transpose();

        AssertResult.Error(OtherValue, result);
    }

    [Fact]
    public async Task Transpose_OnNone_ReturnsSuccessNone()
    {
        var result = await Task.FromResult(Option.None<Result<object, object>>()).Transpose();

        AssertResult.Success(None, result);
    }

    [Fact]
    public async Task AsEnumerable_OnSome_ReturnsEnumerableWithOneElement()
    {
        var result = await TaskSome.AsEnumerable();

        Assert.Equivalent(new [] { Value }, result);
    }

    [Fact]
    public async Task AsEnumerable_OnNone_ReturnsEmptyEnumerable()
    {
        var result = await TaskNone.AsEnumerable();

        Assert.Empty(result);
    }

    [Fact]
    public async Task ToList_OnSome_ReturnsListWithOneElement()
    {
        var result = await TaskSome.ToList();

        Assert.Equivalent(new [] { Value }, result);
    }

    [Fact]
    public async Task ToList_OnNone_ReturnsEmptyList()
    {
        var result = await TaskNone.ToList();

        Assert.Empty(result);
    }

    [Fact]
    public async Task ToArray_OnSome_ReturnsArrayWithOneElement()
    {
        var result = await TaskSome.ToArray();

        Assert.Equivalent(new [] { Value }, result);
    }

    [Fact]
    public async Task ToArray_OnNone_ReturnsEmptyArray()
    {
        var result = await TaskNone.ToArray();

        Assert.Empty(result);
    }

    [Fact]
    public async Task And_OnSomeSome_ReturnsSomeWithRightValue()
    {
        var result = await TaskSome.And(OtherSome);

        AssertOption.Some(OtherValue, result);
    }

    [Fact]
    public async Task And_OnSomeNone_ReturnsNone()
    {
        var result = await TaskSome.And(None);

        AssertOption.None(result);
    }

    [Fact]
    public async Task And_OnNoneSome_ReturnsNone()
    {
        var result = await TaskNone.And(Some);

        AssertOption.None(result);
    }

    [Fact]
    public async Task And_OnNoneNone_ReturnsNone()
    {
        var result = await TaskNone.And(None);

        AssertOption.None(result);
    }

    [Fact]
    public async Task AndThen_OnSomeSome_ReturnsSomeWithRightValue()
    {
        var result = await TaskSome.AndThen(_ => OtherSome);

        AssertOption.Equal(OtherSome, result);
    }

    [Fact]
    public async Task AndThen_OnSomeNone_ReturnsNone()
    {
        var result = await TaskSome.AndThen(_ => None);

        AssertOption.None(result);
    }

    [Fact]
    public async Task AndThen_OnNoneSome_ReturnsNone()
    {
        var result = await TaskNone.AndThen(_ => Some);

        AssertOption.None(result);
    }

    [Fact]
    public async Task AndThen_OnNoneNone_ReturnsNone()
    {
        var result = await TaskNone.AndThen(_ => None);

        AssertOption.None(result);
    }

    [Fact]
    public async Task AndThen_OnNone_DoesNotCallOptionProvider()
    {
        var optionProvider = Substitute.For<Func<object, Option<object>>>();
        optionProvider.Invoke(Arg.Any<object>()).Returns(TaskSome);

        await TaskNone.AndThen(optionProvider);

        Assert.Empty(optionProvider.ReceivedCalls());
    }

    [Fact]
    public async Task Or_OnSomeSome_ReturnsSomeWithLeftValue()
    {
        var result = await TaskSome.Or(OtherSome);

        AssertOption.Equal(Some, result);
    }

    [Fact]
    public async Task Or_OnSomeNone_ReturnsSomeWithLeftValue()
    {
        var result = await TaskSome.Or(None);

        AssertOption.Equal(Some, result);
    }

    [Fact]
    public async Task Or_OnNoneSome_ReturnsSomeWithRightValue()
    {
        var result = await TaskNone.Or(OtherSome);

        AssertOption.Equal(OtherSome, result);
    }

    [Fact]
    public async Task Or_OnNoneNone_ReturnsNone()
    {
        var result = await TaskNone.Or(None);

        AssertOption.None(result);
    }

    [Fact]
    public async Task OrElse_OnSomeSome_ReturnsSomeWithLeftValue()
    {
        var result = await TaskSome.OrElse(() => OtherSome);

        AssertOption.Equal(Some, result);
    }

    [Fact]
    public async Task OrElse_OnSomeNone_ReturnsSomeWithLeftValue()
    {
        var result = await TaskSome.OrElse(() => None);

        AssertOption.Equal(Some, result);
    }

    [Fact]
    public async Task OrElse_OnNoneSome_ReturnsSomeWithRightValue()
    {
        var result = await TaskNone.OrElse(() => Some);

        AssertOption.Equal(Some, result);
    }

    [Fact]
    public async Task OrElse_OnNoneNone_ReturnsNone()
    {
        var result = await TaskNone.OrElse(() => None);

        AssertOption.None(result);
    }

    [Fact]
    public async Task OrElse_OnSome_DoesNotCallOptionProvider()
    {
        var optionProvider = Substitute.For<Func<Option<object>>>();
        optionProvider.Invoke().Returns(TaskOtherSome);

        await TaskSome.OrElse(optionProvider);

        Assert.Empty(optionProvider.ReceivedCalls());
    }
    [Fact]
    public async Task Xor_OnSomeSome_ReturnsNone()
    {
        var result = await TaskSome.Xor(Some);

        AssertOption.None(result);
    }

    [Fact]
    public async Task Xor_OnSomeNone_ReturnsSomeWithLefValue()
    {
        var result = await TaskSome.Xor(None);

        AssertOption.Equal(Some, result);
    }

    [Fact]
    public async Task Xor_OnNoneSome_ReturnsSomeWithRightValue()
    {
        var result = await TaskNone.Xor(Some);

        AssertOption.Equal(Some, result);
    }

    [Fact]
    public async Task Xor_OnNoneNone_ReturnsNone()
    {
        var result = await TaskNone.Xor(None);

        AssertOption.None(result);
    }

    [Fact]
    public async Task Zip_OnSomeSome_ReturnsSomeWithTuple()
    {
        var result = await TaskSome.Zip(OtherSome);

        AssertOption.Some((Value, OtherValue), result);
    }

    [Fact]
    public async Task Zip_OnSomeNone_ReturnsNone()
    {
        var result = await TaskSome.Zip(None);

        AssertOption.None(result);
    }

    [Fact]
    public async Task Zip_OnNoneSome_ReturnsNone()
    {
        var result = await TaskNone.Zip(Some);

        AssertOption.None(result);
    }

    [Fact]
    public async Task Zip_OnNoneNone_ReturnsNone()
    {
        var result = await TaskNone.Zip(None);

        AssertOption.None(result);
    }

    [Fact]
    public async Task ZipWith_OnSomeSome_ReturnsSomeWithResultOfSelector()
    {
        var result = await TaskSome.ZipWith(OtherSome, (l, r) => (l, r));

        AssertOption.Some((Value, OtherValue), result);
    }

    [Fact]
    public async Task ZipWith_OnSomeNone_ReturnsNone()
    {
        var result = await TaskSome.ZipWith(None, (l, r) => (l, r));

        AssertOption.None(result);
    }

    [Fact]
    public async Task ZipWith_OnNoneSome_ReturnsNone()
    {
        var result = await TaskNone.ZipWith(Some, (l, r) => (l, r));

        AssertOption.None(result);
    }

    [Fact]
    public async Task ZipWith_OnNoneNone_ReturnsNone()
    {
        var result = await TaskNone.ZipWith(None, (l, r) => (l, r));

        AssertOption.None(result);
    }

    [Fact]
    public async Task Unzip_OnSome_ReturnsTupleWithSome()
    {
        var (left, right) = await Task.FromResult(Option.Some((Value, OtherValue)))
            .Unzip();

        AssertOption.Some(Value, left);
        AssertOption.Some(OtherValue, right);
    }

    [Fact]
    public async Task Unzip_OnNone_ReturnsTupleWithNone()
    {
        var (left, right) = await Task.FromResult(Option.None<(object, object)>())
            .Unzip();

        AssertOption.None(left);
        AssertOption.None(right);
    }

    [Fact]
    public async Task CollectArray_OnAllSome_ReturnsSomeWithList()
    {
        var result = await Task.FromResult(new[]
            {
                Some,
                OtherSome
            })
            .Collect();

        AssertOption.Some(new[] { Value, OtherValue }, result);
    }

    [Fact]
    public async Task CollectArray_OnMixed_ReturnsNone()
    {
        var result = await Task.FromResult(new[]
            {
                Some,
                None
            })
            .Collect();

        AssertOption.None(result);
    }

    [Fact]
    public async Task CollectArray_OnAllNone_ReturnsNone()
    {
        var result = await Task.FromResult(new[]
            {
                None,
                None
            })
            .Collect();

        AssertOption.None(result);
    }

    [Fact]
    public async Task CollectArray_OnEmpty_ReturnsSomeWithEmptyList()
    {
        var result = await Task.FromResult(Array.Empty<Option<object>>())
            .Collect();

        AssertOption.Some(Array.Empty<object>(), result);
    }

    [Fact]
    public async Task CollectList_OnAllSome_ReturnsSomeWithList()
    {
        var result = await Task.FromResult(new List<Option<object>>
            {
                Some,
                OtherSome
            })
            .Collect();

        AssertOption.Some(new List<object> { Value, OtherValue }, result);
    }

    [Fact]
    public async Task CollectList_OnMixed_ReturnsNone()
    {
        var result = await Task.FromResult(new List<Option<object>>
            {
                Some,
                None
            })
            .Collect();

        AssertOption.None(result);
    }

    [Fact]
    public async Task CollectList_OnAllNone_ReturnsNone()
    {
        var result = await Task.FromResult(new List<Option<object>>
            {
                None,
                None,
            })
            .Collect();

        AssertOption.None(result);
    }

    [Fact]
    public async Task CollectList_OnEmpty_ReturnsSomeWithEmptyList()
    {
        var result = await Task.FromResult(new List<Option<object>>())
            .Collect();

        AssertOption.Some(new List<object>(), result);
    }

    [Fact]
    public async Task CollectEnumerable_OnAllSome_ReturnsSomeWithList()
    {
        var result = await Task.FromResult(new[]
            {
                Some,
                OtherSome
            }.AsEnumerable())
            .Collect();

        AssertOption.Some(new[] { Value, OtherValue }.AsEnumerable(), result);
    }

    [Fact]
    public async Task CollectEnumerable_OnMixed_ReturnsNone()
    {
        var result = await Task.FromResult(new[]
            {
                Some,
                None
            }.AsEnumerable())
            .Collect();

        AssertOption.None(result);
    }

    [Fact]
    public async Task CollectEnumerable_OnAllNone_ReturnsNone()
    {
        var result = await Task.FromResult(new[]
            {
                None,
                None
            }.AsEnumerable())
            .Collect();

        AssertOption.None(result);
    }

    [Fact]
    public async Task CollectEnumerable_OnEmpty_ReturnsSomeWithEmptyList()
    {
        var result = await Task.FromResult(Array.Empty<Option<object>>().AsEnumerable())
            .Collect();

        AssertOption.Some(Array.Empty<object>().AsEnumerable(), result);
    }
}