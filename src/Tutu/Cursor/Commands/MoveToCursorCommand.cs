using Tutu.Windows2;

namespace Tutu.Cursor.Commands;

/// <summary>
/// A command that moves the terminal cursor to the given position (column, row).
/// </summary>
/// <param name="Column">The desired column.</param>
/// <param name="Row">The desired row.</param>
/// <remarks>
/// <para>Top left cell is represented as `0,0`.</para>
/// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
/// </remarks>
public record MoveToCursorCommand(ushort Column, ushort Row) : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.CSI}{Row + 1};{Column + 1}H");

    /// <inheritdoc />
    public void ExecuteWindowsApi() => WindowsCursor.MoveTo(Column, Row);
}