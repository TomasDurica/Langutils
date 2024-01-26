namespace Langutils.Core.Defers;

/// <summary>
/// This type is used to create async defers with context - an async function that should be run at the end of the scope.
/// </summary>
/// <example>
/// await using var _ = new Defer(ctx => Console.WriteLine(ctx), "End of scope!");
/// </example>
/// <typeparam name="TContext"></typeparam>
public readonly struct DeferAsyncWith<TContext> : IAsyncDisposable
{
    private readonly Func<TContext, ValueTask> _onDispose;
    private readonly TContext _context;

    /// <summary>
    /// Creates a new async defer with context.
    /// </summary>
    /// <param name="onDispose"></param>
    /// <param name="context"></param>
    public DeferAsyncWith(Func<TContext, ValueTask> onDispose, TContext context)
    {
        _onDispose = onDispose;
        _context = context;
    }

    /// <summary>
    /// Runs the async defer.
    /// </summary>
    /// <returns></returns>
    public ValueTask DisposeAsync()
        => _onDispose(_context);
}