namespace Erised.Cursor.Commands;

/// <summary>
/// A command that moves the terminal cursor to the given row on the current column.
/// </summary>
/// <param name="NewRow">The target row.</param>
/// <remarks>
///* This command is 0 based, meaning 0 is the topmost row.
/// * Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record MoveToRowCursorCommand(ushort NewRow) : ICommand
{
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.CSI}{NewRow + 1}d");

    public void ExecuteWindowsApi() => Windows.MoveToRow(NewRow);
}