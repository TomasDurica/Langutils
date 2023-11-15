namespace Langutils.Core;

/// <summary>
/// This type is used to represent a unit value (or no value).
/// </summary>
public readonly record struct Unit
{
    /// <summary>
    /// The only instance of the Unit type.
    /// </summary>
    public static readonly Unit Instance = new();
}