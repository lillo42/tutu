namespace Tutu.Events;

/// <summary>
/// Represents a keyboard event kind.
/// </summary>
public enum KeyEventKind
{
    /// <summary>
    /// The key was pressed.
    /// </summary>
    Press,

    /// <summary>
    /// The key is repeat.
    /// </summary>
    Repeat,

    /// <summary>
    /// The key was released.
    /// </summary>
    Release,
}
