using Tutu.Events;
using Tutu.Events.Commands;

namespace Tutu.Commands;

/// <summary>
/// The events commands.
/// </summary>
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
    /// A command that enables the <see href="https://sw.kovidgoyal.net/kitty/keyboard-protocol/">kitty keyboard protocol</see>,
    /// which adds extra information to keyboard events and removes ambiguity for modifier keys.
    /// </summary>
    /// <param name="flags">The <see cref="KeyboardEnhancementFlags"/></param>
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
    public static ICommand PushKeyboardEnhancementFlags(KeyboardEnhancementFlags flags) =>
        new PushKeyboardEnhancementFlagsCommand(flags);

    /// <summary>
    /// A command that disables extra kinds of keyboard events.
    /// </summary>
    /// <remarks>
    /// Specifically, it pops one level of keyboard enhancement flags.
    /// See <see cref="PushKeyboardEnhancementFlags"/> and https://sw.kovidgoyal.net/kitty/keyboard-protocol/ for more information
    /// </remarks>
    public static ICommand PopKeyboardEnhancementFlags { get; } = new PopKeyboardEnhancementFlagsCommand();

    /// <summary>
    /// A command that enables focus event emission.
    /// </summary>
    /// <remarks>
    /// It should be paired with <see cref="DisableFocusChangeCommand"/> at the end of execution.
    /// Focus events can be captured with <see cref="EventReader.Read"/>.
    /// </remarks>
    public static ICommand EnableFocusChange { get; } = new EnableFocusChangeCommand();

    /// <summary>
    /// A command that disables focus event emission.
    /// </summary>
    public static ICommand DisableFocusChange { get; } = new DisableFocusChangeCommand();


    /// <summary>
    /// A command that enables <see href="https://en.wikipedia.org/wiki/Bracketed-paste">bracketed paste mode</see>.
    /// </summary>
    /// <remarks>
    /// It should be paired with <see cref="DisableBracketedPasteCommand"/> at the end of execution.
    ///
    /// This is not supported in older Windows terminals without
    /// <see href="https://docs.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences">virtual terminal sequences</see>.
    /// </remarks>
    public static ICommand EnableBracketedPaste { get; } = new EnableBracketedPasteCommand();

    /// <summary>
    /// A command that disables bracketed paste mode.
    /// </summary>
    public static ICommand DisableBracketedPaste { get; } = new DisableBracketedPasteCommand();
}
