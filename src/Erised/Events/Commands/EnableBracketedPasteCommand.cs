namespace Erised.Events.Commands;

/// <summary>
/// A command that enables [bracketed paste mode](https://en.wikipedia.org/wiki/Bracketed-paste).
///
/// It should be paired with <see cref="DisableBracketedPasteCommand"/> at the end of execution.
///
/// This is not supported in older Windows terminals without
/// [virtual terminal sequences](https://docs.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences).
/// </summary>
public record EnableBracketedPasteCommand : ICommand
{
    public void WriteAnsi(TextWriter write) 
        => write.Write($"{AnsiCodes.CSI}?2004h");

    public void ExecuteWindowsApi() => throw new NotSupportedException("Bracketed paste not implemented in the legacy Windows API.");
}