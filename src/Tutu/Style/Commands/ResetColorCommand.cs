using Tutu.Windows;

namespace Tutu.Style.Commands;

/// <summary>
/// A command that resets the colors back to default.
/// </summary>
/// <remarks>
/// Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record ResetColorCommand : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => Execute(write);

    internal static void Execute(TextWriter write)
        => write.Write($"{AnsiCodes.CSI}0m");

    /// <inheritdoc />
    public void ExecuteWindowsApi() => WindowsConsole.CurrentOutput.Reset();
}
