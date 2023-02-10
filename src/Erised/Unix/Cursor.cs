namespace Erised;

internal static partial class Unix
{
    public static partial class Cursor
    {
        /// <summary>
        /// The cursor position.
        /// </summary>
        public static (ushort Column, ushort Row) Position
        {
            get
            {
                if (Terminal.IsRawModeEnabled)
                {
                    return ReadPositionRaw();
                }

                Terminal.EnableRawMode();
                var position = ReadPositionRaw();
                Terminal.DisableRawMode();
                return position;
            }
        }

        private static (ushort, ushort) ReadPositionRaw()
        {
            var stdout = Console.OpenStandardOutput();
            stdout.Write("\x1B[6n"u8.ToArray());
            stdout.Flush();

            return (0, 0);
            /*while (true)
            {
                
            }*/
        }
        
        public static bool SupportKeyboardEnhancement
        {
            get
            {
                if (Terminal.IsRawModeEnabled)
                {
                    return ReadSupportsKeyboardEnhancementRaw;
                }

                return false;
            }
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