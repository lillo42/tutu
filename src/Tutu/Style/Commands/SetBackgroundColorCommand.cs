using Tutu.Style.Types;
using Tutu.Windows2;

namespace Tutu.Style.Commands;

/// <summary>
/// A command that sets the the background color.
/// </summary>
/// <param name="Color">The <see cref="ICommand"/>.</param>
/// <remarks>
/// Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record SetBackgroundColorCommand(Color Color) : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => Execute(write, Color);

    internal static void Execute(TextWriter write, Color color)
        => write.Write($"{AnsiCodes.CSI}{Colored.BackgroundColor(color)}m");

    /// <inheritdoc />
    public void ExecuteWindowsApi() 
        => WindowsConsole.CurrentOutput.SetBackgroundColor(Color);
}