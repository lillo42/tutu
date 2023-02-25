using Tutu.Windows2;

namespace Tutu.Cursor.Commands;

/// <summary>
/// A command that moves the terminal cursor a given number of columns to the left.
/// </summary>
/// <param name="Count">The number of column to be move to left.</param>
/// <remarks>
/// <para>This command is 1 based, meaning `MoveLeft(1)` moves the cursor left one cell.</para>
/// <para>Most terminals default 0 argument to 1.</para>
/// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
/// </remarks>
public record MoveLeftCursorCommand(ushort Count) : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.CSI}{Count}D");

    /// <inheritdoc />
    public void ExecuteWindowsApi() => WindowsCursor.MoveLeft(Count);
}