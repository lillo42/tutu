using Tutu.Windows;

namespace Tutu.Events.Commands;

/// <summary>
/// A command that disables mouse event capturing.
/// </summary>
public record DisableMouseCaptureCommand : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => write.Write(
            $"{AnsiCodes.CSI}?1006l" +
            $"{AnsiCodes.CSI}?1015l" +
            $"{AnsiCodes.CSI}?1003l" +
            $"{AnsiCodes.CSI}?1002l" +
            $"{AnsiCodes.CSI}?1000l"
        );

    /// <inheritdoc />
    public void ExecuteWindowsApi() => WindowsConsole.CurrentIn.DisableMouseCapture();


    /// <inheritdoc />
    bool ICommand.IsAnsiCodeSupported => false;
}
