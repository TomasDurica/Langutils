namespace Langutils.Core.Defers;

public readonly struct DeferAsyncWith<TContext> : IAsyncDisposable
{
    private readonly Func<TContext, ValueTask> _onDispose;
    private readonly TContext _context;

    public DeferAsyncWith(Func<TContext, ValueTask> onDispose, TContext context)
    {
        _onDispose = onDispose;
        _context = context;
    }

    public ValueTask DisposeAsync()
        => _onDispose(_context);
}