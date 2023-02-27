using Tutu.Windows;

namespace Tutu.Cursor.Commands;

/// <summary>
/// A command that moves the terminal cursor a given number of rows down.
/// </summary>
/// <param name="Count">The number of line to be move down.</param>
/// <remarks>
/// <para>This command is 1 based, meaning `MoveDown(1)` moves the cursor down one cell.</para>
/// <para>Most terminals default 0 argument to 1.</para>
/// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
/// </remarks>
public record MoveDownCursorCommand(ushort Count) : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.CSI}{Count}B");

    /// <inheritdoc />
    public void ExecuteWindowsApi() => WindowsCursor.MoveDown(Count);
}