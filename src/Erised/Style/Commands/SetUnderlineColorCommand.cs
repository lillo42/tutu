using Erised.Style.Types;

namespace Erised.Style.Commands;

/// <summary>
/// A command that sets the the underline color.
/// </summary>
/// <param name="Color">The <see cref="Erised.Style.Types.Color"/>.</param>
/// <remarks>
/// Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record SetUnderlineColorCommand(Color Color) : ICommand
{
    public void WriteAnsi(TextWriter write)
        => Execute(write, Color); 

    internal static void Execute(TextWriter write, Color color)
        => write.Write($"{AnsiCodes.CSI}{Colored.UnderlineColor(color)}m");

    public void ExecuteWindowsApi() => throw new NotSupportedException("SetUnderlineColor not supported by winapi.");
}