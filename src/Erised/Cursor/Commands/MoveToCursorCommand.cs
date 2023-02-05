namespace Erised.Cursor.Commands;

/// <summary>
/// A command that moves the terminal cursor to the given position (column, row).
/// </summary>
/// <param name="Column">The desired column.</param>
/// <param name="Row">The desired row.</param>
/// <remarks>
/// * Top left cell is represented as `0,0`.
/// * Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record MoveToCursorCommand(ushort Column, ushort Row) : ICommand
{
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.CSI}{Row + 1};{Column + 1}H");

    public void ExecuteWindowsApi() => Windows.MoveTo(Column, Row);
}