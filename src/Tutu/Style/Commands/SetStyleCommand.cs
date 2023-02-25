namespace Tutu.Style.Commands;

/// <summary>
/// A command that sets a style (colors and attributes).
/// </summary>
/// <param name="Style"></param>
/// <remarks>
///  Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record SetStyleCommand(ContentStyled Style) : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
    {
        if (Style.BackgroundColor != null)
        {
            SetBackgroundColorCommand.Execute(write, Style.BackgroundColor.Value);
        }

        if (Style.ForegroundColor != null)
        {
            SetForegroundColorCommand.Execute(write, Style.ForegroundColor.Value);
        }

        if (Style.UnderlineColor != null)
        {
            SetUnderlineColorCommand.Execute(write, Style.UnderlineColor.Value);
        }

        if (Style.Attributes.Count > 0)
        {
            SetAttributesCommand.Execute(write, Style.Attributes);
        }
    }

    /// <inheritdoc />
    public void ExecuteWindowsApi() =>
        throw new NotSupportedException("tried to execute SetStyle command using WinAPI, use ANSI instead");

    /// <inheritdoc />
    bool ICommand.IsAnsiCodeSupported => true;
}