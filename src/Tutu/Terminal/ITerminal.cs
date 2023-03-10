using Tutu.Events;

namespace Tutu.Terminal;

/// <summary>
/// The terminal. 
/// </summary>
public interface ITerminal
{
    /// <summary>
    /// Tells whether the raw mode is enabled.
    /// </summary>
    bool IsRawModeEnabled { get; }

    /// <summary>
    /// Enable raw mode.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    void EnableRawMode();

    /// <summary>
    /// Disables raw mode.
    /// </summary>
    /// <remarks>
    /// On unix system, More precisely, reset the whole termios mode to what it was before the first call
    /// to <see cref="EnableRawMode"/>. If you don't mess with termios outside of Tutu, it's
    /// effectively disabling the raw mode and doing nothing else.
    /// </remarks>
    void DisableRawMode();

    /// <summary>
    /// Current terminal size.
    /// </summary>
    TerminalSize Size { get; }

    /// <summary>
    /// Queries the terminal's support for progressive keyboard enhancement.
    /// </summary>
    /// <remarks>
    /// This always returns `false` on Windows.
    ///
    /// On unix systems, this function will block and possibly time out while
    /// <see cref="EventReader.Read"/> or <see cref="EventReader.Poll(System.Nullable{NodaTime.Duration})"/> are being called.
    /// </remarks>
    bool SupportsKeyboardEnhancement { get; }
}
