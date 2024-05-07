using Tutu.Windows;

namespace Tutu.Terminal.Commands;

/// <summary>
/// Enable line wrapping.
/// </summary>
public sealed record EnableLineWrapCommand : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.CSI}?7h");

    /// <inheritdoc />
    public void ExecuteWindowsApi()
    {
        var console = WindowsConsole.CurrentOutput;
        console.Mode = (ushort)(console.Mode & WindowsConsole.EnableWrapAtEolOutput);
    }
}
