using Erised.Style.Types;

namespace Erised.Style;

public record StyledContent<T>(ContentStyled Style, T Content)
    where T : notnull
{
    public StyledContent<T> With(Color color) => this with { Style = Style.With(color) };

    public StyledContent<T> On(Color color) => this with { Style = Style.On(color) };

    public StyledContent<T> Underline(Color color) => this with { Style = Style.Underline(color) };

    public StyledContent<T> Attribute(Types.Attribute attribute) => this with { Style = Style.Attribute(attribute) };

    public StyledContent<T> Reset() => Attribute(Types.Attribute.Reset);

    public StyledContent<T> Bold() => Attribute(Types.Attribute.Bold);

    public StyledContent<T> Underline() => Attribute(Types.Attribute.Underlined);

    public StyledContent<T> Reverse() => Attribute(Types.Attribute.Reverse);

    public StyledContent<T> Dim() => Attribute(Types.Attribute.Dim);

    public StyledContent<T> Italic() => Attribute(Types.Attribute.Italic);

    public StyledContent<T> Negative() => Attribute(Types.Attribute.Reverse);

    public StyledContent<T> SlowBlink() => Attribute(Types.Attribute.SlowBlink);

    public StyledContent<T> RapidBlink() => Attribute(Types.Attribute.RapidBlink);

    public StyledContent<T> Hidden() => Attribute(Types.Attribute.Hidden);

    public StyledContent<T> CrossedOut() => Attribute(Types.Attribute.CrossedOut);

    public StyledContent<T> Black() => With(Color.Black);
    public StyledContent<T> OnBlack() => On(Color.Black);
    public StyledContent<T> UnderlineBlack() => Underline(Color.Black);

    public StyledContent<T> DarkGrey() => With(Color.DarkGrey);
    public StyledContent<T> OnDarkGrey() => On(Color.DarkGrey);
    public StyledContent<T> UnderlineDarkGrey() => Underline(Color.DarkGrey);

    public StyledContent<T> Red() => With(Color.Red);
    public StyledContent<T> OnRed() => On(Color.Red);
    public StyledContent<T> UnderlineRed() => Underline(Color.Red);

    public StyledContent<T> Green() => With(Color.Green);
    public StyledContent<T> OnGreen() => On(Color.Green);
    public StyledContent<T> UnderlineGreen() => Underline(Color.Green);

    public StyledContent<T> Yellow() => With(Color.Yellow);
    public StyledContent<T> OnYellow() => On(Color.Yellow);
    public StyledContent<T> UnderlineYellow() => Underline(Color.Yellow);

    public StyledContent<T> Blue() => With(Color.Blue);
    public StyledContent<T> OnBlue() => On(Color.Blue);
    public StyledContent<T> UnderlineBlue() => Underline(Color.Blue);

    public StyledContent<T> Magenta() => With(Color.Magenta);
    public StyledContent<T> OnMagenta() => On(Color.Magenta);
    public StyledContent<T> UnderlineMagenta() => Underline(Color.Magenta);

    public StyledContent<T> Cyan() => With(Color.Cyan);
    public StyledContent<T> OnCyan() => On(Color.Cyan);
    public StyledContent<T> UnderlineCyan() => Underline(Color.Cyan);

    public StyledContent<T> White() => With(Color.White);
    public StyledContent<T> OnWhite() => On(Color.White);
    public StyledContent<T> UnderlineWhite() => Underline(Color.White);

    public StyledContent<T> Grey() => With(Color.Grey);
    public StyledContent<T> OnGrey() => On(Color.Grey);
    public StyledContent<T> UnderlineGrey() => Underline(Color.Grey);
}