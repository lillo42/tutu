namespace Erised.Terminal.Commands;

/// <summary>
/// A command that sets the terminal buffer size `(columns, rows)`.
/// </summary>
/// <param name="Column">The new column size.</param>
/// <param name="Row">The new row size.</param>
/// <remarks>
/// * Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record SetSizeCommand(ushort Column, ushort Row) : ICommand
{
    public void WriteAnsi(TextWriter write) => write.Write($"{AnsiCodes.CSI}8;{Column};{Row}t");

    public void ExecuteWindowsApi() => Windows.Terminal.Size = (Column, Row);
}