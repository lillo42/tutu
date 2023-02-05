namespace Erised.Cursor.Commands;

/// <summary>
/// A command that moves the terminal cursor a given number of columns to the right.
/// </summary>
/// <param name="Count">The number of column to be move.</param>
/// <remarks>
/// * This command is 1 based, meaning `MoveRight(1)` moves the cursor right one cell.
/// * Most terminals default 0 argument to 1.
/// * Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record MoveRightCursorCommand(ushort Count) : ICommand
{
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.CSI}{Count}C");

    public void ExecuteWindowsApi() => Windows.MoveRight(Count);
}