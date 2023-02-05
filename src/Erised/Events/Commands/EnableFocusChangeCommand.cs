using Erised.Events.Commands;

namespace Erised.Events;

/// <summary>
/// A command that enables focus event emission.
///
/// It should be paired with <see cref="DisableFocusChangeCommand"/> at the end of execution.
///
/// Focus events can be captured with [read](./fn.read.html)/[poll](./fn.poll.html).
/// </summary>
public record EnableFocusChangeCommand : ICommand
{
    public void WriteAnsi(TextWriter write) 
        => write.Write($"{AnsiCodes.CSI}?1004h");

    public void ExecuteWindowsApi() { }
}