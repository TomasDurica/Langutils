namespace Langutils.Core.Defers;

/// <summary>
/// This type is used to create defers with context - a function that should be run at the end of the scope.
/// </summary>
/// <example>
/// using var _ = new Defer(ctx => Console.WriteLine(ctx), "End of scope!");
/// </example>
/// <typeparam name="TContext"></typeparam>
public readonly struct DeferWith<TContext> : IDisposable
{
    private readonly Action<TContext> _onDispose;
    private readonly TContext _context;

    /// <summary>
    /// Creates a new defer with context.
    /// </summary>
    /// <param name="onDispose"></param>
    /// <param name="context"></param>
    public DeferWith(Action<TContext> onDispose, TContext context)
    {
        _onDispose = onDispose;
        _context = context;
    }

    /// <summary>
    /// Runs the defer.
    /// </summary>
    public void Dispose()
        => _onDispose(_context);
}