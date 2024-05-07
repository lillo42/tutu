using Tutu.Windows;

namespace Tutu.Terminal.Commands;

/// <summary>
/// A command that sets the terminal title
/// </summary>
/// <param name="Title">The new title.</param>
/// <remarks>
/// Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public sealed record SetTitleCommand(string Title) : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write) => write.Write($"{AnsiCodes.ESC}]0;{Title}\x07");

    /// <inheritdoc />
    public void ExecuteWindowsApi() => WindowsTerminal.SetTitle(Title);
}
