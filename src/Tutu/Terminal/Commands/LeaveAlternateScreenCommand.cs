using Tutu.Windows2;

namespace Tutu.Terminal.Commands;

/// <summary>
/// A command that switches back to the main screen.
/// </summary>
/// <remarks>
/// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
/// <para>Use <see cref="EnterAlternateScreenCommand"/> to enter the alternate screen.</para>
/// </remarks>
public record LeaveAlternateScreenCommand : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write) 
        => write.Write($"{AnsiCodes.CSI}?1049l");

    /// <inheritdoc />
    public void ExecuteWindowsApi() => ScreenBuffer.CurrentOutput.Show();
}