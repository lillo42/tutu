namespace Tutu.Events.Commands;

/// <summary>
/// A command that enables the <see href="https://sw.kovidgoyal.net/kitty/keyboard-protocol/">kitty keyboard protocol</see>,
/// which adds extra information to keyboard events and removes ambiguity for modifier keys.
/// </summary>
/// <param name="Flags">The <see cref="KeyEventKind"/>.</param>
/// <remarks>
/// <para>It should be paired with <see cref="PopKeyboardEnhancementFlagsCommand"/> at the end of execution.</para>
/// <list type="bullet">
///     <item><see href="https://sw.kovidgoyal.net/kitty/">kitty terminal</see></item>
///     <item><see href="https://codeberg.org/dnkl/foot/issues/319">foot terminal</see></item>
///     <item><see href="https://wezfurlong.org/wezterm/config/lua/config/enable_kitty_keyboard.html">WezTerm terminal</see></item>
///     <item><see href="https://github.com/dankamongmen/notcurses/issues/2131">notcurses library</see></item>
///     <item><see href="https://github.com/neovim/neovim/pull/18181">neovim text editor</see></item>
///     <item><see href="https://github.com/mawww/kakoune/issues/4103">kakoune text editor</see></item>
///     <item><see href="https://gitlab.com/craigbarnes/dte/-/issues/138">dte text editor</see></item>
/// </list> 
/// </remarks>
public record PushKeyboardEnhancementFlagsCommand(KeyboardEnhancementFlags Flags) : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write) => write.Write($"{AnsiCodes.CSI}>{(int)Flags}u");

    public void ExecuteWindowsApi() =>
        throw new NotSupportedException("Windows API does not support keyboard enhancement flags.");

    /// <inheritdoc />
    bool ICommand.IsAnsiCodeSupported => false;
}