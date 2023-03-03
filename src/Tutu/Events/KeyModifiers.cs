namespace Tutu.Events;

/// <summary>
/// Represents key modifiers (shift, control, alt, etc.).
/// </summary>
[Flags]
public enum KeyModifiers : byte
{
    /// <summary>
    /// Shift key.
    /// </summary>
    Shift = 0b0000_0001,

    /// <summary>
    /// The control key.
    /// </summary>
    Control = 0b0000_0010,

    /// <summary>
    /// The alt key.
    /// </summary>
    Alt = 0b0000_0100,

    /// <summary>
    /// The super key.
    /// </summary>
    Super = 0b0000_1000,

    /// <summary>
    /// The hyper key.
    /// </summary>
    Hyper = 0b0001_0000,

    /// <summary>
    /// The meta key.
    /// </summary>
    Meta = 0b0010_0000,

    /// <summary>
    /// No modifiers.
    /// </summary>
    None = 0b0000_0000
}
