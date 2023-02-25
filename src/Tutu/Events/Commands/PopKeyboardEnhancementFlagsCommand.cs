namespace Tutu.Events.Commands;

/// <summary>
/// A command that disables extra kinds of keyboard events.
///
/// Specifically, it pops one level of keyboard enhancement flags.
///
/// See <see cref="PushKeyboardEnhancementFlagsCommand"/> and https://sw.kovidgoyal.net/kitty/keyboard-protocol/ for more information.
/// </summary>
public record PopKeyboardEnhancementFlagsCommand : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write) 
        => write.Write($"{AnsiCodes.CSI}<1u");

    public void ExecuteWindowsApi() => throw new NotSupportedException("Windows API does not support keyboard enhancements");
    
    /// <inheritdoc />
    bool ICommand.IsAnsiCodeSupported => false;
}