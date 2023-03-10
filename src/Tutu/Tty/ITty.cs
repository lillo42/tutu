namespace Tutu.Tty;

/// <summary>
/// The TTY interface.
/// </summary>
public interface ITty
{
    /// <summary>
    /// Whether the current process is a TTY.
    /// </summary>
    bool IsTty { get; }
}
