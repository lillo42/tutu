using Tutu.Windows2;

namespace Tutu.Terminal.Commands;

/// <summary>
/// A command that switches to alternate screen.
/// </summary>
/// <remarks>
/// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
/// <para>Use <see cref="LeaveAlternateScreenCommand"/> command to leave the entered alternate screen.</para>
/// </remarks>
public record EnterAlternateScreenCommand : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write) 
        => write.Write($"{AnsiCodes.CSI}?1049h");

    /// <inheritdoc />
    public void ExecuteWindowsApi() => ScreenBuffer.Create().Show();
}