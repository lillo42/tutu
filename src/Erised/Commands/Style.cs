using Erised.Style;
using Erised.Style.Commands;
using Erised.Style.Types;
using Attribute = Erised.Style.Types.Attribute;

namespace Erised.Commands;

public static class Style
{
    /// <summary>
    /// A command that prints the given displayable type.
    ///
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// </summary>
    /// <param name="content">The content to be print.</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>New instance of <see cref="PrintCommand{T}"/>.</returns>
    public static ICommand Print<T>(T content) where T : notnull
        => new PrintCommand<T>(content);

    /// <summary>
    /// A command that prints styled content.
    /// </summary>
    /// <param name="content">The styled content.</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>New instance of <see cref="PrintStyledContentCommand{T}"/>.</returns>
    /// <remarks>
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand PrintStyledContent<T>(StyledContent<T> content) where T : notnull
        => new PrintStyledContentCommand<T>(content);

    /// <summary>
    /// A command that resets the colors back to default.
    /// </summary>
    /// <remarks>
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand Reset { get; } = new ResetColorCommand();

    /// <summary>
    /// A command that sets an attribute.
    /// </summary>
    /// <param name="contentAttribute">The <see cref="ContentAttribute"/>.</param>
    /// <returns>New instance of <see cref="SetAttributeCommand"/>.</returns>
    /// <remarks>
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand SetAttribute(Attribute contentAttribute)
        => new SetAttributeCommand(contentAttribute);

    /// <summary>
    /// A command that sets several attributes.
    /// </summary>
    /// <param name="contentAttribute">The <see cref="ContentAttribute"/>.</param>
    /// <returns>New instance of <see cref="SetAttributesCommand"/>.</returns>
    /// <remarks>
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand SetAttributes(List<Attribute> contentAttribute)
        => new SetAttributesCommand(contentAttribute);

    /// <summary>
    /// A command that sets several attributes.
    /// </summary>
    /// <param name="contentAttribute">The <see cref="ContentAttribute"/>.</param>
    /// <returns>New instance of <see cref="SetAttributesCommand"/>.</returns>
    /// <remarks>
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand SetAttributes(params Attribute[] contentAttribute)
        => SetAttributes(new List<Attribute>(contentAttribute));

    /// <summary>
    /// A command that sets the the background color.
    /// </summary>
    /// <param name="color">The <see cref="ICommand"/>.</param>
    /// <returns>New instance of <see cref="SetBackgroundColorCommand"/>.</returns>
    /// <remarks>
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand SetBackgroundColor(Color color)
        => new SetBackgroundColorCommand(color);

    /// <summary>
    /// A command that optionally sets the foreground and/or background color.
    /// </summary>
    /// <param name="foreground">The foreground <see cref="Color"/>.</param>
    /// <param name="background">The background <see cref="Color"/>.</param>
    /// <returns>New instance of <see cref="SetColorsCommand"/>.</returns>
    /// <remarks>
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand SetColors(Color? foreground, Color? background)
        => new SetColorsCommand(foreground, background);

    /// <summary>
    /// A command that sets the the foreground color.
    /// </summary>
    /// <param name="color"></param>
    /// <returns>New instance of <see cref="SetForegroundColorCommand"/>.</returns>
    /// <remarks>
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand SetForegroundColor(Color color)
        => new SetForegroundColorCommand(color);

    /// <summary>
    /// A command that sets a style (colors and attributes).
    /// </summary>
    /// <param name="style">The <see cref="ContentStyled{T}"/>.</param>
    /// <returns>New instance <see cref="SetStyleCommand"/>.</returns>
    /// <remarks>
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand SetStyleCommand(ContentStyled style)
        => new SetStyleCommand(style);


    /// <summary>
    /// A command that sets the the underline color.
    /// </summary>
    /// <param name="color">The <see cref="Color"/>.</param>
    /// <remarks>New instance of <see cref="SetUnderlineColorCommand"/>.</remarks>
    /// <remarks>
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand SetUnderlineColor(Color color)
        => new SetUnderlineColorCommand(color);
}