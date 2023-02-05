using System.Runtime.InteropServices;

namespace Erised;

internal static class AnsiSupport
{
    private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
    public static bool IsAnsiSupported { get; }

    static AnsiSupport()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            IsAnsiSupported = EnableVirtualTerminalProcessing();
        }

        IsAnsiSupported = true;
    }

    private static bool EnableVirtualTerminalProcessing()
    {
        var console = new Windows.Console(Windows.Handle.CurrentOutHandle());
        var oldMode = console.Mode;
        
        if((oldMode & ENABLE_VIRTUAL_TERMINAL_PROCESSING) == 0)
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
}