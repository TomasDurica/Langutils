using System.Collections;
using Langutils.Core.Options;
using Langutils.Core.Tests.Asserts;
using NSubstitute;

namespace Langutils.Core.Tests.Options;

public class TaskOptionAsyncExtensionsTests
{
    private static readonly object Value = new();
    private static readonly object OtherValue = new();
    private static readonly Task<object> TaskValue = Task.FromResult(Value);
    private static readonly Task<object> TaskOtherValue = Task.FromResult(OtherValue);
    private static readonly Option<object> Some = Option.Some(Value);
    private static readonly Option<object> OtherSome = Option.Some(OtherValue);
    private static readonly Option<object> None = Option.None<object>();
    private static readonly Task<Option<object>> TaskSome = Task.FromResult(Option.Some(Value));
    private static readonly Task<Option<object>> TaskOtherSome = Task.FromResult(Option.Some(OtherValue));
    private static readonly Task<Option<object>> TaskNone = Task.FromResult(Option.None<object>());

    [Fact]
    public async Task IsSomeAndAsync_OnSome_WhenMatchesPredicate_ReturnsTrue()
    {
        var result = await TaskSome.IsSomeAndAsync(v => Task.FromResult(v == Value));

        Assert.True(result);
    }

    [Fact]
    public async Task IsSomeAndAsync_OnSome_WhenDoesNotMatchPredicate_ReturnsFalse()
    {
        var result = await TaskSome.IsSomeAndAsync(v => Task.FromResult(v != Value));

        Assert.False(result);
    }

    [Fact]
    public async Task IsSomeAndAsync_OnNone_ReturnsFalse()
    {
        var result = await TaskNone.IsSomeAndAsync(_ => Task.FromResult(true));

        Assert.False(result);
    }

