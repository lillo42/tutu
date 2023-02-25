namespace Tutu.Events.Commands;

/// <summary>
/// A command that disables focus event emission.
/// </summary>
public record DisableFocusChangeCommand : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write) => write.Write($"{AnsiCodes.CSI}?1004l");

    /// <inheritdoc />
    public void ExecuteWindowsApi() { }
}