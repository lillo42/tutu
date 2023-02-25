namespace Tutu.Events.Commands;

/// <summary>
/// A command that enables focus event emission.
/// </summary>
/// <remarks>
/// <para>It should be paired with <see cref="DisableFocusChangeCommand"/> at the end of execution.</para>
/// <para>Focus events can be captured with [read](./fn.read.html)/[poll](./fn.poll.html).</para>
/// </remarks>
public record EnableFocusChangeCommand : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write) 
        => write.Write($"{AnsiCodes.CSI}?1004h");

    /// <inheritdoc />
    public void ExecuteWindowsApi() { }
}