using Tutu.Windows2;

namespace Tutu.Cursor.Commands;

/// <summary>
/// A command that moves the terminal cursor a given number of rows up.
/// </summary>
/// <param name="Count">The number of line to be up.</param>
/// <remarks>
/// This command is 1 based, meaning `MoveUp(1)` moves the cursor up one cell.
///
/// Most terminals default 0 argument to 1.
///
/// Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record MoveUpCursorCommand(ushort Count) : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.CSI}{Count}A");

    /// <inheritdoc />
    public void ExecuteWindowsApi() => WindowsCursor.MoveUp(Count);
}