namespace Erised.Cursor.Commands;

/// <summary>
/// A command that saves the current terminal cursor position.
/// </summary>
/// <remarks>
/// - The cursor position is stored globally.
/// - Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record SavePositionCursorCommand : ICommand
{
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.ESC}7");

    public void ExecuteWindowsApi() => Windows.SavePosition();
}