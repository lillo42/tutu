namespace Tutu.Windows;

/// <summary>
/// Windows implementation of <see cref="IAnsiSupport"/>.
/// </summary>
public sealed class WindowsAnsiSupport : IAnsiSupport
{
    private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

    private static readonly bool _isAnsiSupported;

    static WindowsAnsiSupport()
    {
        _isAnsiSupported = EnableVirtualTerminalProcessing();
    }

    private static bool EnableVirtualTerminalProcessing()
    {
        var console = WindowsConsole.CurrentOutput;
        var oldMode = console.Mode;

        if ((oldMode & ENABLE_VIRTUAL_TERMINAL_PROCESSING) == 0)
        {
            try
            {
                console.Mode = oldMode | ENABLE_VIRTUAL_TERMINAL_PROCESSING;
            }
            catch
            {
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc cref="IAnsiSupport.IsAnsiSupported"/>
    public bool IsAnsiSupported => _isAnsiSupported;
}
