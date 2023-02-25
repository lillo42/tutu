namespace Tutu.Terminal.Commands;

/// <summary>
/// Disables line wrapping.
/// </summary>
public record DisableLineWrapCommand : ICommand
{
    public void WriteAnsi(TextWriter write) 
        => write.Write($"{AnsiCodes.CSI}?7l");

    public void ExecuteWindowsApi()
    {
        var screenBuffer = Windows.Windows.ScreenBuffer.Current;
        var console = new Windows.Windows.Console(screenBuffer.Handle);
        console.Mode = (uint)(console.Mode & ~Windows.Windows.Console.EnableWrapAtEolOutput);
    }
}