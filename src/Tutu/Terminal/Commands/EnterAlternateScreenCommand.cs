using Tutu.Windows2;

namespace Tutu.Terminal.Commands;

/// <summary>
/// A command that switches to alternate screen.
/// </summary>
/// <remarks>
/// * Commands must be executed/queued for execution otherwise they do nothing.
/// * Use [LeaveAlternateScreen](./struct.LeaveAlternateScreen.html) command to leave the entered alternate screen.
/// </remarks>
public record EnterAlternateScreenCommand : ICommand
{
    public void WriteAnsi(TextWriter write) 
        => write.Write($"{AnsiCodes.CSI}?1049h");

    public void ExecuteWindowsApi() => ScreenBuffer.Create().Show();
}