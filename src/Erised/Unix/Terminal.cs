using System.Collections.Immutable;
using System.Diagnostics;
using Tmds.Linux;

namespace Erised;

internal static partial class Unix
{
    public static class Terminal
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

        public static void EnableRawMode()
        {
            using var originalMode = TerminalModePriorRawMode.Lock();
            if (originalMode.Value != null)
            {
                return;
            }

            var tty = FileDesc.TtyFd();
            var ios = GetTerminalAttributes(tty);
            var originalModeIOS = ios;

            RawTerminalAttribute(ref ios);
            SetTerminalAttributes(tty, ref ios);

            originalMode.Value = originalModeIOS;
        }

        // Reset the raw mode.
        //
        // More precisely, reset the whole termios mode to what it was before the first call
        // to [enable_raw_mode]. If you don't mess with termios outside of crossterm, it's
        // effectively disabling the raw mode and doing nothing else.
        public static void DisableRawMode()
        {
            using var originalMode = TerminalModePriorRawMode.Lock();
            if (originalMode.Value == null)
            {
                return;
            }

            var tty = FileDesc.TtyFd();
            var originalModeIOS = originalMode.Value.Value;
            SetTerminalAttributes(tty, ref originalModeIOS);
            originalMode.Value = null;
        }

        public static unsafe (ushort, ushort) Size
        {
            get
            {
                // http://rosettacode.org/wiki/Terminal_control/Dimensions#Library:_BSD_libc
                var size = new winsize();
                var fd = FileDesc.TtyFd();
                if (LibC.ioctl(fd.Fd, LibC.TIOCGWINSZ, &size) == 0)
                {
                    return (size.ws_col, size.ws_row);
                }

                return (TputValue("cols"), TputValue("rows"));

                // execute tput with the given argument and parse
                // the output as a u16.
                // The arg should be "cols" or "lines"
                static ushort TputValue(string arg)
                {
                    using var process = Process.Start("tput", arg);
                    var output = process.StandardOutput.ReadToEnd();
                    ushort.TryParse(output, out var value);
                    return value;
                }
            }
        }

        /// Queries the terminal's support for progressive keyboard enhancement.
        ///
        /// On unix systems, this function will block and possibly time out while
        /// [`crossterm::event::read`](crate::event::read) or [`crossterm::event::poll`](crate::event::poll) are being called.
        public static bool SupportsKeyboardEnhancement
        {
            get
            {
                if (IsRawModeEnabled)
                {
                    return ReadSupportKeyBoardEnhancementRaw();
                }
                else
                {
                }

                return false;
            }
        }

        private static bool ReadSupportKeyBoardEnhancementRaw()
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
            try
            {
                File.WriteAllBytes("/dev/tty", query);
            }
            catch (Exception e)
            {
                var stdout = Console.OpenStandardOutput();
                stdout.Write(query);
                stdout.Flush();
            }
            
            


            return false;
        }

        private static unsafe termios GetTerminalAttributes(FileDesc fd)
        {
            var termios = new termios();
            if (LibC.tcgetattr(fd.Fd, &termios) == -1)
            {
                PlatformException.Throw();
            }

            return termios;
        }

        private static unsafe void SetTerminalAttributes(FileDesc fd, ref termios termios)
        {
            fixed (termios* ptr = &termios)
            {
                if (LibC.tcsetattr(fd.Fd, (int)LibC.TCSANOW, ptr) == -1)
                {
                    PlatformException.Throw();
                }
            }
        }

        // Transform the given mode into an raw mode (non-canonical) mode.
        private static unsafe void RawTerminalAttribute(ref termios termios)
        {
            // TODO: implement cfmakeraw
        }
    }
}