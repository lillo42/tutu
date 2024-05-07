namespace Tutu.Events.Commands;

/// <summary>
/// A command that enables  <see href="https://en.wikipedia.org/wiki/Bracketed-paste">bracketed paste mode</see>.
/// </summary>
/// <remarks>
/// <para>It should be paired with <see cref="DisableBracketedPasteCommand"/> at the end of execution.</para>
/// <para>This is not supported in older Windows terminals without
/// <see href="https://docs.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences">virtual terminal sequences</see>.
/// </para>
/// </remarks>
public sealed record EnableBracketedPasteCommand : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.CSI}?2004h");

    /// <inheritdoc />
    public void ExecuteWindowsApi() =>
        throw new NotSupportedException("Bracketed paste not implemented in the legacy Windows API.");
}
