using System.Collections.Immutable;
using Attribute = Tutu.Style.Types.Attribute;

namespace Tutu.Style.Extensions;

/// <summary>
/// The <see cref="StyledContent{T}"/> extensions.
/// </summary>
public static class StyledContentExtensions
{
    private static readonly ContentStyled Default = new(null, null, null, ImmutableList<Attribute>.Empty);

    /// <inheritdoc cref="StyledContent{T}.Reset"/>
    public static StyledContent<T> Reset<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Reset();

    /// <inheritdoc cref="StyledContent{T}.Bold"/>
    public static StyledContent<T> Bold<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Bold();

    /// <inheritdoc cref="StyledContent{T}.Underline()"/>
    public static StyledContent<T> Underline<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Underline();

    /// <inheritdoc cref="StyledContent{T}.Reverse"/>
    public static StyledContent<T> Reverse<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Reverse();

    /// <inheritdoc cref="StyledContent{T}.Dim"/>
    public static StyledContent<T> Dim<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Dim();

    /// <inheritdoc cref="StyledContent{T}.Italic"/>
    public static StyledContent<T> Italic<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Italic();

    /// <inheritdoc cref="StyledContent{T}.Negative"/>
    public static StyledContent<T> Negative<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Negative();

    /// <inheritdoc cref="StyledContent{T}.SlowBlink"/>
    public static StyledContent<T> SlowBlink<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).SlowBlink();

    /// <inheritdoc cref="StyledContent{T}.RapidBlink"/>
    public static StyledContent<T> RapidBlink<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).RapidBlink();

    /// <inheritdoc cref="StyledContent{T}.Hidden"/>
    public static StyledContent<T> Hidden<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Hidden();

    /// <inheritdoc cref="StyledContent{T}.CrossedOut"/>
    public static StyledContent<T> CrossedOut<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).CrossedOut();

    /// <inheritdoc cref="StyledContent{T}.Black"/>
    public static StyledContent<T> Black<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Black();

    /// <inheritdoc cref="StyledContent{T}.OnBlack"/>
    public static StyledContent<T> OnBlack<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnBlack();

    /// <inheritdoc cref="StyledContent{T}.UnderlineBlack"/>
    public static StyledContent<T> UnderlineBlack<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineBlack();

    /// <inheritdoc cref="StyledContent{T}.DarkGrey"/>
    public static StyledContent<T> DarkGrey<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).DarkGrey();

    /// <inheritdoc cref="StyledContent{T}.DarkGrey"/>
    public static StyledContent<T> OnDarkGrey<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnDarkGrey();

    /// <inheritdoc cref="StyledContent{T}.UnderlineDarkGrey"/>
    public static StyledContent<T> UnderlineDarkGrey<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineDarkGrey();

    /// <inheritdoc cref="StyledContent{T}.Red"/>
    public static StyledContent<T> Red<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Red();

    /// <inheritdoc cref="StyledContent{T}.OnRed"/>
    public static StyledContent<T> OnRed<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnRed();

    /// <inheritdoc cref="StyledContent{T}.UnderlineRed"/>
    public static StyledContent<T> UnderlineRed<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineRed();

    /// <inheritdoc cref="StyledContent{T}.Green"/>
    public static StyledContent<T> Green<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Green();

    /// <inheritdoc cref="StyledContent{T}.OnGreen"/>
    public static StyledContent<T> OnGreen<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnGreen();

    /// <inheritdoc cref="StyledContent{T}.UnderlineGreen"/>
    public static StyledContent<T> UnderlineGreen<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineGreen();

    /// <inheritdoc cref="StyledContent{T}.Yellow"/>
    public static StyledContent<T> Yellow<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Yellow();

    /// <inheritdoc cref="StyledContent{T}.OnYellow"/>
    public static StyledContent<T> OnYellow<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnYellow();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> UnderlineYellow<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineYellow();

    /// <inheritdoc cref="StyledContent{T}.Blue"/>
    public static StyledContent<T> Blue<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Blue();

    /// <inheritdoc cref="StyledContent{T}.OnBlue"/>
    public static StyledContent<T> OnBlue<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnBlue();

    /// <inheritdoc cref="StyledContent{T}.UnderlineBlue"/>
    public static StyledContent<T> UnderlineBlue<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineBlue();

    /// <inheritdoc cref="StyledContent{T}.Magenta"/>
    public static StyledContent<T> Magenta<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Magenta();

    /// <inheritdoc cref="StyledContent{T}.OnMagenta"/>
    public static StyledContent<T> OnMagenta<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnMagenta();

    /// <inheritdoc cref="StyledContent{T}.UnderlineMagenta"/>
    public static StyledContent<T> UnderlineMagenta<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineMagenta();

    /// <inheritdoc cref="StyledContent{T}.Cyan"/>
    public static StyledContent<T> Cyan<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Cyan();

    /// <inheritdoc cref="StyledContent{T}.OnCyan"/>
    public static StyledContent<T> OnCyan<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnCyan();

    /// <inheritdoc cref="StyledContent{T}.UnderlineCyan"/>
    public static StyledContent<T> UnderlineCyan<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineCyan();

    /// <inheritdoc cref="StyledContent{T}.White"/>
    public static StyledContent<T> White<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).White();

    /// <inheritdoc cref="StyledContent{T}.OnWhite"/>
    public static StyledContent<T> OnWhite<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnWhite();

    /// <inheritdoc cref="StyledContent{T}.UnderlineWhite"/>
    public static StyledContent<T> UnderlineWhite<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineWhite();

    /// <inheritdoc cref="StyledContent{T}.Grey"/>
    public static StyledContent<T> Grey<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Grey();

    /// <inheritdoc cref="StyledContent{T}.OnGrey"/>
    public static StyledContent<T> OnGrey<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnGrey();

    /// <inheritdoc cref="StyledContent{T}.UnderlineGrey"/>
    public static StyledContent<T> UnderlineGrey<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineGrey();
}
