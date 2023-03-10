using System.Diagnostics;
using System.Runtime.InteropServices;
using NodaTime;
using Tutu.Events;
using Tutu.Exceptions;
using Tutu.Terminal;
using Tutu.Unix.Interop.LibC;
using static Tutu.Events.DefaultEventReader;
using static Tutu.Unix.Interop.LibC.LibC;

namespace Tutu.Unix;

/// <summary>
/// The Unix implementation of <see cref="ITerminal"/>.
/// </summary>
/// <remarks>
/// It should be use as Singleton.
/// </remarks>
public class UnixTerminal : ITerminal
{
    private readonly Mutex<termios?> _terminalModePriorRawMode = new(null);

    /// <inheritdoc cref="ITerminal.IsRawModeEnabled"/> 
    public bool IsRawModeEnabled
    {
        get
        {
            using var value = _terminalModePriorRawMode.Lock();
            return value.Value != null;
        }
    }

    /// <inheritdoc cref="ITerminal.DisableRawMode"/> 
    public void EnableRawMode()
    {
        using var originalMode = _terminalModePriorRawMode.Lock();
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

    /// <inheritdoc cref="ITerminal.DisableRawMode"/> 
    public void DisableRawMode()
    {
        using var originalMode = _terminalModePriorRawMode.Lock();
        if (originalMode.Value == null)
        {
            return;
        }

        var tty = FileDesc.TtyFd();
        var originalModeIOS = originalMode.Value.Value;
        SetTerminalAttributes(tty, ref originalModeIOS);
        originalMode.Value = null;
    }

    /// <inheritdoc cref="ITerminal.Size"/> 
    public unsafe TerminalSize Size
    {
        get
        {
            FileStream? file = null;
            try
            {
                FileDesc fd;
                try
                {
                    file = File.Open("/dev/tty", FileMode.Open);
                    fd = new FileDesc(file.SafeFileHandle.DangerousGetHandle().ToInt32(), false);
                }
                catch
                {
                    fd = new FileDesc(STDOUT_FILENO, false);
                }

                // http://rosettacode.org/wiki/Terminal_control/Dimensions#Library:_BSD_libc
                var size = new winsize();
                if (ioctl(fd, TIOCGWINSZ, &size) == 0)
                {
                    return new(size.ws_col, size.ws_row);
                }

                return new(TputValue("cols"), TputValue("lines"));
            }
            finally
            {
                file?.Dispose();
            }

            // execute tput with the given argument and parse
            // the output as a u16.
            // The arg should be "cols" or "lines"
            static ushort TputValue(string arg)
            {
                using var process = new Process();
                process.StartInfo.FileName = "tput";
                process.StartInfo.Arguments = arg;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
                var output = process.StandardOutput.ReadToEnd();
                if (!ushort.TryParse(output, out var value))
                {
                    throw new IOException("Could not parse tput output.");
                }

                return value;
            }
        }
    }

    /// <inheritdoc cref="ITerminal.SupportsKeyboardEnhancement"/> 
    public bool SupportsKeyboardEnhancement =>
        IsRawModeEnabled ? ReadSupportKeyBoardEnhancementRaw() : ReadSupportKeyBoardEnhancementFlags();

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
        catch
        {
            var stdout = Console.OpenStandardOutput();
            stdout.Write(query);
            stdout.Flush();
        }

        while (true)
        {
            try
            {
                var hasEvent = PollInternal(Duration.FromSeconds(2), KeyboardEnhancementFlagsFilter.Default);

                if (hasEvent)
                {
                    var read = ReadInternal(KeyboardEnhancementFlagsFilter.Default);
                    if (read is KeyboardEnhancementFlagsInternalEvent)
                    {
                        // Flush the PrimaryDeviceAttributes out of the event queue.
                        ReadInternal(PrimaryDeviceAttributesFilter.Default);
                        return true;
                    }

                    return false;
                }
            }
            catch
            {
                // retry in case of an error 
                continue;
            }

            throw new KeyboardEnhancementException(
                "The keyboard enhancement status could not be read within a normal duration");
        }
    }

    private bool ReadSupportKeyBoardEnhancementFlags()
    {
        EnableRawMode();
        try
        {
            var flags = ReadSupportKeyBoardEnhancementRaw();
            return flags;
        }
        finally
        {
            DisableRawMode();
        }
    }

    private static termios GetTerminalAttributes(FileDesc fd)
    {
        var termios = new termios();
        if (tcgetattr(fd, ref termios) < 0)
        {
            Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
        }

        return termios;
    }

    private static void SetTerminalAttributes(FileDesc fd, ref termios termios)
    {
        if (tcsetattr(fd, TCSANOW, ref termios) < 0)
        {
            Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
        }
    }

    // Transform the given mode into an raw mode (non-canonical) mode.
    private static void RawTerminalAttribute(ref termios termios)
    {
        cfmakeraw(ref termios);
        Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
    }
}
