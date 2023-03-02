using System.Runtime.InteropServices;
using Tutu.Windows.Interop;
using Tutu.Windows.Interop.Kernel32;
using static Tutu.Windows.Interop.Consts;
using static Tutu.Windows.Interop.Kernel32.Kernel32;

namespace Tutu.Windows;

/// <summary>
/// The screen buffer.
/// </summary>
internal readonly struct ScreenBuffer
{
    private const uint ConsoleTextModeBuffer = 1;
    public readonly Handle Handle;

    public ScreenBuffer(Handle handle)
    {
        Handle = handle;
    }

    /// <summary>
    /// Create new console screen buffer.
    /// </summary>
    /// <returns>New instance of <see cref="ScreenBuffer" />.</returns>
    /// <remarks>
    /// This wraps <see href="https://docs.microsoft.com/en-us/windows/console/createconsolescreenbuffer">CreateConsoleScreenBuffer</see>
    /// </remarks>
    public static unsafe ScreenBuffer Create()
    {
        var security = new SECURITY_ATTRIBUTES
        {
            nLength = sizeof(SECURITY_ATTRIBUTES),
            lpSecurityDescriptor = nint.Zero,
            bInheritHandle = true
        };

        var newScreenBuffer = CreateConsoleScreenBuffer(
            GENERIC_READ | GENERIC_WRITE,
            FILE_SHARE_READ | FILE_SHARE_WRITE,
            security,
            ConsoleTextModeBuffer,
            nint.Zero
        );

        Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
        return new(Handle.Create(newScreenBuffer));
    }

    /// <summary>
    /// The console input screen buffer.
    /// </summary>
    public static ScreenBuffer Input => new(Handle.Create(HandleType.InputHandle));

    /// <summary>
    /// The console output screen buffer.
    /// </summary>
    public static ScreenBuffer Output => new(Handle.Create(HandleType.OutputHandle));

    /// <summary>
    /// The current console input screen buffer.
    /// </summary>
    public static ScreenBuffer CurrentInput => new(Handle.Create(HandleType.CurrentInput));

    /// <summary>
    /// The current console output screen buffer.
    /// </summary>
    public static ScreenBuffer CurrentOutput => new(Handle.Create(HandleType.CurrentOutput));

    /// <summary>
    ///  Get the screen buffer information like terminal size, cursor position, buffer size.
    /// </summary>
    /// <remarks>
    /// This wraps <see href="https://docs.microsoft.com/en-us/windows/console/getconsolescreenbufferinfo">GetConsoleScreenBufferInfo</see>
    /// </remarks>
    public ScreenBufferInfo Info
    {
        get
        {
            if (!GetConsoleScreenBufferInfo(Handle, out var buffer))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
            }

            return new(buffer);
        }
    }

    /// <summary>
    /// Set this screen buffer to the current one.
    /// </summary>
    /// <remarks>
    /// This wraps <see href="https://docs.microsoft.com/en-us/windows/console/setconsoleactivescreenbuffer">SetConsoleActiveScreenBuffer</see> 
    /// </remarks>
    public void Show()
    {
        if (!SetConsoleActiveScreenBuffer(Handle))
        {
            Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
        }
    }

    /// <summary>
    /// Set the console screen buffer size to the given size.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <remarks>
    /// This wraps <see href="https://docs.microsoft.com/en-us/windows/console/setconsolescreenbuffersize">SetConsoleScreenBufferSize</see>
    /// </remarks>
    public void SetSize(short x, short y)
    {
        if (!SetConsoleScreenBufferSize(Handle, new COORD { X = x, Y = y }))
        {
            Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
        }
    }
}

/// <summary>
/// Information about a console screen buffer.
/// </summary>
/// <remarks>
/// This wraps <see href="https://docs.microsoft.com/en-us/windows/console/console-screen-buffer-info-str">CONSOLE_SCREEN_BUFFER_INFO</see>
/// </remarks>
internal readonly struct ScreenBufferInfo
{
    public readonly CONSOLE_SCREEN_BUFFER_INFO Info;

    public ScreenBufferInfo(CONSOLE_SCREEN_BUFFER_INFO info)
    {
        Info = info;
    }

    /// <summary>
    ///  Get the size of the screen buffer.
    /// </summary>
    /// <remarks>
    /// Will take <see cref="CONSOLE_SCREEN_BUFFER_INFO.dwSize"/> from the current screen buffer and convert it into a <see cref="Size"/>.
    /// </remarks>
    public Size BufferSize => Size.From(Info.dwSize);

    /// <summary>
    /// Get the size of the terminal display window.
    /// </summary>
    /// <remarks>
    /// Will calculate the width and height from <see cref="CONSOLE_SCREEN_BUFFER_INFO.srWindow"/> and convert it into a <see cref="Size"/>.
    /// </remarks>
    public Size TerminalSize => new(
        (short)(Info.srWindow.Bottom - Info.srWindow.Top),
        (short)(Info.srWindow.Right - Info.srWindow.Left)
        );

    /// <summary>
    /// Get the position and size of the terminal display window.
    /// </summary>
    /// <remarks>
    /// Will take <see cref="CONSOLE_SCREEN_BUFFER_INFO.srWindow"/> and convert it into the <see cref="WindowPositions"/> type.
    /// </remarks>
    public WindowPositions TerminalWindow => WindowPositions.From(Info);

    /// <summary>
    /// Get the position and size of the terminal display window.
    /// </summary>
    /// <remarks>
    /// Will take <see cref="CONSOLE_SCREEN_BUFFER_INFO.wAttributes" /> from the current screen buffer.
    /// </remarks>
    public ushort Attributes => Info.wAttributes;

    /// <summary>
    /// Get the current column and row of the terminal cursor in the screen buffer.
    /// </summary>
    /// <remarks>
    /// Will take <see cref="CONSOLE_SCREEN_BUFFER_INFO.dwCursorPosition"/> from the current screen buffer.
    /// </remarks>
    public Coordinate CursorPosition => Coordinate.From(Info.dwCursorPosition);
}