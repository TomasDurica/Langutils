using Langutils.Core.Defers;
using NSubstitute;

namespace Langutils.Core.Tests.Defers;

public class DeferTests
{
    [Fact]
    public void Defer_WhenDisposed_ShouldCallOnDispose()
    {
        var onDispose = Substitute.For<Action>();

        using (new Defer(onDispose))
        {
            Assert.Empty(onDispose.ReceivedCalls());
        }

        Assert.Single(onDispose.ReceivedCalls());
    }

    [Fact]
    public void DeferWith_WhenDisposed_ShouldCallOnDisposeWithContext()
    {
        var onDispose = Substitute.For<Action<object>>();
        var context = new object();

        using (new DeferWith<object>(onDispose, context))
        {
            Assert.Empty(onDispose.ReceivedCalls());
        }

        Assert.Single(onDispose.ReceivedCalls());
        Assert.True(onDispose.ReceivedCalls().All(call => call.GetArguments().Single() == context));
    }

    [Fact]
    public async Task DeferAsync_WhenDisposed_ShouldCallOnDispose()
    {
        var onDispose = Substitute.For<Func<ValueTask>>();

        await using (new DeferAsync(onDispose))
        {
            Assert.Empty(onDispose.ReceivedCalls());
        }

        Assert.Single(onDispose.ReceivedCalls());
    }

    [Fact]
    public async Task DeferAsyncWith_WhenDisposed_ShouldCallOnDisposeWithContext()
    {
        var onDispose = Substitute.For<Func<object, ValueTask>>();
        var context = new object();

        await using (new DeferAsyncWith<object>(onDispose, context))
        {
            Assert.Empty(onDispose.ReceivedCalls());
        }

        Assert.Single(onDispose.ReceivedCalls());
        Assert.True(onDispose.ReceivedCalls().All(call => call.GetArguments().Single() == context));
    }
}