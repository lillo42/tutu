namespace Tutu.Terminal;

/// <summary>
/// The terminal size.
/// </summary>
public readonly struct TerminalSize
{
    /// <summary>
    /// The width (number of column). 
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// The height (number of row).
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Initialize a new instance of <see cref="TerminalSize"/>.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public TerminalSize(int width, int height)
    {
        Width = width;
        Height = height;
    }

    /// <summary>
    /// The deconstructor.
    /// </summary>
    /// <param name="width">The target width.</param>
    /// <param name="height">The target height.</param>
    public void Deconstruct(out int width, out int height)
    {
        width = Width;
        height = Height;
    }
}