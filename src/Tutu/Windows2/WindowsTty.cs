using Tutu.Tty;

namespace Tutu.Windows2;

/// <summary>
/// Windows implementation of <see cref="ITty"/>.
/// </summary>
public class WindowsTty : ITty
{
    /// <inheritdoc cref="ITty.IsTty" />
    public bool IsTty => WindowsConsole.Input.Mode == 1;
}