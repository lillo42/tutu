namespace Erised.Cursor.Commands;

/// <summary>
/// A command that moves the terminal cursor up the given number of lines,
/// and moves it to the first column.
/// </summary>
/// <param name="Count"></param>
/// <remarks>
/// * This command is 1 based, meaning `MoveToPreviousLine(1)` moves to the previous line.
/// * Most terminals default 0 argument to 1.
/// * Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record MoveToPreviousLineCursorCommand(ushort Count) : ICommand
{
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.CSI}{Count}F");

    public void ExecuteWindowsApi() => Windows.MoveToPreviousLine(Count);
}