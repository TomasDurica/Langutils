namespace Langutils.Core.Defers;

public readonly struct DeferWith<TContext> : IDisposable
{
    private readonly Action<TContext> _onDispose;
    private readonly TContext _context;

    public DeferWith(Action<TContext> onDispose, TContext context)
    {
        _onDispose = onDispose;
        _context = context;
    }

    public void Dispose()
        => _onDispose(_context);
}