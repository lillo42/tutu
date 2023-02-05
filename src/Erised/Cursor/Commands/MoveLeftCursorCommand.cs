namespace Erised.Cursor.Commands;

/// <summary>
/// A command that moves the terminal cursor a given number of columns to the left.
/// </summary>
/// <param name="Count">The number of column to be move to left.</param>
/// <remarks>
/// * This command is 1 based, meaning `MoveLeft(1)` moves the cursor left one cell.
/// * Most terminals default 0 argument to 1.
/// * Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record MoveLeftCursorCommand(ushort Count) : ICommand
{
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.CSI}{Count}D");

    public void ExecuteWindowsApi() => Windows.MoveLeft(Count);
}