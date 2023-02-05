namespace Erised.Terminal;

/// <summary>
/// Different ways to clear the terminal buffer.
/// </summary>
public enum ClearType
{
    /// <summary>
    /// All cells.
    /// </summary>
    All,

    /// <summary>
    /// All plus history
    /// </summary>
    Purge,

    /// <summary>
    /// All cells from the cursor position downwards.
    /// </summary>
    FromCursorDown,

    /// <summary>
    /// All cells from the cursor position upwards.
    /// </summary>
    FromCursorUp,

    /// <summary>
    /// All cells at the cursor row.
    /// </summary>
    CurrentLine,

    /// <summary>
    /// All cells from the cursor position until the new line.
    /// </summary>
    UntilNewLine,
}