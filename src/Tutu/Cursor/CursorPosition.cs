namespace Tutu.Cursor;

/// <summary>
/// The cursor position.
/// </summary>
public readonly struct CursorPosition
{
    /// <summary>
    /// Initialize a new instance of <see cref="CursorPosition"/>.
    /// </summary>
    /// <param name="column">The column.</param>
    /// <param name="row">The row.</param>
    public CursorPosition(int column, int row)
    {
        Column = column;
        Row = row;
    }

    /// <summary>
    /// The cursor column.
    /// </summary>
    public int Column { get; }

    /// <summary>
    /// The cursor row.
    /// </summary>
    public int Row { get; }

    /// <summary>
    /// The deconstructor.
    /// </summary>
    /// <param name="column">The target column.</param>
    /// <param name="row">The target row.</param>
    public void Deconstruct(out int column, out int row)
    {
        column = Column;
        row = Row;
    }
}