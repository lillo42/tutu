namespace Erised.Events.Commands;

/// <summary>
/// A command that disables mouse event capturing.
/// </summary>
public record DisableMouseCaptureCommand : ICommand
{
    public void WriteAnsi(TextWriter write) 
        => write.Write(
            $"{AnsiCodes.CSI}?1006l" +
            $"{AnsiCodes.CSI}?1015l" +
            $"{AnsiCodes.CSI}?1003l" +
            $"{AnsiCodes.CSI}?1002l" +
            $"{AnsiCodes.CSI}?1000l"
        );

    public void ExecuteWindowsApi() 
        => Windows.Event.DisableMouseCapture();


    bool ICommand.IsAnsiCodeSupported => false;
}