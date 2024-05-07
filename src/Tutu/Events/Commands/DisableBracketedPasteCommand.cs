namespace Tutu.Events.Commands;

/// <summary>
/// A command that disables bracketed paste mode.
/// </summary>
public sealed record DisableBracketedPasteCommand : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.CSI}?2004l");

    /// <inheritdoc />
    public void ExecuteWindowsApi() { }
}
