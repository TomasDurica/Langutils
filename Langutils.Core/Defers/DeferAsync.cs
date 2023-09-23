using System;
using System.Threading.Tasks;

namespace Langutils.Core.Defers;

public readonly struct DeferAsync : IAsyncDisposable
{
    private readonly Func<ValueTask> _onDispose;

    public DeferAsync(Func<ValueTask> onDispose)
        => _onDispose = onDispose;

    public ValueTask DisposeAsync()
        => _onDispose();
}