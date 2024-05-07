using Tutu.Windows;

namespace Tutu.Terminal.Commands;

/// <summary>
/// A command that sets the terminal buffer size `(columns, rows)`.
/// </summary>
/// <param name="Column">The new column size.</param>
/// <param name="Row">The new row size.</param>
/// <remarks>
/// * Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public sealed record SetSizeCommand(ushort Column, ushort Row) : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write) => write.Write($"{AnsiCodes.CSI}8;{Column};{Row}t");

    /// <inheritdoc />
    public void ExecuteWindowsApi() => WindowsTerminal.SetSize(new(Column, Row));
}
