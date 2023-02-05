namespace Erised.Events.Commands;

/// <summary>
/// A command that enables the [kitty keyboard protocol](https://sw.kovidgoyal.net/kitty/keyboard-protocol/), which adds extra information to keyboard events and removes ambiguity for modifier keys.
///
/// It should be paired with [`PopKeyboardEnhancementFlags`] at the end of execution.
/// </summary>
/// <param name="Flags">The <see cref="KeyEventKind"/>.</param>
/// <remarks>
/// * [kitty terminal](https://sw.kovidgoyal.net/kitty/)
/// * [foot terminal](https://codeberg.org/dnkl/foot/issues/319)
/// * [WezTerm terminal](https://wezfurlong.org/wezterm/config/lua/config/enable_kitty_keyboard.html)
/// * [notcurses library](https://github.com/dankamongmen/notcurses/issues/2131)
/// * [neovim text editor](https://github.com/neovim/neovim/pull/18181)
/// * [kakoune text editor](https://github.com/mawww/kakoune/issues/4103)
/// * [dte text editor](https://gitlab.com/craigbarnes/dte/-/issues/138)
/// </remarks>
public record PushKeyboardEnhancementFlagsCommand(KeyboardEnhancementFlags Flags) : ICommand
{
    public void WriteAnsi(TextWriter write) => write.Write($"{AnsiCodes.CSI}>{(int)Flags}u");

    public void ExecuteWindowsApi() => throw new NotSupportedException("Windows API does not support keyboard enhancement flags.");
    
    bool ICommand.IsAnsiCodeSupported => false;
}