﻿namespace Tutu.Events;

/// <summary>
/// Represents extra state about the key event.
/// </summary>
[Flags]
public enum KeyEventState : byte
{
    /// <summary>
    /// The key event origins from the keypad.
    /// </summary>
    Keypad = 0b0000_0001,

    /// <summary>
    /// Caps Lock was enabled for this key event.
    /// </summary>
    /// <remarks>
    /// This is set for the initial press of Caps Lock itself.
    /// </remarks>
    CapsLock = 0b0000_1000,

    /// <summary>
    /// Num Lock was enabled for this key event.
    /// </summary>
    /// <remarks>
    /// This is set for the initial press of Num Lock itself.
    /// </remarks>
    NumLock = 0b0000_1000,

    /// <summary>
    /// None
    /// </summary>
    None = 0b0000_0000
}
