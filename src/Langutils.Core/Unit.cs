using System.Collections;

namespace Langutils.Core;

/// <summary>
/// This type is used to represent a unit value (or no value).
/// </summary>
public readonly record struct Unit : IEnumerable<Unit>
{
    /// <summary>
    /// The only instance of the Unit type.
    /// </summary>
    public static readonly Unit Instance = new();

    /// <summary>
    /// Returns an empty enumerable of Unit.
    /// </summary>
    /// <returns></returns>
    public IEnumerator<Unit> GetEnumerator()
    {
        yield break;
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}