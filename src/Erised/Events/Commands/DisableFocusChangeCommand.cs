namespace Erised.Events.Commands;

/// <summary>
/// A command that disables focus event emission.
/// </summary>
public record DisableFocusChangeCommand : ICommand
{
    public void WriteAnsi(TextWriter write) => write.Write($"{AnsiCodes.CSI}?1004l");

    public void ExecuteWindowsApi() { }
}