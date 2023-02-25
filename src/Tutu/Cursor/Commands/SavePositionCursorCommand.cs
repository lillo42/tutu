using Tutu.Windows2;

namespace Tutu.Cursor.Commands;

/// <summary>
/// A command that saves the current terminal cursor position.
/// </summary>
/// <remarks>
/// The cursor position is stored globally.
///
/// Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record SavePositionCursorCommand : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.ESC}7");

    /// <inheritdoc />
    public void ExecuteWindowsApi() => WindowsCursor.SavePosition();
}