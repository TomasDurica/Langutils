namespace Langutils.Core.Defers;

/// <summary>
/// This type is used to create defers - a function that should be run at the end of the scope.
/// </summary>
/// <example>
/// using var _ = new Defer(() => Console.WriteLine("End of scope!"));
/// </example>
public readonly struct Defer : IDisposable
{
    private readonly Action _onDispose;

    /// <summary>
    /// Creates a new defer.
    /// </summary>
    /// <param name="onDispose"></param>
    public Defer(Action onDispose)
        => _onDispose = onDispose;

    /// <summary>
    /// Runs the defer.
    /// </summary>
    public void Dispose()
        => _onDispose();
}