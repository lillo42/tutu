using Tutu.Windows;

namespace Tutu.Cursor.Commands;

/// <summary>
/// A command that moves the terminal cursor a given number of columns to the right.
/// </summary>
/// <param name="Count">The number of column to be move.</param>
/// <remarks>
/// <para>This command is 1 based, meaning `MoveRight(1)` moves the cursor right one cell.</para>
/// <para>Most terminals default 0 argument to 1.</para>
/// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
/// </remarks>
public record MoveRightCursorCommand(ushort Count) : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.CSI}{Count}C");

    /// <inheritdoc />
    public void ExecuteWindowsApi() => WindowsCursor.MoveRight(Count);
}