    [Fact]
    public async Task UnwrapOrElseAsync_OnSome_ReturnsValue()
    {
        var result = await TaskSome.UnwrapOrElseAsync(() => TaskOtherValue);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task UnwrapOrElseAsync_OnSome_DoesNotCallDefaultValueProvider()
    {
        var defaultValueProvider = Substitute.For<Func<Task<object>>>();
        defaultValueProvider.Invoke().Returns(TaskOtherValue);

        await TaskSome.UnwrapOrElseAsync(defaultValueProvider);

        Assert.Empty(defaultValueProvider.ReceivedCalls());
    }

    [Fact]
    public async Task UnwrapOrElseAsync_OnNone_ReturnsValueFromDefaultValueProvider()
    {
        var result = await TaskNone.UnwrapOrElseAsync(() => TaskValue);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task TapAsync_OnSome_ShouldCallOnSome()
    {
        var onSome = Substitute.For<Func<object, Task>>();

        await TaskSome.TapAsync(onSome);

        Assert.Single((IEnumerable)onSome.ReceivedCalls());
        Assert.StrictEqual(Value, onSome.ReceivedCalls().First().GetArguments().First());
    }

    [Fact]
    public async Task TapAsync_OnNone_ShouldNotCallOnSome()
    {
        var onSome = Substitute.For<Func<object, Task>>();

        await TaskNone.TapAsync(onSome);

        Assert.Empty(onSome.ReceivedCalls());
    }

    [Fact]
    public async Task FilterAsync_OnSome_WhenMatchesPredicate_ReturnsSome()
    {
        var result = await TaskSome.FilterAsync(v => Task.FromResult(v == Value));

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task FilterAsync_OnSome_WhenDoesNotMatchPredicate_ReturnsNone()
    {
        var result = await TaskSome.FilterAsync(v => Task.FromResult(v != Value));

        AssertOption.None(result);
    }

    [Fact]
    public async Task FilterAsync_OnNone_ReturnsNone()
    {
        var result = await TaskNone.FilterAsync(_ => Task.FromResult(true));

        AssertOption.None(result);
    }

    [Fact]
    public async Task MapAsync_OnSome_ReturnsSome()
    {
        var result = await TaskSome.MapAsync(Task.FromResult);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task MapAsync_OnNone_ReturnsNone()
    {
        var result = await TaskNone.MapAsync(Task.FromResult);

        AssertOption.None(result);
    }

    [Fact]
    public async Task MapOrAsync_OnSome_ReturnsSome()
    {
        var result = await TaskSome.MapOrAsync(OtherValue, Task.FromResult);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task MapOrAsync_OnNone_ReturnsDefaultValue()
    {
        var result = await TaskNone.MapOrAsync(OtherValue, Task.FromResult);

        AssertOption.Some(OtherValue, result);
    }

    [Fact]
    public async Task MapOrElseAsync_OnSome_ReturnsSome()
    {
        var result = await TaskSome.MapOrElseAsync(() => OtherValue, Task.FromResult);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task MapOrElseAsync_OnSome_DoesNotCallDefaultValueProvider()
    {
        var defaultValueProvider = Substitute.For<Func<object>>();
        defaultValueProvider.Invoke().Returns(OtherValue);

        await TaskSome.MapOrElseAsync(defaultValueProvider, Task.FromResult);

        Assert.Empty(defaultValueProvider.ReceivedCalls());
    }

    [Fact]
    public async Task MapOrElseAsync_OnNone_ReturnsDefaultValue()
    {
        var result = await TaskNone.MapOrElseAsync(() => Value, Task.FromResult);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task MapOrElseAsync2_OnSome_ReturnsSome()
    {
        var result = await TaskSome.MapOrElseAsync(() => TaskOtherValue, value => value);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task MapOrElseAsync2_OnSome_DoesNotCallDefaultValueProvider()
    {
        var defaultValueProvider = Substitute.For<Func<Task<object>>>();
        defaultValueProvider.Invoke().Returns(TaskOtherValue);

        await TaskSome.MapOrElseAsync(defaultValueProvider, value => value);

        Assert.Empty(defaultValueProvider.ReceivedCalls());
    }

    [Fact]
    public async Task MapOrElseAsync2_OnNone_ReturnsDefaultValue()
    {
        var result = await TaskNone.MapOrElseAsync(() => TaskValue, value => value);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task MapOrElseAsync3_OnSome_ReturnsSome()
    {
        var result = await TaskSome.MapOrElseAsync(() => TaskOtherValue, Task.FromResult);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task MapOrElseAsync3_OnSome_DoesNotCallDefaultValueProvider()
    {
        var defaultValueProvider = Substitute.For<Func<Task<object>>>();
        defaultValueProvider.Invoke().Returns(TaskOtherValue);

        await TaskSome.MapOrElseAsync(defaultValueProvider, Task.FromResult);

        Assert.Empty(defaultValueProvider.ReceivedCalls());
    }

    [Fact]
    public async Task MapOrElseAsync3_OnNone_ReturnsDefaultValue()
    {
        var result = await TaskNone.MapOrElseAsync(() => TaskValue, Task.FromResult);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task SomeOrElseAsync_OnSome_ReturnsSome()
    {
        var result = await TaskSome.SomeOrElseAsync(() => TaskOtherValue);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task SomeOrElseAsync_OnSome_DoesNotCallDefaultValueProvider()
    {
        var defaultValueProvider = Substitute.For<Func<Task<object>>>();
        defaultValueProvider.Invoke().Returns(OtherValue);

        await TaskSome.SomeOrElseAsync(defaultValueProvider);

        Assert.Empty(defaultValueProvider.ReceivedCalls());
    }

    [Fact]
    public async Task SomeOrElseAsync_OnNone_ReturnsError()
    {
        var result = await TaskNone.SomeOrElseAsync(() => TaskOtherValue);

        AssertResult.Error(OtherValue, result);
    }

    [Fact]
    public async Task AndThenAsync_OnSomeSome_ReturnsSomeWithRightValue()
    {
        var result = await TaskSome.AndThenAsync(_ => TaskOtherSome);

        AssertOption.Equal(OtherSome, result);
    }

    [Fact]
    public async Task AndThenAsync_OnSomeNone_ReturnsNone()
    {
        var result = await TaskSome.AndThenAsync(_ => TaskNone);

        AssertOption.None(result);
    }

    [Fact]
    public async Task AndThenAsync_OnNoneSome_ReturnsNone()
    {
        var result = await TaskNone.AndThenAsync(_ => TaskSome);

        AssertOption.None(result);
    }

    [Fact]
    public async Task AndThenAsync_OnNoneNone_ReturnsNone()
    {
        var result = await TaskNone.AndThenAsync(_ => TaskNone);

        AssertOption.None(result);
    }

    [Fact]
    public async Task AndThenAsync_OnNone_DoesNotCallOptionProvider()
    {
        var optionProvider = Substitute.For<Func<object, Task<Option<object>>>>();
        optionProvider.Invoke(Arg.Any<object>()).Returns(Some);

        await TaskNone.AndThenAsync(optionProvider);

        Assert.Empty(optionProvider.ReceivedCalls());
    }

    [Fact]
    public async Task OrElseAsync_OnSomeSome_ReturnsSomeWithLeftValue()
    {
        var result = await TaskSome.OrElseAsync(() => TaskOtherSome);

        AssertOption.Equal(Some, result);
    }

    [Fact]
    public async Task OrElseAsync_OnSomeNone_ReturnsSomeWithLeftValue()
    {
        var result = await TaskSome.OrElseAsync(() => TaskNone);

        AssertOption.Equal(Some, result);
    }

    [Fact]
    public async Task OrElseAsync_OnNoneSome_ReturnsSomeWithRightValue()
    {
        var result = await TaskNone.OrElseAsync(() => TaskSome);

        AssertOption.Equal(Some, result);
    }

    [Fact]
    public async Task OrElseAsync_OnNoneNone_ReturnsNone()
    {
        var result = await TaskNone.OrElseAsync(() => TaskNone);

        AssertOption.None(result);
    }

    [Fact]
    public async Task OrElseAsync_OnSome_DoesNotCallOptionProvider()
    {
        var optionProvider = Substitute.For<Func<Task<Option<object>>>>();
        optionProvider.Invoke().Returns(OtherSome);

        await TaskSome.OrElseAsync(optionProvider);

        Assert.Empty(optionProvider.ReceivedCalls());
    }

    [Fact]
    public async Task ZipWithAsync_OnSomeSome_ReturnsSomeWithResultOfSelector()
    {
        var result = await TaskSome.ZipWithAsync(OtherSome, (l, r) => Task.FromResult((l, r)));

        AssertOption.Some((Value, OtherValue), result);
    }

    [Fact]
    public async Task ZipWithAsync_OnSomeNone_ReturnsNone()
    {
        var result = await TaskSome.ZipWithAsync(None, (l, r) => Task.FromResult((l, r)));

        AssertOption.None(result);
    }

    [Fact]
    public async Task ZipWithAsync_OnNoneSome_ReturnsNone()
    {
        var result = await TaskNone.ZipWithAsync(Some, (l, r) => Task.FromResult((l, r)));

        AssertOption.None(result);
    }

    [Fact]
    public async Task ZipWithAsync_OnNoneNone_ReturnsNone()
    {
        var result = await TaskNone.ZipWithAsync(None, (l, r) => Task.FromResult((l, r)));

        AssertOption.None(result);
    }
}