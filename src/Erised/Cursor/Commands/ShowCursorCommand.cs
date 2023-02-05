namespace Erised.Cursor.Commands;

/// <summary>
/// A command that shows the terminal cursor.
/// </summary>
/// <remarks>
/// Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record ShowCursorCommand : ICommand
{
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.CSI}?25h");

    public void ExecuteWindowsApi() => Windows.Show();
}