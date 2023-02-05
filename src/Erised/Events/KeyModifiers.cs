namespace Erised.Events;

/// <summary>
/// Represents key modifiers (shift, control, alt, etc.).
/// </summary>
[Flags]
public enum KeyModifiers : byte
{
    Shift = 0b0000_0001,
    Control = 0b0000_0010,
    Alt = 0b0000_0100,
    Super = 0b0000_1000,
    Hyper = 0b0001_0000,
    Meta = 0b0010_0000,
    None = 0b0000_0000
}
