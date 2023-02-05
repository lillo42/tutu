namespace Erised.Events.Commands;

/// <summary>
/// A command that disables extra kinds of keyboard events.
///
/// Specifically, it pops one level of keyboard enhancement flags.
///
/// See [`PushKeyboardEnhancementFlags`] and <https://sw.kovidgoyal.net/kitty/keyboard-protocol/> for more information.
/// </summary>
public record PopKeyboardEnhancementFlagsCommand : ICommand
{
    public void WriteAnsi(TextWriter write) 
        => write.Write($"{AnsiCodes.CSI}<1u");

    public void ExecuteWindowsApi() => throw new NotSupportedException("Windows API does not support keyboard enhancements");
    
    bool ICommand.IsAnsiCodeSupported => false;
}