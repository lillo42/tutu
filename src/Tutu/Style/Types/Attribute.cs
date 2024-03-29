﻿namespace Tutu.Style.Types;

/// <summary>
/// Represents an attribute.
/// </summary>
/// <param name="Value">The value.</param>
/// <param name="Name">The name value.</param>
/// <remarks>
/// <para><strong>Platform-specific Notes</strong></para>
/// <para>Only UNIX and Windows 10 terminals do support text attributes.</para>
/// <para>Keep in mind that not all terminals support all attributes.</para>
/// <para>Tutu implements almost all attributes listed in the</para>
/// <list type="table">
///     <listheader>
///         <term>Attribute</term>
///         <term>Windows</term>
///         <term>UNIX</term>
///         <term>Notes</term>
///     </listheader>
///     <item>
///         <term>Reset</term>
///         <term>✓</term>
///         <term>✓</term>
///         <term></term>
///     </item>
///     <item>
///         <term>Bold</term>
///         <term>✓</term>
///         <term>✓</term>
///         <term></term>
///     </item>
///     <item>
///         <term>Dim</term>
///         <term>✓</term>
///         <term>✓</term>
///         <term></term>
///     </item>
///     <item>
///         <term>Italic</term>
///         <term>?</term>
///         <term>?</term>
///         <term>Not widely supported, sometimes treated as inverse.</term>
///     </item>
///    <item>
///         <term>Underlined</term>
///         <term>✓</term>
///         <term>✓</term>
///         <term></term>
///     </item>
///     <item>
///         <term>SlowBlink</term>
///         <term>?</term>
///         <term>?</term>
///         <term>Not widely supported, sometimes treated as inverse.</term>
///     </item>
///     <item>
///         <term>RapidBlink</term>
///         <term>?</term>
///         <term>?</term>
///         <term>Not widely supported. MS-DOS ANSI.SYS; 150+ per minute.</term>
///     </item>
///     <item>
///         <term>Reverse</term>
///         <term>✓</term>
///         <term>✓</term>
///         <term></term>
///     </item>
///     <item>
///         <term>Hidden</term>
///         <term>✓</term>
///         <term>✓</term>
///         <term>Also known as Conceal.</term>
///     </item>
///     <item>
///         <term>Fraktur</term>
///         <term>✗</term>
///         <term>✓</term>
///         <term>Legible characters, but marked for deletion.</term>
///     </item>
///     <item>
///         <term>DefaultForegroundColor</term>
///         <term>?</term>
///         <term>?</term>
///         <term>Implementation specific (according to standard).</term>
///     </item>
///     <item>
///         <term>DefaultBackgroundColor</term>
///         <term>?</term>
///         <term>?</term>
///         <term>Implementation specific (according to standard).</term>
///     </item>
///     <item>
///         <term>Framed</term>
///         <term>?</term>
///         <term>?</term>
///         <term>Not widely supported.</term>
///     </item>
///     <item>
///         <term>Encircled</term>
///         <term>?</term>
///         <term>?</term>
///         <term>This should turn on the encircled attribute.</term>
///     </item>
///     <item>
///         <term>Overlined</term>
///         <term>?</term>
///         <term>?</term>
///         <term>Not widely supported.</term>
///     </item>
/// </list>
/// </remarks>
public readonly record struct Attribute(string Name, string Value)
{
    internal static IEnumerable<Attribute> AllAttributes
    {
        get
        {
            yield return Reset;
            yield return Bold;
            yield return Dim;
            yield return Italic;
            yield return Underlined;
            yield return DoubleUnderlined;
            yield return Undercurled;
            yield return Underdotted;
            yield return Underdashed;
            yield return SlowBlink;
            yield return RapidBlink;
            yield return Reverse;
            yield return CrossedOut;
            yield return PrimaryFont;
            yield return Fraktur;
            yield return NoBold;
            yield return NormalIntensity;
            yield return NoItalic;
            yield return NoUnderline;
            yield return NoBlink;
            yield return NoReverse;
            yield return Reveal;
            yield return NotCrossedOut;
            yield return Frame;
            yield return Encircled;
            yield return OverLined;
            yield return NotFramedOrEncircled;
            yield return NotOverLined;
        }
    }

    /// <summary>
    /// Resets all the attributes.
    /// </summary>
    /// <remarks>
    /// All attributes off
    /// </remarks>
    public static Attribute Reset { get; } = new("Reset", "0");

    /// <summary>
    /// Increases the text intensity.
    /// </summary>
    /// <remarks>
    /// As with faint, the color change is a PC (SCO / CGA) invention.
    /// </remarks>
    public static Attribute Bold { get; } = new("Bold", "1");

    /// <summary>
    /// Decreases the text intensity.
    /// </summary>
    /// <remarks>
    /// May be implemented as a light font weight like bold.
    /// </remarks>
    public static Attribute Dim { get; } = new("Dim", "2");

    /// <summary>
    /// Emphasises the text.
    /// </summary>
    /// <remarks>
    /// Not widely supported. Sometimes treated as inverse or blink.
    /// </remarks>
    public static Attribute Italic { get; } = new("Italic", "3");

    /// <summary>
    /// Underlines the text.
    /// </summary>
    /// <remarks>
    /// Style extensions exist for Kitty, VTE, mintty and iTerm2.
    /// </remarks>
    public static Attribute Underlined { get; } = new("Underlined", "4");

    /// <summary>
    /// Other types of underlining
    /// Double underlines the text.
    /// </summary>
    public static Attribute DoubleUnderlined { get; } = new("DoubledUnderlined", "4:2");

    /// <summary>
    /// Undercurls the text.
    /// </summary>
    public static Attribute Undercurled { get; } = new("Undercurled", "4:3");

    /// <summary>
    /// Undercurls the text.
    /// </summary>
    public static Attribute Underdotted { get; } = new("Underdotted", "4:4");

    /// <summary>
    /// Underdashed the text.
    /// </summary>
    public static Attribute Underdashed { get; } = new("Underdashed", "4:5");

    /// <summary>
    /// Makes the text blinking ( less 150 per minute).
    /// </summary>
    /// <remarks>
    /// Sets blinking to less than 150 times per minute.
    /// </remarks>
    public static Attribute SlowBlink { get; } = new("SlowBlink", "5");

    /// <summary>
    /// Makes the text blinking (>= 150 per minute).
    /// </summary>
    /// <remarks>
    /// MS-DOS ANSI.SYS, 150+ per minute; not widely supported.
    /// </remarks>
    public static Attribute RapidBlink { get; } = new("RapidBlink", "6");

    /// <summary>
    /// Swaps foreground and background colors.
    /// </summary>
    /// <remarks>
    /// Swap foreground and background colors; inconsistent emulation.
    /// </remarks>
    public static Attribute Reverse { get; } = new("Reverse", "7");

    /// <summary>
    /// Hides the text (also known as Conceal).
    /// </summary>
    /// <remarks>
    /// Not widely supported.
    /// </remarks>
    public static Attribute Hidden { get; } = new("Hidden", "8");

    /// <summary>
    /// Crosses the text.
    /// </summary>
    /// <remarks>
    /// Characters legible but marked as if for deletion. Not supported in Terminal.app.
    /// </remarks>
    public static Attribute CrossedOut { get; } = new("CrossedOut", "9");

    /// <summary>
    /// Primary (default) font.
    /// </summary>
    public static Attribute PrimaryFont { get; } = new("PrimaryFont", "10");

    /// <summary>
    /// Select alternative font n − 10
    /// </summary>
    /// <param name="fontNumber">The font number</param>
    /// <returns></returns>
    public static Attribute AlternativeFont(int fontNumber) => new("AlternativeFont", (10 + fontNumber).ToString());

    /// <summary>
    /// Sets the <see href="https://en.wikipedia.org/wiki/Fraktur">Fraktur</see> typeface.
    /// </summary>
    /// <remarks>
    /// <para>Mostly used for <see href="https://en.wikipedia.org/wiki/Mathematical_Alphanumeric_Symbols">mathematical alphanumeric symbols</see>.</para>
    /// <para>Rarely supported.</para>
    /// </remarks>
    public static Attribute Fraktur { get; } = new("Fraktur", "20");

    /// <summary>
    /// Turns off the <see cref="Bold"/> attribute. - Inconsistent - Prefer to use <see cref="NormalIntensity"/>. 
    /// </summary>
    /// <remarks>
    /// Double-underline per ECMA-48 but instead disables bold intensity on several terminals, including in the
    /// Linux kernel's console before version 4.17
    /// </remarks>
    public static Attribute NoBold { get; } = new("NoBold", "21");

    /// <summary>
    /// Switches the text back to normal intensity (<see cref="NoBold"/>, <see cref="Italic"/>).
    /// </summary>
    /// <remarks>
    /// Neither bold nor faint; color changes where intensity is implemented as such.
    /// </remarks>
    public static Attribute NormalIntensity { get; } = new("NormalIntensity", "22");

    /// <summary>
    /// Turns off the <see cref="Italic"/> attribute.
    /// </summary>
    public static Attribute NoItalic { get; } = new("NoItalic", "23");

    /// <summary>
    /// Turns off the <see cref="Underlined"/> attribute.
    /// </summary>
    /// <remarks>
    /// Neither singly nor doubly underlined.
    /// </remarks>
    public static Attribute NoUnderline { get; } = new("NoUnderline", "24");

    /// <summary>
    /// Turns off the text blinking (<see cref="SlowBlink"/> or <see cref="RapidBlink"/>).
    /// </summary>
    public static Attribute NoBlink { get; } = new("NoBlink", "25");

    /// <summary>
    /// Turns off the <see cref="Reverse"/> attribute.
    /// </summary>
    public static Attribute NoReverse { get; } = new("NoReverse", "27");

    /// <summary>
    /// Turns off the <see cref="Hidden"/> attribute.
    /// </summary>
    /// <remarks>
    /// Not concealed.
    /// </remarks>
    public static Attribute Reveal { get; } = new("NoHidden", "28");

    /// <summary>
    /// Turns off the <see cref="CrossedOut"/> attribute.
    /// </summary>
    public static Attribute NotCrossedOut { get; } = new("NotCrossedOut", "29");

    /// <summary>
    /// Makes the text framed.
    /// </summary>
    /// <remarks>
    /// Implemented as "emoji variation selector" in mintty.
    /// </remarks>
    public static Attribute Frame { get; } = new("Framed", "51");

    /// <summary>
    /// Makes the text encircled.
    /// </summary>
    /// <remarks>
    /// Implemented as "emoji variation selector" in mintty.
    /// </remarks>
    public static Attribute Encircled { get; } = new("Encircled", "52");

    /// <summary>
    /// Draws a line at the top of the text.
    /// </summary>
    /// <remarks>
    /// Not supported in Terminal.app
    /// </remarks>
    public static Attribute OverLined { get; } = new("OverLined", "53");

    /// <summary>
    /// Turns off the <see cref="Frame"/> and <see cref="Encircled"/> attributes.
    /// </summary>
    public static Attribute NotFramedOrEncircled { get; } = new("NotFramedOrEncircled", "54");

    /// <summary>
    /// Turns off the <see cref="OverLined"/> attribute.
    /// </summary>
    public static Attribute NotOverLined { get; } = new("NotOverLined", "55");

    /// <summary>
    /// The SGR attribute value.
    /// </summary>
    /// <remarks>
    /// See https://en.wikipedia.org/wiki/ANSI_escape_code#SGR_parameters
    /// </remarks>
    public string Sgr => Value;
}
