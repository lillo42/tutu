using Erised.Events;
using Erised.Events.Commands;

namespace Erised.Commands;

public static class Events
{
    /// <summary>
    /// A command that enables mouse event capturing.
    /// </summary>
    public static ICommand EnableMouseCapture { get; } = new EnableMouseCaptureCommand();

    /// <summary>
    /// A command that enables mouse event capturing.
    /// </summary>
    public static ICommand DisableMouseCapture { get; } = new DisableMouseCaptureCommand();

    /// <summary>
    /// A command that enables the [kitty keyboard protocol](https://sw.kovidgoyal.net/kitty/keyboard-protocol/), which adds extra information to keyboard events and removes ambiguity for modifier keys.
    ///
    /// It should be paired with [`PopKeyboardEnhancementFlags`] at the end of execution.
    /// </summary>
    /// <param name="flags">The <see cref="KeyboardEnhancementFlags"/></param>
    /// <remarks>
    /// * [kitty terminal](https://sw.kovidgoyal.net/kitty/)
    /// * [foot terminal](https://codeberg.org/dnkl/foot/issues/319)
    /// * [WezTerm terminal](https://wezfurlong.org/wezterm/config/lua/config/enable_kitty_keyboard.html)
    /// * [notcurses library](https://github.com/dankamongmen/notcurses/issues/2131)
    /// * [neovim text editor](https://github.com/neovim/neovim/pull/18181)
    /// * [kakoune text editor](https://github.com/mawww/kakoune/issues/4103)
    /// * [dte text editor](https://gitlab.com/craigbarnes/dte/-/issues/138)
    /// </remarks>
    public static ICommand PushKeyboardEnhancementFlags(KeyboardEnhancementFlags flags) =>
        new PushKeyboardEnhancementFlagsCommand(flags);

    /// <summary>
    /// A command that disables extra kinds of keyboard events.
    ///
    /// Specifically, it pops one level of keyboard enhancement flags.
    ///
    /// See [`PushKeyboardEnhancementFlags`] and <https://sw.kovidgoyal.net/kitty/keyboard-protocol/> for more information
    /// </summary>
    public static ICommand PopKeyboardEnhancementFlags { get; } = new PopKeyboardEnhancementFlagsCommand();

    /// <summary>
    /// A command that enables focus event emission.
    ///
    /// It should be paired with <see cref="DisableFocusChangeCommand"/> at the end of execution.
    ///
    /// Focus events can be captured with [read](./fn.read.html)/[poll](./fn.poll.html).
    /// </summary>
    public static ICommand EnableFocusChange { get; } = new EnableFocusChangeCommand();

    /// <summary>
    /// A command that disables focus event emission.
    /// </summary>
    public static ICommand DisableFocusChange { get; } = new DisableFocusChangeCommand();


    /// <summary>
    /// A command that enables [bracketed paste mode](https://en.wikipedia.org/wiki/Bracketed-paste).
    ///
    /// It should be paired with <see cref="DisableBracketedPasteCommand"/> at the end of execution.
    ///
    /// This is not supported in older Windows terminals without
    /// [virtual terminal sequences](https://docs.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences).
    /// </summary>
    public static ICommand EnableBracketedPaste { get; } = new EnableBracketedPasteCommand();

    /// <summary>
    /// A command that disables bracketed paste mode.
    /// </summary>
    public static ICommand DisableBracketedPaste { get; } = new DisableBracketedPasteCommand();
}