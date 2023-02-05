namespace Erised.Terminal.Commands;

/// <summary>
/// A command that scrolls the terminal screen a given number of rows down.
/// </summary>
/// <param name="Lines">Number of line to be jump down.</param>
/// <remarks>
/// * Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record ScrollDownCommand(ushort Lines) : ICommand
{
    public void WriteAnsi(TextWriter write) => write.Write($"{AnsiCodes.CSI}{Lines}T");

    public void ExecuteWindowsApi() => Windows.Terminal.ScrollDown(Lines);
}