namespace Tutu.Events.Commands;

/// <summary>
/// A command that disables extra kinds of keyboard events.
/// </summary>
/// <remarks>
/// <para>Specifically, it pops one level of keyboard enhancement flags.</para>
/// <para>See <see cref="PushKeyboardEnhancementFlagsCommand"/> and https://sw.kovidgoyal.net/kitty/keyboard-protocol/ for more information.</para>
/// </remarks>
public record PopKeyboardEnhancementFlagsCommand : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.CSI}<1u");

    /// <inheritdoc />
    public void ExecuteWindowsApi() =>
        throw new NotSupportedException("Windows API does not support keyboard enhancements");

    /// <inheritdoc />
    bool ICommand.IsAnsiCodeSupported => false;
}
