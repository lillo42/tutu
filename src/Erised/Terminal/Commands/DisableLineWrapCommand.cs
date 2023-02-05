namespace Erised.Terminal.Commands;

/// <summary>
/// Disables line wrapping.
/// </summary>
public record DisableLineWrapCommand : ICommand
{
    public void WriteAnsi(TextWriter write) 
        => write.Write($"{AnsiCodes.CSI}?7l");

    public void ExecuteWindowsApi()
    {
        var screenBuffer = Windows.ScreenBuffer.Current;
        var console = new Windows.Console(screenBuffer.Handle);
        console.Mode = (uint)(console.Mode & ~Windows.Console.EnableWrapAtEolOutput);
    }
}