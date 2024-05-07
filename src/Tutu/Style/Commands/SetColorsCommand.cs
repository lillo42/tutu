using Tutu.Style.Types;
using Tutu.Windows;

namespace Tutu.Style.Commands;

/// <summary>
/// A command that optionally sets the foreground and/or background color.
/// </summary>
/// <param name="Foreground">The foreground <see cref="Color"/>.</param>
/// <param name="Background">The background <see cref="Color"/>.</param>
/// <remarks>
/// Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public sealed record SetColorsCommand(Color? Foreground, Color? Background) : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
    {
        if (Foreground != null)
        {
            SetForegroundColorCommand.Execute(write, Foreground.Value);
        }

        if (Background != null)
        {
            SetBackgroundColorCommand.Execute(write, Background.Value);
        }
    }

    /// <inheritdoc />
    public void ExecuteWindowsApi()
    {
        if (Foreground != null)
        {
            WindowsConsole.CurrentOutput.SetForegroundColor(Foreground.Value);
        }

        if (Background != null)
        {
            WindowsConsole.CurrentOutput.SetBackgroundColor(Background.Value);
        }
    }
}
