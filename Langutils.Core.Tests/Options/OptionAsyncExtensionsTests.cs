using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Langutils.Core.Options;
using Langutils.Core.Tests.Asserts;
using NSubstitute;

namespace Langutils.Core.Tests.Options;

public class OptionAsyncExtensionsTests
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
        var result = await Some.IsSomeAndAsync(v => Task.FromResult(v == Value));

        Assert.True(result);
    }

    [Fact]
    public async Task IsSomeAndAsync_OnSome_WhenDoesNotMatchPredicate_ReturnsFalse()
    {
        var result = await Some.IsSomeAndAsync(v => Task.FromResult(v != Value));

        Assert.False(result);
    }

    [Fact]
    public async Task IsSomeAndAsync_OnNone_ReturnsFalse()
    {
        var result = await None.IsSomeAndAsync(_ => Task.FromResult(true));

        Assert.False(result);
    }

    [Fact]
    public async Task UnwrapOrElseAsync_OnSome_ReturnsValue()
    {
        var result = await Some.UnwrapOrElseAsync(() => TaskOtherValue);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task UnwrapOrElseAsync_OnSome_DoesNotCallDefaultValueProvider()
    {
        var defaultValueProvider = Substitute.For<Func<Task<object>>>();
        defaultValueProvider.Invoke().Returns(TaskOtherValue);

        await Some.UnwrapOrElseAsync(defaultValueProvider);

        Assert.Empty(defaultValueProvider.ReceivedCalls());
    }

    [Fact]
    public async Task UnwrapOrElseAsync_OnNone_ReturnsValueFromDefaultValueProvider()
    {
        var result = await None.UnwrapOrElseAsync(() => TaskValue);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task TapAsync_OnSome_ShouldCallOnSome()
    {
        var onSome = Substitute.For<Func<object, Task>>();

        await Some.TapAsync(onSome);

        Assert.Single((IEnumerable)onSome.ReceivedCalls());
        Assert.StrictEqual(Value, onSome.ReceivedCalls().First().GetArguments().First());
    }

    [Fact]
    public async Task TapAsync_OnNone_ShouldNotCallOnSome()
    {
        var onSome = Substitute.For<Func<object, Task>>();

        await None.TapAsync(onSome);

        Assert.Empty(onSome.ReceivedCalls());
    }

    [Fact]
    public async Task FilterAsync_OnSome_WhenMatchesPredicate_ReturnsSome()
    {
        var result = await Some.FilterAsync(v => Task.FromResult(v == Value));

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task FilterAsync_OnSome_WhenDoesNotMatchPredicate_ReturnsNone()
    {
        var result = await Some.FilterAsync(v => Task.FromResult(v != Value));

        AssertOption.None(result);
    }

    [Fact]
    public async Task FilterAsync_OnNone_ReturnsNone()
    {
        var result = await None.FilterAsync(_ => Task.FromResult(true));

        AssertOption.None(result);
    }

    [Fact]
    public async Task MapAsync_OnSome_ReturnsSome()
    {
        var result = await Some.MapAsync(Task.FromResult);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task MapAsync_OnNone_ReturnsNone()
    {
        var result = await None.MapAsync(Task.FromResult);

        AssertOption.None(result);
    }

    [Fact]
    public async Task MapOrAsync_OnSome_ReturnsSome()
    {
        var result = await Some.MapOrAsync(OtherValue, Task.FromResult);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task MapOrAsync_OnNone_ReturnsDefaultValue()
    {
        var result = await None.MapOrAsync(OtherValue, Task.FromResult);

        AssertOption.Some(OtherValue, result);
    }

    [Fact]
    public async Task MapOrElseAsync_OnSome_ReturnsSome()
    {
        var result = await Some.MapOrElseAsync(() => OtherValue, Task.FromResult);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task MapOrElseAsync_OnSome_DoesNotCallDefaultValueProvider()
    {
        var defaultValueProvider = Substitute.For<Func<object>>();
        defaultValueProvider.Invoke().Returns(OtherValue);

        await Some.MapOrElseAsync(defaultValueProvider, Task.FromResult);

        Assert.Empty(defaultValueProvider.ReceivedCalls());
    }

    [Fact]
    public async Task MapOrElseAsync_OnNone_ReturnsDefaultValue()
    {
        var result = await None.MapOrElseAsync(() => Value, Task.FromResult);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task SomeOrElseAsync_OnSome_ReturnsSome()
    {
        var result = await Some.SomeOrElseAsync(() => TaskOtherValue);

        AssertResult.Success(Value, result);
    }

    [Fact]
    public async Task SomeOrElseAsync_OnSome_DoesNotCallDefaultValueProvider()
    {
        var defaultValueProvider = Substitute.For<Func<Task<object>>>();
        defaultValueProvider.Invoke().Returns(OtherValue);

        await Some.SomeOrElseAsync(defaultValueProvider);

        Assert.Empty(defaultValueProvider.ReceivedCalls());
    }

    [Fact]
    public async Task SomeOrElseAsync_OnNone_ReturnsError()
    {
        var result = await None.SomeOrElseAsync(() => TaskOtherValue);

        AssertResult.Error(OtherValue, result);
    }

    [Fact]
    public async Task AndThenAsync_OnSomeSome_ReturnsSomeWithRightValue()
    {
        var result = await Some.AndThenAsync(_ => TaskOtherSome);

        AssertOption.Equal(OtherSome, result);
    }

    [Fact]
    public async Task AndThenAsync_OnSomeNone_ReturnsNone()
    {
        var result = await Some.AndThenAsync(_ => TaskNone);

        AssertOption.None(result);
    }

    [Fact]
    public async Task AndThenAsync_OnNoneSome_ReturnsNone()
    {
        var result = await None.AndThenAsync(_ => TaskSome);

        AssertOption.None(result);
    }

    [Fact]
    public async Task AndThenAsync_OnNoneNone_ReturnsNone()
    {
        var result = await None.AndThenAsync(_ => TaskNone);

        AssertOption.None(result);
    }

    [Fact]
    public async Task AndThenAsync_OnNone_DoesNotCallOptionProvider()
    {
        var optionProvider = Substitute.For<Func<object, Task<Option<object>>>>();
        optionProvider.Invoke(Arg.Any<object>()).Returns(Some);

        await None.AndThenAsync(optionProvider);

        Assert.Empty(optionProvider.ReceivedCalls());
    }

    [Fact]
    public async Task OrElseAsync_OnSomeSome_ReturnsSomeWithLeftValue()
    {
        var result = await Some.OrElseAsync(() => TaskOtherSome);

        AssertOption.Equal(Some, result);
    }

    [Fact]
    public async Task OrElseAsync_OnSomeNone_ReturnsSomeWithLeftValue()
    {
        var result = await Some.OrElseAsync(() => TaskNone);

        AssertOption.Equal(Some, result);
    }

    [Fact]
    public async Task OrElseAsync_OnNoneSome_ReturnsSomeWithRightValue()
    {
        var result = await None.OrElseAsync(() => TaskSome);

        AssertOption.Equal(Some, result);
    }

    [Fact]
    public async Task OrElseAsync_OnNoneNone_ReturnsNone()
    {
        var result = await None.OrElseAsync(() => TaskNone);

        AssertOption.None(result);
    }

    [Fact]
    public async Task OrElseAsync_OnSome_DoesNotCallOptionProvider()
    {
        var optionProvider = Substitute.For<Func<Task<Option<object>>>>();
        optionProvider.Invoke().Returns(OtherSome);

        await Some.OrElseAsync(optionProvider);

        Assert.Empty(optionProvider.ReceivedCalls());
    }

    [Fact]
    public async Task ZipWithAsync_OnSomeSome_ReturnsSomeWithResultOfSelector()
    {
        var result = await Some.ZipWithAsync(OtherSome, (l, r) => Task.FromResult((l, r)));

        AssertOption.Some((Value, OtherValue), result);
    }

    [Fact]
    public async Task ZipWithAsync_OnSomeNone_ReturnsNone()
    {
        var result = await Some.ZipWithAsync(None, (l, r) => Task.FromResult((l, r)));

        AssertOption.None(result);
    }

    [Fact]
    public async Task ZipWithAsync_OnNoneSome_ReturnsNone()
    {
        var result = await None.ZipWithAsync(Some, (l, r) => Task.FromResult((l, r)));

        AssertOption.None(result);
    }

    [Fact]
    public async Task ZipWithAsync_OnNoneNone_ReturnsNone()
    {
        var result = await None.ZipWithAsync(None, (l, r) => Task.FromResult((l, r)));

        AssertOption.None(result);
    }
}