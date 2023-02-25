using Tutu.Windows2;

namespace Tutu.Cursor.Commands;

/// <summary>
/// A command that hides the terminal cursor.
/// </summary>
/// <remarks>
/// Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record HideCursorCommand : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write) 
        => write.Write($"{AnsiCodes.CSI}?25l");

    /// <inheritdoc />
    public void ExecuteWindowsApi() => WindowsCursor.Hide();
}