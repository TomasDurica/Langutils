using NSubstitute;

namespace Langutils.Core.Tests;

public class ScopeExtensionTests
{
    private static readonly object Value = new();
    private static readonly object OtherValue = new();
    private static readonly Task<object> ValueTask = Task.FromResult(Value);
    private static readonly Task<object> OtherValueTask = Task.FromResult(OtherValue);

    [Fact]
    public void Also_ReturnsOriginalValue()
    {
        var result = Value.Also(v => { });

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task Also_WhenValueIsTask_ReturnsOriginalValue()
    {
        var result = await ValueTask.Also(v => { });

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task AlsoAsync_ReturnsOriginalValue()
    {
        var result = await Value.AlsoAsync(v => Task.CompletedTask);

        Assert.Same(Value, result);
    }

    [Fact]
    public async Task AlsoAsync_WhenValueIsTask_ReturnsOriginalValue()
    {
        var result = await ValueTask.AlsoAsync(v => Task.CompletedTask);

        Assert.Same(Value, result);
    }

    [Fact]
    public void Also_CallsAction()
    {
        var action = Substitute.For<Action<object>>();

        Value.Also(action);

        var call = Assert.Single(action.ReceivedCalls());
        Assert.Same(Value, call.GetArguments().Single());
    }

    [Fact]
    public async Task Also_WhenValueIsTask_CallsAction()
    {
        var action = Substitute.For<Action<object>>();

        await ValueTask.Also(action);

        var call = Assert.Single(action.ReceivedCalls());
        Assert.Same(Value, call.GetArguments().Single());
    }

    [Fact]
    public async Task AlsoAsync_CallsAction()
    {
        var action = Substitute.For<Func<object, Task>>();
        action.Invoke(Arg.Any<object>()).Returns(Task.CompletedTask);

        await Value.AlsoAsync(action);

        var call = Assert.Single(action.ReceivedCalls());
        Assert.Same(Value, call.GetArguments().Single());
    }

    [Fact]
    public async Task AlsoAsync_WhenValueIsTask_CallsAction()
    {
        var action = Substitute.For<Func<object, Task>>();
        action.Invoke(Arg.Any<object>()).Returns(Task.CompletedTask);

        await ValueTask.AlsoAsync(action);

        var call = Assert.Single(action.ReceivedCalls());
        Assert.Same(Value, call.GetArguments().Single());
    }

    [Fact]
    public void Let_ReturnsProjectedValue()
    {
        var result = Value.Let(v => OtherValue);

        Assert.Same(OtherValue, result);
    }

    [Fact]
    public async Task Let_WhenValueIsTask_ReturnsProjectedValue()
    {
        var result = await ValueTask.Let(v => OtherValue);

        Assert.Same(OtherValue, result);
    }

    [Fact]
    public async Task LetAsync_ReturnsProjectedValue()
    {
        var result = await Value.LetAsync(v => OtherValueTask);

        Assert.Same(OtherValue, result);
    }

    [Fact]
    public async Task LetAsync_WhenValueIsTask_ReturnsProjectedValue()
    {
        var result = await ValueTask.LetAsync(v => OtherValueTask);

        Assert.Same(OtherValue, result);
    }

    [Fact]
    public void Let_CallsProjection()
    {
        var projection = Substitute.For<Func<object, object>>();
        projection.Invoke(Arg.Any<object>()).Returns(OtherValue);

        Value.Let(projection);

        var call = Assert.Single(projection.ReceivedCalls());
        Assert.Same(Value, call.GetArguments().Single());
    }

    [Fact]
    public async Task Let_WhenValueIsTask_CallsProjection()
    {
        var projection = Substitute.For<Func<object, object>>();
        projection.Invoke(Arg.Any<object>()).Returns(OtherValue);

        await ValueTask.Let(projection);

        var call = Assert.Single(projection.ReceivedCalls());
        Assert.Same(Value, call.GetArguments().Single());
    }

    [Fact]
    public async Task LetAsync_CallsProjection()
    {
        var projection = Substitute.For<Func<object, Task<object>>>();
        projection.Invoke(Arg.Any<object>()).Returns(OtherValueTask);

        await Value.Let(projection);

        var call = Assert.Single(projection.ReceivedCalls());
        Assert.Same(Value, call.GetArguments().Single());
    }

    [Fact]
    public async Task LetAsync_WhenValueIsTask_CallsProjection()
    {
        var projection = Substitute.For<Func<object, Task<object>>>();
        projection.Invoke(Arg.Any<object>()).Returns(OtherValueTask);

        await ValueTask.Let(projection);

        var call = Assert.Single(projection.ReceivedCalls());
        Assert.Same(Value, call.GetArguments().Single());
    }
}