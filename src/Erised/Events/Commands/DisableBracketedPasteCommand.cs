
namespace Erised.Events.Commands;

/// <summary>
/// A command that disables bracketed paste mode.
/// </summary>
public record DisableBracketedPasteCommand : ICommand
{
    public void WriteAnsi(TextWriter write) 
        => write.Write($"{AnsiCodes.CSI}?2004l");

    public void ExecuteWindowsApi() { }
}