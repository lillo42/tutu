using Tutu.Windows;

namespace Tutu.Events.Commands;

/// <summary>
/// A command that enables mouse event capturing.
/// </summary>
public record EnableMouseCaptureCommand : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write) => write.Write(
            // Normal tracking: Send mouse X & Y on button press and release
            $"{AnsiCodes.CSI}?1000h" +
            // Button-event tracking: Report button motion events (dragging)
            $"{AnsiCodes.CSI}?1002h" +
            // Any-event tracking: Report all motion events
            $"{AnsiCodes.CSI}?1003h" +
            // RXVT mouse mode: Allows mouse coordinates of >223
            $"{AnsiCodes.CSI}?1015h" +
            // SGR mouse mode: Allows mouse coordinates of >223, preferred over RXVT mode
            $"{AnsiCodes.CSI}?1006h"
        );

    public void ExecuteWindowsApi() => WindowsConsole.CurrentIn.EnableMouseCapture();

    /// <inheritdoc />
    bool ICommand.IsAnsiCodeSupported => false;
}