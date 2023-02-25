using Tutu.Windows2;

namespace Tutu.Cursor.Commands;

/// <summary>
/// A command that moves the terminal cursor up the given number of lines,
/// and moves it to the first column.
/// </summary>
/// <param name="Count"></param>
/// <remarks>
/// <para>This command is 1 based, meaning `MoveToPreviousLine(1)` moves to the previous line.</para>
/// <para>Most terminals default 0 argument to 1.</para>
/// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
/// </remarks>
public record MoveToPreviousLineCursorCommand(ushort Count) : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.CSI}{Count}F");

    /// <inheritdoc />
    public void ExecuteWindowsApi() => WindowsCursor.MoveToPreviousLine(Count);
}