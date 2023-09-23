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
    public async Task WhereAsync_OnSome_WhenMatchesPredicate_ReturnsSome()
    {
        var result = await Some.WhereAsync(v => Task.FromResult(v == Value));

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task WhereAsync_OnSome_WhenDoesNotMatchPredicate_ReturnsNone()
    {
        var result = await Some.WhereAsync(v => Task.FromResult(v != Value));

        AssertOption.None(result);
    }

    [Fact]
    public async Task WhereAsync_OnNone_ReturnsNone()
    {
        var result = await None.WhereAsync(_ => Task.FromResult(true));

        AssertOption.None(result);
    }

    [Fact]
    public async Task SelectManyAsync_OnSome_WhenSelectorReturnsSome_ReturnsSome()
    {
        var result = await Some.SelectManyAsync(v => Task.FromResult(Option.Some(v)));

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task SelectManyAsync_OnSome_WhenSelectorReturnsNone_ReturnsNone()
    {
        var result = await Some.SelectManyAsync(_ => TaskNone);

        AssertOption.None(result);
    }

    [Fact]
    public async Task SelectManyAsync_OnNone_ReturnsNone()
    {
        var result = await None.SelectManyAsync(_ => TaskSome);

        AssertOption.None(result);
    }

    [Fact]
    public async Task SelectAsync_OnSome_ReturnsSome()
    {
        var result = await Some.SelectAsync(Task.FromResult);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task SelectAsync_OnNone_ReturnsNone()
    {
        var result = await None.SelectAsync(Task.FromResult);

        AssertOption.None(result);
    }

    [Fact]
    public async Task SelectOrAsync_OnSome_ReturnsSome()
    {
        var result = await Some.SelectOrAsync(OtherValue, Task.FromResult);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task SelectOrAsync_OnNone_ReturnsDefaultValue()
    {
        var result = await None.SelectOrAsync(OtherValue, Task.FromResult);

        AssertOption.Some(OtherValue, result);
    }

    [Fact]
    public async Task SelectOrElseAsync_OnSome_ReturnsSome()
    {
        var result = await Some.SelectOrElseAsync(() => OtherValue, Task.FromResult);

        AssertOption.Some(Value, result);
    }

    [Fact]
    public async Task SelectOrElseAsync_OnSome_DoesNotCallDefaultValueProvider()
    {
        var defaultValueProvider = Substitute.For<Func<object>>();
        defaultValueProvider.Invoke().Returns(OtherValue);

        await Some.SelectOrElseAsync(defaultValueProvider, Task.FromResult);

        Assert.Empty(defaultValueProvider.ReceivedCalls());
    }

    [Fact]
    public async Task SelectOrElseAsync_OnNone_ReturnsDefaultValue()
    {
        var result = await None.SelectOrElseAsync(() => Value, Task.FromResult);

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
        var result = await Some.AndThenAsync(() => TaskOtherSome);

        AssertOption.Equal(OtherSome, result);
    }

    [Fact]
    public async Task AndThenAsync_OnSomeNone_ReturnsNone()
    {
        var result = await Some.AndThenAsync(() => TaskNone);

        AssertOption.None(result);
    }

    [Fact]
    public async Task AndThenAsync_OnNoneSome_ReturnsNone()
    {
        var result = await None.AndThenAsync(() => TaskSome);

        AssertOption.None(result);
    }

    [Fact]
    public async Task AndThenAsync_OnNoneNone_ReturnsNone()
    {
        var result = await None.AndThenAsync(() => TaskNone);

        AssertOption.None(result);
    }

    [Fact]
    public async Task AndThenAsync_OnNone_DoesNotCallOptionProvider()
    {
        var optionProvider = Substitute.For<Func<Task<Option<object>>>>();
        optionProvider.Invoke().Returns(Some);

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