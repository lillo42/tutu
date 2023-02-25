using System.Collections.Immutable;
using Attribute = Tutu.Style.Types.Attribute;

namespace Tutu.Style.Extensions;

public static class StyledContentExtensions
{
    private static readonly ContentStyled Default = new(null, null, null, ImmutableList<Attribute>.Empty);

    public static StyledContent<T> Reset<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Reset();

    public static StyledContent<T> Bold<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Bold();

    public static StyledContent<T> Underline<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Bold();

    public static StyledContent<T> Reverse<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Reverse();

    public static StyledContent<T> Dim<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Dim();

    public static StyledContent<T> Italic<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Italic();

    public static StyledContent<T> Negative<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Negative();

    public static StyledContent<T> SlowBlink<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).SlowBlink();

    public static StyledContent<T> RapidBlink<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).RapidBlink();

    public static StyledContent<T> Hidden<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Hidden();

    public static StyledContent<T> CrossedOut<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).CrossedOut();

    public static StyledContent<T> Black<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Black();

    public static StyledContent<T> OnBlack<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnBlack();

    public static StyledContent<T> UnderlineBlack<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineBlack();

    public static StyledContent<T> DarkGrey<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).DarkGrey();

    public static StyledContent<T> OnDarkGrey<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnDarkGrey();

    public static StyledContent<T> UnderlineDarkGrey<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineDarkGrey();

    public static StyledContent<T> Red<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Red();

    public static StyledContent<T> OnRed<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnRed();

    public static StyledContent<T> UnderlineRed<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineRed();

    public static StyledContent<T> Green<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Green();

    public static StyledContent<T> OnGreen<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnGreen();

    public static StyledContent<T> UnderlineGreen<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineGreen();

    public static StyledContent<T> Yellow<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Yellow();

    public static StyledContent<T> OnYellow<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnYellow();

    public static StyledContent<T> UnderlineYellow<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineYellow();

    public static StyledContent<T> Blue<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Blue();

    public static StyledContent<T> OnBlue<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnBlue();

    public static StyledContent<T> UnderlineBlue<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineBlue();

    public static StyledContent<T> Magenta<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Magenta();

    public static StyledContent<T> OnMagenta<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnMagenta();

    public static StyledContent<T> UnderlineMagenta<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineMagenta();

    public static StyledContent<T> Cyan<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Cyan();

    public static StyledContent<T> OnCyan<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnCyan();

    public static StyledContent<T> UnderlineCyan<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineCyan();

    public static StyledContent<T> White<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).White();

    public static StyledContent<T> OnWhite<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnWhite();

    public static StyledContent<T> UnderlineWhite<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineWhite();

    public static StyledContent<T> Grey<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Grey();

    public static StyledContent<T> OnGrey<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnGrey();

    public static StyledContent<T> UnderlineGrey<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineGrey();
}