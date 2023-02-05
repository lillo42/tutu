using Tmds.Linux;

namespace Erised;

internal static partial class Unix
{
    private static readonly Mutex<termios?> TerminalModePriorRawMode = new(null);

    public static bool IsRawModeEnabled
    {
        get
        {
            using var value = TerminalModePriorRawMode.Lock();
            return value.Value != null;
        }
    }


    public static bool SupportKeyboardEnhancement
    {
        get
        {
            if (IsRawModeEnabled)
            {
                return ReadSupportsKeyboardEnhancementRaw;
            }

            return false;
        }
    }

    private static bool ReadSupportsKeyboardEnhancementRaw
    {
        get
        {
            // This is the recommended method for testing support for the keyboard enhancement protocol.
            // We send a query for the flags supported by the terminal and then the primary device attributes
            // query. If we receive the primary device attributes response but not the keyboard enhancement
            // flags, none of the flags are supported.
            //
            // See <https://sw.kovidgoyal.net/kitty/keyboard-protocol/#detection-of-support-for-this-protocol>

            // ESC [ ? u        Query progressive keyboard enhancement flags (kitty protocol).
            // ESC [ c          Query primary device attributes. 

            var query = "\x1B[?u\x1B[c"u8.ToArray();
            if (TryToOpen("/dev/tty", out var file))
            {
                file.Write(query);
                file.Flush();
            }
            else
            {
                var @out = Console.Out;
                @out.Write(query);
                @out.Flush();
                return false;
            }
            
            

            return true;
        }
    }
}