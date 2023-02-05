namespace Erised.Events;

/// <summary>
/// Represents special flags that tell compatible terminals to add extra information to keyboard events.
///
/// See https://sw.kovidgoyal.net/kitty/keyboard-protocol/#progressive-enhancement for more information.
/// Alternate keys and Unicode codepoints are not yet supported by crossterm.
/// </summary>
public enum KeyboardEnhancementFlags
{
    /// <summary>
    /// Represent Escape and modified keys using CSI-u sequences, so they can be unambiguously
    /// read.
    /// </summary>
    DisambiguateEscapeCodes = 0b0000_0001,

    /// <summary>
    /// Add extra events with <see cref="KeyEventKind" /> set to <see cref="KeyEventKind.Repeat"/> or
    /// <see cref="KeyEventKind.Release"/> when keys are autorepeated or released.
    /// </summary>
    ReportEventTypes = 0b0000_0010,

    /// <summary>
    /// Represent all keyboard events as CSI-u sequences. This is required to get repeat/release
    /// events for plain-text keys.
    /// </summary>
    ReportAllKeysAsEscapeCodes = 0b0000_1000
}