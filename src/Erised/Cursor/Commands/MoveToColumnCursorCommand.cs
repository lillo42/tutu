namespace Erised.Cursor.Commands;

/// <summary>
/// A command that moves the terminal cursor to the given column on the current row.
/// </summary>
/// <param name="NewColumn">The target column.</param>
/// <remarks>
/// * This command is 0 based, meaning 0 is the leftmost column.
/// * Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record MoveToColumnCursorCommand(ushort NewColumn) : ICommand
{
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.CSI}{NewColumn + 1}G");

    public void ExecuteWindowsApi() => Windows.MoveToColumn(NewColumn);
}