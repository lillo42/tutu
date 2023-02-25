using Tutu.Windows2;

namespace Tutu.Cursor.Commands;

/// <summary>
/// A command that moves the terminal cursor to the given column on the current row.
/// </summary>
/// <param name="NewColumn">The target column.</param>
/// <remarks>
/// <para>This command is 0 based, meaning 0 is the leftmost column.</para>
/// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
/// </remarks>
public record MoveToColumnCursorCommand(ushort NewColumn) : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.CSI}{NewColumn + 1}G");

    /// <inheritdoc />
    public void ExecuteWindowsApi() => WindowsCursor.MoveToColumn(NewColumn);
}