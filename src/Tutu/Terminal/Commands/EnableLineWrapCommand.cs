namespace Tutu.Terminal.Commands;

/// <summary>
/// Enable line wrapping.
/// </summary>
public record EnableLineWrapCommand : ICommand
{
    public void WriteAnsi(TextWriter write) 
        => write.Write($"{AnsiCodes.CSI}?7h");

    public void ExecuteWindowsApi()
    {
        var screenBuffer = Windows.Windows.ScreenBuffer.Current;
        var console = new Windows.Windows.Console(screenBuffer.Handle);
        console.Mode = (ushort)(console.Mode & Windows.Windows.Console.EnableWrapAtEolOutput);
    }
}