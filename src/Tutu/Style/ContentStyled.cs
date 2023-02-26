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
public record ContentStyled(
    Color? ForegroundColor,
    Color? BackgroundColor,
    Color? UnderlineColor,
    ImmutableList<Attribute> Attributes)
{
    /// <summary>
    /// The default instance.
    /// </summary>
    public static ContentStyled Default { get; } = new(null, null, null, ImmutableList<Attribute>.Empty);

    // TODO: Add support for 256 colors.

    public ContentStyled With(Color color) => this with { ForegroundColor = color };

    public ContentStyled On(Color color) => this with { BackgroundColor = color };

    public ContentStyled Underline(Color color) => this with { UnderlineColor = color };

    public ContentStyled Attribute(Attribute attribute) => this with { Attributes = Attributes.Add(attribute) };

    public ContentStyled Reset() => Attribute(Types.Attribute.Reset);

    public ContentStyled Bold() => Attribute(Types.Attribute.Bold);

    public ContentStyled Underline() => Attribute(Types.Attribute.Underlined);

    public ContentStyled Reverse() => Attribute(Types.Attribute.Reverse);

    public ContentStyled Dim() => Attribute(Types.Attribute.Dim);

    public ContentStyled Italic() => Attribute(Types.Attribute.Italic);

    public ContentStyled Negative() => Attribute(Types.Attribute.Reverse);

    public ContentStyled SlowBlink() => Attribute(Types.Attribute.SlowBlink);

    public ContentStyled RapidBlink() => Attribute(Types.Attribute.RapidBlink);

    public ContentStyled Hidden() => Attribute(Types.Attribute.Hidden);

    public ContentStyled CrossedOut() => Attribute(Types.Attribute.CrossedOut);

    public ContentStyled Black() => With(Color.Black);
    public ContentStyled OnBlack() => On(Color.Black);
    public ContentStyled UnderlineBlack() => Underline(Color.Black);

    public ContentStyled DarkGrey() => With(Color.DarkGrey);
    public ContentStyled OnDarkGrey() => On(Color.DarkGrey);
    public ContentStyled UnderlineDarkGrey() => Underline(Color.DarkGrey);

    public ContentStyled Red() => With(Color.Red);
    public ContentStyled OnRed() => On(Color.Red);
    public ContentStyled UnderlineRed() => Underline(Color.Red);

    public ContentStyled Green() => With(Color.Green);
    public ContentStyled OnGreen() => On(Color.Green);
    public ContentStyled UnderlineGreen() => Underline(Color.Green);

    public ContentStyled Yellow() => With(Color.Yellow);
    public ContentStyled OnYellow() => On(Color.Yellow);
    public ContentStyled UnderlineYellow() => Underline(Color.Yellow);

    public ContentStyled Blue() => With(Color.Blue);
    public ContentStyled OnBlue() => On(Color.Blue);
    public ContentStyled UnderlineBlue() => Underline(Color.Blue);

    public ContentStyled Magenta() => With(Color.Magenta);
    public ContentStyled OnMagenta() => On(Color.Magenta);
    public ContentStyled UnderlineMagenta() => Underline(Color.Magenta);

    public ContentStyled Cyan() => With(Color.Cyan);
    public ContentStyled OnCyan() => On(Color.Cyan);
    public ContentStyled UnderlineCyan() => Underline(Color.Cyan);

    public ContentStyled White() => With(Color.White);
    public ContentStyled OnWhite() => On(Color.White);
    public ContentStyled UnderlineWhite() => Underline(Color.White);

    public ContentStyled Grey() => With(Color.Grey);
    public ContentStyled OnGrey() => On(Color.Grey);
    public ContentStyled UnderlineGrey() => Underline(Color.Grey);
}