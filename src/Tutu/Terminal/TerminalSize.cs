namespace Tutu.Terminal;

/// <summary>
/// The terminal size.
/// </summary>
/// <param name="Width">The width (number of column). </param>
/// <param name="Height">The height (number of row).</param>
public readonly record struct TerminalSize(int Width, int Height);
