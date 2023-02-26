using System.Collections.Immutable;
using Attribute = Tutu.Style.Types.Attribute;

namespace Tutu.Style.Extensions;

/// <summary>
/// The <see cref="StyledContent{T}"/> extensions.
/// </summary>
public static class StyledContentExtensions
{
    private static readonly ContentStyled Default = new(null, null, null, ImmutableList<Attribute>.Empty);

    /// <summary>
    /// Mark the <see cref="StyledContent{T}"/> as reset.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> Reset<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Reset();

    /// <summary>
    /// Mark the <see cref="StyledContent{T}"/> as bold.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with bold option.</returns>
    public static StyledContent<T> Bold<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Bold();

    /// <summary>
    /// Mark the <see cref="StyledContent{T}"/> as underline.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with underline.</returns>
    public static StyledContent<T> Underline<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Bold();

    /// <summary>
    /// Mark the <see cref="StyledContent{T}"/> as Reverse.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reverse option.</returns>
    public static StyledContent<T> Reverse<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Reverse();

    /// <summary>
    /// Mark the <see cref="StyledContent{T}"/> as Dim.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Dim option.</returns>
    public static StyledContent<T> Dim<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Dim();

    /// <summary>
    /// Mark the <see cref="StyledContent{T}"/> as Italic.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Italic option.</returns>
    public static StyledContent<T> Italic<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Italic();

    /// <summary>
    /// Mark the <see cref="StyledContent{T}"/> as Negative.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Negative option.</returns>
    public static StyledContent<T> Negative<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Negative();

    /// <summary>
    /// Mark the <see cref="StyledContent{T}"/> as Slow blink.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Slow blink option.</returns>
    public static StyledContent<T> SlowBlink<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).SlowBlink();

    /// <summary>
    /// Mark the <see cref="StyledContent{T}"/> as Rapid blink.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Rapid blink option.</returns>
    public static StyledContent<T> RapidBlink<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).RapidBlink();

    /// <summary>
    /// Mark the <see cref="StyledContent{T}"/> as Hidden.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Hidden option.</returns>
    public static StyledContent<T> Hidden<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Hidden();

    /// <summary>
    /// Mark the <see cref="StyledContent{T}"/> as Crossed out.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Crossed out option.</returns>
    public static StyledContent<T> CrossedOut<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).CrossedOut();

    /// <summary>
    /// Mark the <see cref="StyledContent{T}"/> as black.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Black option.</returns>
    public static StyledContent<T> Black<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Black();

    // TODO: Improve comments

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> OnBlack<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnBlack();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> UnderlineBlack<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineBlack();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> DarkGrey<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).DarkGrey();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> OnDarkGrey<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnDarkGrey();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> UnderlineDarkGrey<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineDarkGrey();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> Red<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Red();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> OnRed<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnRed();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> UnderlineRed<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineRed();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> Green<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Green();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> OnGreen<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnGreen();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> UnderlineGreen<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineGreen();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> Yellow<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Yellow();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
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

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> Blue<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Blue();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> OnBlue<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnBlue();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> UnderlineBlue<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineBlue();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> Magenta<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Magenta();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> OnMagenta<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnMagenta();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> UnderlineMagenta<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineMagenta();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> Cyan<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Cyan();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> OnCyan<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnCyan();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> UnderlineCyan<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineCyan();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> White<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).White();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> OnWhite<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnWhite();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> UnderlineWhite<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineWhite();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> Grey<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).Grey();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> OnGrey<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).OnGrey();

    /// <summary>
    /// Resets the <see cref="StyledContent{T}"/> to the default.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <typeparam name="T">The content type.</typeparam>
    /// <returns>A new instance of <see cref="StyledContent{T}"/> with Reset option.</returns>
    public static StyledContent<T> UnderlineGrey<T>(this T content) where T : notnull =>
        new StyledContent<T>(Default, content).UnderlineGrey();
}