using Tutu.Windows;

namespace Tutu.Cursor.Commands;

/// <summary>
/// A command that shows the terminal cursor.
/// </summary>
/// <remarks>
/// Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public sealed record ShowCursorCommand : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.CSI}?25h");

    /// <inheritdoc />
    public void ExecuteWindowsApi() => WindowsCursor.Show();
}
