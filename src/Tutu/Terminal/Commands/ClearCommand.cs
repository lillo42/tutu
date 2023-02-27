using Tutu.Windows;

namespace Tutu.Terminal.Commands;

/// <summary>
/// A command that clears the terminal screen buffer.
/// </summary>
/// <param name="Type">The <see cref="ClearType"/>.</param>
/// <remarks>
/// Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record ClearCommand(ClearType Type) : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write) => write.Write(Type switch
        {
            ClearType.All => $"{AnsiCodes.CSI}2J",
            ClearType.Purge => $"{AnsiCodes.CSI}3J",
            ClearType.FromCursorDown => $"{AnsiCodes.CSI}J",
            ClearType.FromCursorUp => $"{AnsiCodes.CSI}1J",
            ClearType.CurrentLine => $"{AnsiCodes.CSI}2K",
            ClearType.UntilNewLine => $"{AnsiCodes.CSI}K",
            _ => throw new ArgumentOutOfRangeException()
        });

    /// <inheritdoc />
    public void ExecuteWindowsApi() => WindowsTerminal.Clear(Type);
}