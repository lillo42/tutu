using Erised.Style.Types;

namespace Erised.Style.Commands;

/// <summary>
/// A command that sets the the background color.
/// </summary>
/// <param name="Color">The <see cref="ICommand"/>.</param>
/// <remarks>
/// Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record SetBackgroundColorCommand(Color Color) : ICommand
{
    public void WriteAnsi(TextWriter write)
        => Execute(write, Color);

    internal static void Execute(TextWriter write, Color color)
        => write.Write($"{AnsiCodes.CSI}{Colored.BackgroundColor(color)}m");

    public void ExecuteWindowsApi() 
        => Windows.Style.SetBackgroundColor(Color);
}