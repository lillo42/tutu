using Tutu.Windows;

namespace Tutu.Terminal.Commands;

/// <summary>
/// Disables line wrapping.
/// </summary>
public record DisableLineWrapCommand : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write) 
        => write.Write($"{AnsiCodes.CSI}?7l");

    /// <inheritdoc />
    public void ExecuteWindowsApi()
    {
        var console = WindowsConsole.CurrentOutput;
        console.Mode = (uint)(console.Mode & ~WindowsConsole.EnableWrapAtEolOutput);
    }
}