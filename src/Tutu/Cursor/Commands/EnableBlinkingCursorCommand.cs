namespace Tutu.Cursor.Commands;

/// <summary>
/// A command that enables blinking of the terminal cursor.
/// </summary>
/// <remarks>
/// Some Unix terminals (ex: GNOME and Konsole) as well as Windows versions lower than Windows 10 do not support this functionality.
///
/// Use <see cref="SetCursorStyleCursorCommand"/> for better cross-compatibility.
///
/// Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record EnableBlinkingCursorCommand : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.CSI}?12h");

    /// <inheritdoc />
    public void ExecuteWindowsApi() { }
}