using Erised.Style.Types;

namespace Erised.Style.Commands;

/// <summary>
/// A command that sets the the foreground color.
/// </summary>
/// <param name="Color">The <see cref="Erised.Style.Types.Color"/>.</param>
/// <remarks>
/// Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record SetForegroundColorCommand(Color Color) : ICommand
{
    public void WriteAnsi(TextWriter write)
        => Execute(write, Color);

    internal static void Execute(TextWriter write, Color color)
        => write.Write($"{AnsiCodes.CSI}{Colored.ForegroundColor(color)}m");

    public void ExecuteWindowsApi() => Windows.Style.SetForegroundColor(Color);
}