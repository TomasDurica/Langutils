namespace Langutils.Core.Defers;

/// <summary>
/// This type is used to create async defers - an async function that should be run at the end of the scope.
/// </summary>
/// <example>
/// await using var _ = new Defer(() => Console.WriteLine("End of scope!"));
/// </example>
public readonly struct DeferAsync : IAsyncDisposable
{
    private readonly Func<ValueTask> _onDispose;

    /// <summary>
    /// Creates a new async defer.
    /// </summary>
    /// <param name="onDispose"></param>
    public DeferAsync(Func<ValueTask> onDispose)
        => _onDispose = onDispose;

    /// <summary>
    /// Runs the async defer.
    /// </summary>
    /// <returns></returns>
    public ValueTask DisposeAsync()
        => _onDispose();
}