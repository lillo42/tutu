using Tutu.Style.Types;

namespace Tutu.Style;

/// <summary>
/// The styled content.
/// </summary>
/// <param name="Style">The <see cref="ContentStyled"/>.</param>
/// <param name="Content">The content.</param>
/// <typeparam name="T">Th content type.</typeparam>
public sealed record StyledContent<T>(ContentStyled Style, T Content)
    where T : notnull
{
    /// <summary>
    /// Change foreground color.
    /// </summary>
    /// <param name="color">The <see cref="Color"/>.</param>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with the new foreground color.</returns>
    public StyledContent<T> With(Color color) => this with { Style = Style.With(color) };

    /// <summary>
    /// Change background color.
    /// </summary>
    /// <param name="color">The <see cref="Color"/>.</param>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with the new background color.</returns>
    public StyledContent<T> On(Color color) => this with { Style = Style.On(color) };

    /// <summary>
    /// Change underline color.
    /// </summary>
    /// <param name="color">The <see cref="Color"/>.</param>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with the new underline color.</returns>
    public StyledContent<T> Underline(Color color) => this with { Style = Style.Underline(color) };

    /// <summary>
    /// Add an attribute.
    /// </summary>
    /// <param name="attribute">The <see cref="Attribute"/>.</param>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with the new attributes.</returns>
    public StyledContent<T> Attribute(Types.Attribute attribute) => this with { Style = Style.Attribute(attribute) };

    /// <summary>
    /// Reset all attributes.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Types.Attribute.Reset"/> attribute.</returns>
    public StyledContent<T> Reset() => Attribute(Types.Attribute.Reset);

    /// <summary>
    /// Mark as bold.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Types.Attribute.Bold"/> attribute.</returns>
    public StyledContent<T> Bold() => Attribute(Types.Attribute.Bold);

    /// <summary>
    /// Mark as underline.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Types.Attribute.Reset"/> attribute.</returns>
    public StyledContent<T> Underline() => Attribute(Types.Attribute.Underlined);

    /// <summary>
    /// Mark as reverse.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Types.Attribute.Reverse"/> attribute.</returns>
    public StyledContent<T> Reverse() => Attribute(Types.Attribute.Reverse);

    /// <summary>
    /// Mark as dim.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Types.Attribute.Dim"/> attribute.</returns>
    public StyledContent<T> Dim() => Attribute(Types.Attribute.Dim);

    /// <summary>
    /// Mark as italic.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Types.Attribute.Italic"/> attribute.</returns>
    public StyledContent<T> Italic() => Attribute(Types.Attribute.Italic);

    /// <summary>
    /// Mark as negative.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Types.Attribute.Reverse"/> attribute.</returns>
    public StyledContent<T> Negative() => Attribute(Types.Attribute.Reverse);

    /// <summary>
    /// Mark as slow blink.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Types.Attribute.SlowBlink"/> attribute.</returns>
    public StyledContent<T> SlowBlink() => Attribute(Types.Attribute.SlowBlink);

    /// <summary>
    /// Mark as rapid blink.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Types.Attribute.RapidBlink"/> attribute.</returns>
    public StyledContent<T> RapidBlink() => Attribute(Types.Attribute.RapidBlink);

    /// <summary>
    /// Mark as hidden.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Types.Attribute.Hidden"/> attribute.</returns>
    public StyledContent<T> Hidden() => Attribute(Types.Attribute.Hidden);

    /// <summary>
    /// Mark as crossed out.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Types.Attribute.CrossedOut"/> attribute.</returns>
    public StyledContent<T> CrossedOut() => Attribute(Types.Attribute.CrossedOut);

    /// <summary>
    /// Change foreground color to black.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.Black"/> foreground.</returns>
    public StyledContent<T> Black() => With(Color.Black);

    /// <summary>
    /// Change background color to black.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.Black"/> background.</returns>
    public StyledContent<T> OnBlack() => On(Color.Black);

    /// <summary>
    /// Change underline color to black.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.Black"/> underline.</returns>
    public StyledContent<T> UnderlineBlack() => Underline(Color.Black);

    /// <summary>
    /// Change foreground color to dark grey.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.DarkGrey"/> foreground.</returns>
    public StyledContent<T> DarkGrey() => With(Color.DarkGrey);

    /// <summary>
    /// Change background color to dark grey.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.DarkGrey"/> background.</returns>
    public StyledContent<T> OnDarkGrey() => On(Color.DarkGrey);

    /// <summary>
    /// Change underline color to dark grey.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.DarkGrey"/> underline.</returns>
    public StyledContent<T> UnderlineDarkGrey() => Underline(Color.DarkGrey);

    /// <summary>
    /// Change foreground color to red.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.Red"/> foreground.</returns>
    public StyledContent<T> Red() => With(Color.Red);

    /// <summary>
    /// Change background color to red.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.Red"/> background.</returns>
    public StyledContent<T> OnRed() => On(Color.Red);

    /// <summary>
    /// Change underline color to red.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.Red"/> underline.</returns>
    public StyledContent<T> UnderlineRed() => Underline(Color.Red);

    /// <summary>
    /// Change foreground color to green.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.Green"/> foreground.</returns>
    public StyledContent<T> Green() => With(Color.Green);

    /// <summary>
    /// Change background color to green.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.Green"/> background.</returns>
    public StyledContent<T> OnGreen() => On(Color.Green);

    /// <summary>
    /// Change underline color to green.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.Green"/> underline.</returns>
    public StyledContent<T> UnderlineGreen() => Underline(Color.Green);

    /// <summary>
    /// Change foreground color to yellow.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.Yellow"/> foreground.</returns>
    public StyledContent<T> Yellow() => With(Color.Yellow);

    /// <summary>
    /// Change background color to yellow.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.Yellow"/> background.</returns>
    public StyledContent<T> OnYellow() => On(Color.Yellow);

    /// <summary>
    /// Change underline color to yellow.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.Yellow"/> underline.</returns>
    public StyledContent<T> UnderlineYellow() => Underline(Color.Yellow);

    /// <summary>
    /// Change foreground color to blue.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.Blue"/> foreground.</returns>
    public StyledContent<T> Blue() => With(Color.Blue);

    /// <summary>
    /// Change background color to blue.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.Blue"/> background.</returns>
    public StyledContent<T> OnBlue() => On(Color.Blue);

    /// <summary>
    /// Change underline color to blue.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.Blue"/> underline.</returns>
    public StyledContent<T> UnderlineBlue() => Underline(Color.Blue);

    /// <summary>
    /// Change foreground color to magenta.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.Magenta"/> foreground.</returns>
    public StyledContent<T> Magenta() => With(Color.Magenta);

    /// <summary>
    /// Change background color to magenta.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.Magenta"/> background.</returns>
    public StyledContent<T> OnMagenta() => On(Color.Magenta);

    /// <summary>
    /// Change underline color to magenta.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.Magenta"/> underline.</returns>
    public StyledContent<T> UnderlineMagenta() => Underline(Color.Magenta);

    /// <summary>
    /// Change foreground color to cyan.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.Cyan"/> foreground.</returns>
    public StyledContent<T> Cyan() => With(Color.Cyan);

    /// <summary>
    /// Change background color to cyan.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.Cyan"/> background.</returns>
    public StyledContent<T> OnCyan() => On(Color.Cyan);

    /// <summary>
    /// Change underline color to cyan.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.Cyan"/> underline.</returns>
    public StyledContent<T> UnderlineCyan() => Underline(Color.Cyan);

    /// <summary>
    /// Change foreground color to white.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.White"/> foreground.</returns>
    public StyledContent<T> White() => With(Color.White);

    /// <summary>
    /// Change background color to white.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.White"/> background.</returns>
    public StyledContent<T> OnWhite() => On(Color.White);

    /// <summary>
    /// Change underline color to white.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.White"/> underline.</returns>
    public StyledContent<T> UnderlineWhite() => Underline(Color.White);

    /// <summary>
    /// Change foreground color to grey.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.Grey"/> foreground.</returns>
    public StyledContent<T> Grey() => With(Color.Grey);

    /// <summary>
    /// Change background color to grey.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.Grey"/> background.</returns>
    public StyledContent<T> OnGrey() => On(Color.Grey);

    /// <summary>
    /// Change underline color to grey.
    /// </summary>
    /// <returns>New instance of <see cref="StyledContent{T}"/> with <see cref="Color.Grey"/> underline.</returns>
    public StyledContent<T> UnderlineGrey() => Underline(Color.Grey);
}
