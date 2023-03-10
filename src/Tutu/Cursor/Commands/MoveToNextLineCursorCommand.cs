using Tutu.Windows;

namespace Tutu.Cursor.Commands;

/// <summary>
/// A command that moves the terminal cursor down the given number of lines,
/// and moves it to the first column.
/// </summary>
/// <param name="Count">The number of line to move forward.</param>
/// <remarks>
/// <para>This command is 1 based, meaning `MoveToNextLine(1)` moves to the next line.</para>
/// <para>Most terminals default 0 argument to 1.</para>
/// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
/// </remarks>
public record MoveToNextLineCursorCommand(ushort Count) : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.CSI}{Count}E");

    /// <inheritdoc />
    public void ExecuteWindowsApi() => WindowsCursor.MoveToNextLine(Count);
}
