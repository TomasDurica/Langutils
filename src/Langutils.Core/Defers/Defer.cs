namespace Langutils.Core.Defers;

public readonly struct Defer : IDisposable
{
    private readonly Action _onDispose;

    public Defer(Action onDispose)
        => _onDispose = onDispose;

    public void Dispose()
        => _onDispose();
}