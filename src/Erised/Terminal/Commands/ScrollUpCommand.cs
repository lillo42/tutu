namespace Erised.Terminal.Commands;

/// <summary>
/// A command that scrolls the terminal screen a given number of rows up.
/// </summary>
/// <param name="Lines">Number of line to be jump up.</param>
/// <remarks>
/// * Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record ScrollUpCommand(ushort Lines) : ICommand
{
    public void WriteAnsi(TextWriter write) => write.Write($"{AnsiCodes.CSI}{Lines}S");

    public void ExecuteWindowsApi() => Windows.Terminal.ScrollUp(Lines);
}