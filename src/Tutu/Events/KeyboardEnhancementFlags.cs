﻿namespace Tutu.Events;

/// <summary>
/// Represents special flags that tell compatible terminals to add extra information to keyboard events.
/// </summary>
/// <remarks>
/// See https://sw.kovidgoyal.net/kitty/keyboard-protocol/#progressive-enhancement for more information.
/// Alternate keys and Unicode codepoints are not yet supported by Tutu.
/// </remarks>
[Flags]
public enum KeyboardEnhancementFlags
{
    /// <summary>
    /// 
    /// </summary>
    None = 0,

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
    /// Send <see href="https://sw.kovidgoyal.net/kitty/keyboard-protocol/#key-codes">alternate keycodes</see>
    /// </summary>
    /// <remarks>
    /// in addition to the base keycode. The alternate keycode overrides the base keycode in
    /// resulting <see cref="KeyEvent"/>..
    /// </remarks>
    ReportAlternateKeys = 0b0000_0100,

    /// <summary>
    /// Represent all keyboard events as CSI-u sequences. This is required to get repeat/release
    /// events for plain-text keys.
    /// </summary>
    ReportAllKeysAsEscapeCodes = 0b0000_1000
}
