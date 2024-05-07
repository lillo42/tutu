using System.Collections.Immutable;
using Tutu.Style.Types;
using Attribute = Tutu.Style.Types.Attribute;

namespace Tutu.Style;

/// <summary>
/// The content styled.
/// </summary>
/// <param name="ForegroundColor">The foreground <see cref="Color"/>.</param>
/// <param name="BackgroundColor">The background <see cref="Color"/>.</param>
/// <param name="UnderlineColor">The underline <see cref="Color"/>.</param>
/// <param name="Attributes">The <see cref="Attribute"/> collection.</param>
public sealed record ContentStyled(
    Color? ForegroundColor,
    Color? BackgroundColor,
    Color? UnderlineColor,
    ImmutableList<Attribute> Attributes)
{
    /// <summary>
    /// The default instance.
    /// </summary>
    public static ContentStyled Default { get; } = new(null, null, null, ImmutableList<Attribute>.Empty);

    /// <summary>
    /// Change foreground color.
    /// </summary>
    /// <param name="color">The <see cref="Color"/>.</param>
    /// <returns>New instance of <see cref="ContentStyled"/> with the new <see cref="ForegroundColor"/>.</returns>
    public ContentStyled With(Color color) => this with { ForegroundColor = color };

    /// <summary>
    /// Change background color.
    /// </summary>
    /// <param name="color">The <see cref="Color"/>.</param>
    /// <returns>New instance of <see cref="ContentStyled"/> with the new <see cref="BackgroundColor"/>.</returns>
    public ContentStyled On(Color color) => this with { BackgroundColor = color };

    /// <summary>
    /// Change underline color.
    /// </summary>
    /// <param name="color">The <see cref="Color"/>.</param>
    /// <returns>New instance of <see cref="ContentStyled"/> with the new <see cref="UnderlineColor"/>.</returns>
    public ContentStyled Underline(Color color) => this with { UnderlineColor = color };

    /// <summary>
    /// Add an attribute.
    /// </summary>
    /// <param name="attribute">The <see cref="Attribute"/>.</param>
    /// <returns>New instance of <see cref="ContentStyled"/> with the new <see cref="Attributes"/>.</returns>
    public ContentStyled Attribute(Attribute attribute) => this with { Attributes = Attributes.Add(attribute) };

    /// <summary>
    /// Create a new instance of <see cref="StyledContent{T}"/> with current <see cref="ContentStyled"/>.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>new instance of <see cref="StyledContent{T}"/> with current <see cref="ContentStyled"/>.</returns>
    public StyledContent<T> Apply<T>(T content) where T : notnull => new(this, content);

    /// <summary>
    /// Reset all attributes.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Types.Attribute.Reset"/> attribute.</returns>
    public ContentStyled Reset() => Attribute(Types.Attribute.Reset);

    /// <summary>
    /// Mark as bold.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Types.Attribute.Bold"/> attribute.</returns>
    public ContentStyled Bold() => Attribute(Types.Attribute.Bold);

    /// <summary>
    /// Mark as underlined.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Types.Attribute.Underlined"/> attribute.</returns>
    public ContentStyled Underline() => Attribute(Types.Attribute.Underlined);

    /// <summary>
    /// Reverse foreground and background colors.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Types.Attribute.Reverse"/> attribute.</returns>
    public ContentStyled Reverse() => Attribute(Types.Attribute.Reverse);

    /// <summary>
    /// Mark as dim.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Types.Attribute.Dim"/> attribute.</returns>
    public ContentStyled Dim() => Attribute(Types.Attribute.Dim);

    /// <summary>
    /// Mark as italic.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Types.Attribute.Italic"/> attribute.</returns>
    public ContentStyled Italic() => Attribute(Types.Attribute.Italic);

    /// <summary>
    /// Mark as negative.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Types.Attribute.Reverse"/> attribute.</returns>
    public ContentStyled Negative() => Attribute(Types.Attribute.Reverse);

    /// <summary>
    /// Mark as slow blink.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Types.Attribute.SlowBlink"/> attribute.</returns>
    public ContentStyled SlowBlink() => Attribute(Types.Attribute.SlowBlink);

    /// <summary>
    /// Mark as rapid blink.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Types.Attribute.RapidBlink"/> attribute.</returns>
    public ContentStyled RapidBlink() => Attribute(Types.Attribute.RapidBlink);

    /// <summary>
    /// Mark as hidden.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Types.Attribute.Hidden"/> attribute.</returns>
    public ContentStyled Hidden() => Attribute(Types.Attribute.Hidden);

    /// <summary>
    /// Mark as crossed out. 
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Types.Attribute.CrossedOut"/> attribute.</returns>
    public ContentStyled CrossedOut() => Attribute(Types.Attribute.CrossedOut);

    /// <summary>
    /// Change foreground color to black.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.Black"/> <see cref="ForegroundColor"/>.</returns>
    public ContentStyled Black() => With(Color.Black);

    /// <summary>
    /// Change background color to black.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.Black"/> <see cref="BackgroundColor"/>.</returns>
    public ContentStyled OnBlack() => On(Color.Black);

    /// <summary>
    /// Change underline color to black.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.Black"/> <see cref="UnderlineColor"/>.</returns>
    public ContentStyled UnderlineBlack() => Underline(Color.Black);

    /// <summary>
    /// Change foreground color to dark grey.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.DarkGrey"/> <see cref="ForegroundColor"/>.</returns>
    public ContentStyled DarkGrey() => With(Color.DarkGrey);

    /// <summary>
    /// Change background color to dark grey.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.DarkGrey"/> <see cref="BackgroundColor"/>.</returns>
    public ContentStyled OnDarkGrey() => On(Color.DarkGrey);

    /// <summary>
    /// Change underline color to dark grey.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.DarkGrey"/> <see cref="UnderlineColor"/>.</returns>
    public ContentStyled UnderlineDarkGrey() => Underline(Color.DarkGrey);

    /// <summary>
    /// Change foreground color to red.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.Red"/> <see cref="ForegroundColor"/>.</returns>
    public ContentStyled Red() => With(Color.Red);

    /// <summary>
    /// Change background color to red.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.Red"/> <see cref="BackgroundColor"/>.</returns>
    public ContentStyled OnRed() => On(Color.Red);

    /// <summary>
    /// Change underline color to red.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.Red"/> <see cref="UnderlineColor"/>.</returns>
    public ContentStyled UnderlineRed() => Underline(Color.Red);

    /// <summary>
    /// Change foreground color to green.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.Green"/> <see cref="ForegroundColor"/>.</returns>
    public ContentStyled Green() => With(Color.Green);

    /// <summary>
    /// Change background color to green.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.Green"/> <see cref="BackgroundColor"/>.</returns>
    public ContentStyled OnGreen() => On(Color.Green);

    /// <summary>
    /// Change underline color to green.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.Green"/> <see cref="UnderlineColor"/>.</returns>
    public ContentStyled UnderlineGreen() => Underline(Color.Green);

    /// <summary>
    /// Change foreground color to yellow.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.Yellow"/> <see cref="ForegroundColor"/>.</returns>
    public ContentStyled Yellow() => With(Color.Yellow);

    /// <summary>
    /// Change background color to yellow.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.Yellow"/> <see cref="BackgroundColor"/>.</returns>
    public ContentStyled OnYellow() => On(Color.Yellow);

    /// <summary>
    /// Change underline color to yellow.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.Yellow"/> <see cref="UnderlineColor"/>.</returns>
    public ContentStyled UnderlineYellow() => Underline(Color.Yellow);

    /// <summary>
    /// Change foreground color to blue.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.Blue"/> <see cref="ForegroundColor"/>.</returns>
    public ContentStyled Blue() => With(Color.Blue);

    /// <summary>
    /// Change background color to blue.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.Blue"/> <see cref="BackgroundColor"/>.</returns>
    public ContentStyled OnBlue() => On(Color.Blue);

    /// <summary>
    /// Change underline color to blue.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.Blue"/> <see cref="UnderlineColor"/>.</returns>
    public ContentStyled UnderlineBlue() => Underline(Color.Blue);

    /// <summary>
    /// Change foreground color to magenta.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.Magenta"/> <see cref="ForegroundColor"/>.</returns>
    public ContentStyled Magenta() => With(Color.Magenta);

    /// <summary>
    /// Change background color to magenta.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.Magenta"/> <see cref="BackgroundColor"/>.</returns>
    public ContentStyled OnMagenta() => On(Color.Magenta);

    /// <summary>
    /// Change underline color to magenta.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.Magenta"/> <see cref="UnderlineColor"/>.</returns>
    public ContentStyled UnderlineMagenta() => Underline(Color.Magenta);

    /// <summary>
    /// Change foreground color to cyan.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.Cyan"/> <see cref="ForegroundColor"/>.</returns>
    public ContentStyled Cyan() => With(Color.Cyan);

    /// <summary>
    /// Change background color to cyan.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.Cyan"/> <see cref="BackgroundColor"/>.</returns>
    public ContentStyled OnCyan() => On(Color.Cyan);

    /// <summary>
    /// Change underline color to cyan.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.Cyan"/> <see cref="UnderlineColor"/>.</returns>
    public ContentStyled UnderlineCyan() => Underline(Color.Cyan);

    /// <summary>
    /// Change foreground color to white.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.White"/> <see cref="ForegroundColor"/>.</returns>
    public ContentStyled White() => With(Color.White);

    /// <summary>
    /// Change background color to white.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.White"/> <see cref="BackgroundColor"/>.</returns>
    public ContentStyled OnWhite() => On(Color.White);

    /// <summary>
    /// Change underline color to white.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.White"/> <see cref="UnderlineColor"/>.</returns>
    public ContentStyled UnderlineWhite() => Underline(Color.White);

    /// <summary>
    /// Change foreground color to grey.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.Grey"/> <see cref="ForegroundColor"/>.</returns>
    public ContentStyled Grey() => With(Color.Grey);

    /// <summary>
    /// Change background color to grey.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.Grey"/> <see cref="BackgroundColor"/>.</returns>
    public ContentStyled OnGrey() => On(Color.Grey);

    /// <summary>
    /// Change underline color to grey.
    /// </summary>
    /// <returns>New instance of <see cref="ContentStyled"/> with <see cref="Color.Grey"/> <see cref="UnderlineColor"/>.</returns>
    public ContentStyled UnderlineGrey() => Underline(Color.Grey);
}
