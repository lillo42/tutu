using Tutu.Style.Types;

namespace Tutu.Style.Commands;

/// <summary>
/// A command that sets the the underline color.
/// </summary>
/// <param name="Color">The <see cref="Types.Color"/>.</param>
/// <remarks>
/// Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public sealed record SetUnderlineColorCommand(Color Color) : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => Execute(write, Color);

    internal static void Execute(TextWriter write, Color color)
        => write.Write($"{AnsiCodes.CSI}{Colored.UnderlineColor(color)}m");

    /// <inheritdoc />
    public void ExecuteWindowsApi() => throw new NotSupportedException("SetUnderlineColor not supported by winapi.");
}
