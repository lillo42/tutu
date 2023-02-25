using Tutu.Style.Types;

namespace Tutu.Style.Commands;

/// <summary>
/// A command that prints styled content.
/// </summary>
/// <param name="Content">Print content styled.</param>
/// <typeparam name="T"></typeparam>
/// <remarks>
/// Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record PrintStyledContentCommand<T>(StyledContent<T> Content) : ICommand
    where T : notnull
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
    {
        var style = Content.Style;

        var resetBackground = false;
        var resetForeground = false;
        var reset = false;

        if (style.BackgroundColor != null)
        {
            SetBackgroundColorCommand.Execute(write, style.BackgroundColor.Value);
            resetBackground = true;
        }

        if (style.ForegroundColor != null)
        {
            SetForegroundColorCommand.Execute(write, style.ForegroundColor.Value);
            resetForeground = true;
        }

        if (style.UnderlineColor != null)
        {
            SetUnderlineColorCommand.Execute(write, style.UnderlineColor.Value);
            resetForeground = true;
        }

        if (style.Attributes.Count > 0)
        {
            SetAttributesCommand.Execute(write, style.Attributes);
            reset = true;
        }


        write.Write(Content.Content);
        if (reset)
        {
            ResetColorCommand.Execute(write);
        }
        else if (resetBackground)
        {
            SetBackgroundColorCommand.Execute(write, Color.Reset);
        }
        else if (resetForeground)
        {
            SetForegroundColorCommand.Execute(write, Color.Reset);
        }
    }

    /// <inheritdoc />
    public void ExecuteWindowsApi()
    {
        // Should ignore this command on windows api.
    }
}