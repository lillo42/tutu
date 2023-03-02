using System.Diagnostics;
using System.Runtime.InteropServices;
using Tutu.Style.Types;
using Tutu.Windows.Interop.Kernel32;
using static Tutu.Windows.Interop.Kernel32.Kernel32;

namespace Tutu.Windows;

/// <summary>
/// A wrapper around a <see cref="ScreenBuffer"/>.
/// </summary>
internal readonly struct WindowsConsole
{
    public const ushort EnableWrapAtEolOutput = 0x0002;
    public readonly Handle Handle;

    // We know that if we are using console APIs rather than file APIs, then the encoding
    // is Encoding.Unicode implying 2 bytes per character:
    private const int BytesPerWChar = 2;

    public WindowsConsole(Handle handle)
    {
        Handle = handle;
    }

    /// <summary>
    /// Create new instance of <see cref="WindowsConsole"/>.
    /// </summary>
    /// <remarks>
    /// This created instance will use the default input handle (STD_INPUT_HANDLE) as handle for the function call it wraps.
    /// </remarks>
    public static WindowsConsole Input => new(Handle.Create(HandleType.InputHandle));

    /// <summary>
    /// Create new instance of <see cref="WindowsConsole"/>.
    /// </summary>
    /// <remarks>
    /// This created instance will use the default output handle (STD_OUTPUT_HANDLE) as handle for the function call it wraps.
    /// </remarks>
    public static WindowsConsole Output => new(Handle.Create(HandleType.OutputHandle));

    /// <summary>
    /// Create new instance of <see cref="WindowsConsole"/>.
    /// </summary>
    /// <remarks>
    /// This created instance will use the current input handle (CONIN$) as handle for the function call it wraps.
    /// </remarks>
    public static WindowsConsole CurrentIn => new(Handle.Create(HandleType.CurrentInput));

    /// <summary>
    /// Create new instance of <see cref="WindowsConsole"/>.
    /// </summary>
    /// <remarks>
    /// This created instance will use the current input handle (CONOUT$) as handle for the function call it wraps.
    /// </remarks>
    public static WindowsConsole CurrentOutput => new(Handle.Create(HandleType.CurrentOutput));

    /// <summary>
    /// The console mode.
    /// </summary>
    public uint Mode
    {
        get
        {
            if (!GetConsoleMode(Handle, out var mode))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
            }

            return mode;
        }
        set
        {
            if (!SetConsoleMode(Handle, value))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
            }
        }
    }

    /// <summary>
    /// The size of the largest possible console window, based on the current text and the size of the display.
    /// </summary>
    /// <remarks>
    /// This wraps <see href="https://docs.microsoft.com/en-us/windows/console/getlargestconsolewindowsize">GetLargestConsoleWindowSize</see>
    /// </remarks>
    public Coordinate LargestWindowSize
    {
        get
        {
            var coord = GetLargestConsoleWindowSize(Handle);
            Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
            return Coordinate.From(coord);
        }
    }

    /// <summary>
    /// Sets the attributes of characters written to the console screen buffer by the `WriteFile` or `WriteConsole` functions, or echoed by the `ReadFile` or `ReadConsole` functions.
    /// This function affects text written after the function call.
    /// </summary>
    /// <param name="attribute">The attributes is a bit mask of possible <see href="https://docs.microsoft.com/en-us/windows/console/console-screen-buffers#character-attributes">character attributes</see> </param>
    /// <remarks>
    /// This wraps <see href="https://docs.microsoft.com/en-us/windows/console/setconsoletextattribute">SetConsoleTextAttribute</see>
    /// </remarks>
    public void SetTextAttribute(ushort attribute)
    {
        if (!SetConsoleTextAttribute(Handle, attribute))
        {
            Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
        }
    }

    /// <summary>
    /// Sets the current size and position of a console screen buffer's window.
    /// </summary>
    /// <param name="isAbsolute"></param>
    /// <param name="position"></param>
    /// <remarks>
    /// This wraps <see href="https://docs.microsoft.com/en-us/windows/console/setconsolewindowinfo">SetConsoleWindowInfo</see>
    /// </remarks>
    public void SetInfo(bool isAbsolute, WindowPositions position)
    {
        var rect = position.ToSmallRect();
        if (!SetConsoleWindowInfo(Handle, isAbsolute, ref rect))
        {
            Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
        }
    }

    /// <summary>
    /// Writes a character to the console screen buffer a specified number of times, beginning at the specified coordinates.
    /// </summary>
    /// <param name="startLocation"></param>
    /// <param name="cellToWrite"></param>
    /// <param name="fillingChar"></param>
    /// <returns>The number of characters that have been written.</returns>
    /// <remarks>
    /// This wraps <see href="https://docs.microsoft.com/en-us/windows/console/fillconsoleoutputcharacter">FillConsoleOutputCharacterA</see>
    /// </remarks>
    public uint FillWithCharacter(Coordinate startLocation, uint cellToWrite, char fillingChar)
    {
        if (!FillConsoleOutputCharacter(Handle, fillingChar, cellToWrite, startLocation.ToCoord(), out var written))
        {
            Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
        }

        return written;
    }

    /// <summary>
    /// Sets the character attributes for a specified number of character cells, beginning at the specified coordinates in a screen buffer.
    /// </summary>
    /// <param name="startLocation"></param>
    /// <param name="cellToWrite"></param>
    /// <param name="attribute"></param>
    /// <returns>The number of cells that have been modified.</returns>
    /// <remarks>
    /// This wraps <see href="https://docs.microsoft.com/en-us/windows/console/fillconsoleoutputattribute">FillConsoleOutputAttribute</see>
    /// </remarks>
    public uint FillWithAttribute(Coordinate startLocation, uint cellToWrite, ushort attribute)
    {
        if (!FillConsoleOutputAttribute(Handle, attribute, cellToWrite, startLocation.ToCoord(), out var written))
        {
            Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
        }

        return written;
    }

    /// <summary>
    /// Writes a character string to a console screen buffer beginning at the current cursor location.
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns>
    /// This wraps <see href="https://docs.microsoft.com/en-us/windows/console/writeconsole">WriteConsoleW</see>
    /// </returns>
    public int WriteCharBuffer(ReadOnlySpan<byte> buffer)
    {
        var nLength = Convert.ToUInt32(buffer.Length / BytesPerWChar);
        if (!WriteConsoleW(Handle, buffer, nLength, out _, nint.Size))
        {
            Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
        }

        return buffer.Length;
    }

    /// <summary>
    /// Get the number of available input events that can be read without blocking.
    /// </summary>
    /// <remarks>
    /// This wraps <see href="https://docs.microsoft.com/en-us/windows/console/getnumberofconsoleinputevents">GetNumberOfConsoleInputEvents</see>
    /// </remarks>
    public uint NumberOfInputEvent
    {
        get
        {
            if (!GetNumberOfConsoleInputEvents(Handle, out var count))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
            }

            return count;
        }
    }

    /// <summary>
    /// Read a single input event without blocking.
    /// </summary>
    /// <returns>The single read event.</returns>
    /// <remarks>
    /// This wraps <see href="https://docs.microsoft.com/en-us/windows/console/readconsoleinput">ReadConsoleInputW</see>
    /// </remarks>
    public INPUT_RECORD ReadSingleInputEvent()
    {
        var buffer = new INPUT_RECORD[1];
        var events = ReadInputEvents(buffer);
        Debug.Assert(events == 1);
        return buffer[0];
    }

    /// <summary>
    /// Read all available input events without blocking.
    /// </summary>
    /// <returns>All available input events</returns>
    /// <remarks>
    /// This wraps <see href="https://docs.microsoft.com/en-us/windows/console/readconsoleinput">ReadConsoleInputW</see>
    /// </remarks>
    public INPUT_RECORD[] ReadMultipleInputEvents()
    {
        var bufLen = NumberOfInputEvent;
        if (bufLen == 0)
        {
            return Array.Empty<INPUT_RECORD>();
        }

        var buffer = new INPUT_RECORD[bufLen];
        var events = ReadInputEvents(buffer);
        Debug.Assert(events == bufLen);
        return buffer;
    }

    /// <summary>
    /// Read input (via ReadConsoleInputW) into <paramref name="buffer"/> and return the number
    /// of events read. ReadConsoleInputW guarantees that at least one event
    /// is read, even if it means blocking the thread. <see cref="Span{T}.Length"/> must fit in
    /// a <see cref="Int32"/>.
    /// </summary>
    /// <param name="buffer">The destiny input.</param>
    /// <returns>The number of events read.</returns>
    /// <remarks>
    /// This wraps <see href="https://docs.microsoft.com/en-us/windows/console/readconsoleinput">ReadConsoleInputW</see>
    /// </remarks>
    private uint ReadInputEvents(INPUT_RECORD[] buffer)
    {
        if (!ReadConsoleInput(Handle, buffer, (uint)buffer.Length, out var read))
        {
            Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
        }

        return read;
    }

    internal static long _originalConsoleColor = uint.MaxValue;
    internal const ushort ForegroundBlue = 0x0001; // Text color contains blue.
    internal const ushort ForegroundGreen = 0x0002; // Text color contains green.
    internal const ushort ForegroundRed = 0x0004; // Text color contains red.
    internal const ushort ForegroundIntensity = 0x0008; // Text color is intensified.
    internal const ushort BackgroundBlue = 0x0010; // Background color contains blue.
    internal const ushort BackgroundGreen = 0x0020; // Background color contains green.
    internal const ushort BackgroundRed = 0x0040; // Background color contains red.
    internal const ushort BackgroundIntensity = 0x0080; // Background color is intensified.
    internal const ushort CommonLvbLeadingByte = 0x0100; // Leading byte.
    internal const ushort CommonLvbTrailingByte = 0x0200; // Trailing byte.
    internal const ushort CommonLvbGridHorizontal = 0x0400; // Top horizontal.
    internal const ushort CommonLvbGridLvertical = 0x0800; // Left vertical.
    internal const ushort CommonLvbGridRvertical = 0x1000; // Right vertical.
    internal const ushort CommonLvbReverseVideo = 0x4000; // Reverse foreground and background attribute.
    internal const ushort CommonLvbUnderscore = 0x8000; // Underscore

    public void SetForegroundColor(Color color)
    {
        InitConsoleColor();

        var buffer = new ScreenBuffer(Handle);
        var info = buffer.Info;

        var attrs = info.Attributes;
        var bgColor = attrs | 0x0070;

        var value = (ushort)(bgColor | FromForegroundColor(color));

        if ((attrs & BackgroundIntensity) != 0)
        {
            value |= BackgroundIntensity;
        }

        SetTextAttribute(value);
    }

    public void SetBackgroundColor(Color color)
    {
        InitConsoleColor();

        var buffer = new ScreenBuffer(Handle);
        var info = buffer.Info;

        var attrs = info.Attributes;
        var fgColor = attrs | 0x000F;
        var value = (ushort)(fgColor | FromBackgroundColor(color));
        if ((value & ForegroundIntensity) != 0)
        {
            value |= BackgroundIntensity;
        }

        SetTextAttribute(value);
    }

    private static ushort From(Colored colored)
    {
        if (colored.IsForegroundColor)
        {
            return FromForegroundColor(colored.Color);
        }

        if (colored.IsBackgroundColor)
        {
            return FromBackgroundColor(colored.Color);
        }

        return 0;
    }

    internal static ushort FromForegroundColor(Color color)
    {
        if (color == Color.Black)
        {
            return 0;
        }

        if (color == Color.DarkGrey)
        {
            return ForegroundIntensity;
        }

        if (color == Color.Red)
        {
            return ForegroundIntensity | ForegroundRed;
        }

        if (color == Color.DarkRed)
        {
            return ForegroundRed;
        }

        if (color == Color.Green)
        {
            return ForegroundIntensity | ForegroundGreen;
        }

        if (color == Color.DarkGreen)
        {
            return ForegroundGreen;
        }

        if (color == Color.Yellow)
        {
            return ForegroundIntensity | ForegroundRed | ForegroundGreen;
        }

        if (color == Color.DarkYellow)
        {
            return ForegroundRed | ForegroundGreen;
        }

        if (color == Color.Blue)
        {
            return ForegroundIntensity | ForegroundBlue;
        }

        if (color == Color.DarkBlue)
        {
            return ForegroundBlue;
        }

        if (color == Color.Magenta)
        {
            return ForegroundIntensity | ForegroundRed | ForegroundBlue;
        }

        if (color == Color.DarkMagenta)
        {
            return ForegroundRed | ForegroundBlue;
        }

        if (color == Color.Cyan)
        {
            return ForegroundIntensity | ForegroundGreen | ForegroundBlue;
        }

        if (color == Color.DarkCyan)
        {
            return ForegroundGreen | ForegroundBlue;
        }

        if (color == Color.White)
        {
            return ForegroundIntensity | ForegroundRed | ForegroundGreen | ForegroundBlue;
        }

        if (color == Color.Grey)
        {
            return ForegroundRed | ForegroundGreen | ForegroundBlue;
        }

        if (color == Color.Reset)
        {
            var originalColor = (ushort)Interlocked.Read(ref _originalConsoleColor);
            const ushort removeBackgroundMask =
                BackgroundIntensity | BackgroundRed | BackgroundGreen | BackgroundBlue;

            return (ushort)(originalColor | ~removeBackgroundMask);
        }

        return 0;
    }

    internal static ushort FromBackgroundColor(Color color)
    {
        if (color == Color.Black)
        {
            return 0;
        }

        if (color == Color.DarkGrey)
        {
            return BackgroundIntensity;
        }

        if (color == Color.Red)
        {
            return BackgroundIntensity | BackgroundRed;
        }

        if (color == Color.DarkRed)
        {
            return BackgroundRed;
        }

        if (color == Color.Green)
        {
            return BackgroundIntensity | BackgroundGreen;
        }

        if (color == Color.DarkGreen)
        {
            return BackgroundGreen;
        }

        if (color == Color.Yellow)
        {
            return BackgroundIntensity | BackgroundRed | BackgroundGreen;
        }

        if (color == Color.DarkYellow)
        {
            return BackgroundRed | BackgroundGreen;
        }

        if (color == Color.Blue)
        {
            return BackgroundIntensity | BackgroundBlue;
        }

        if (color == Color.DarkBlue)
        {
            return BackgroundBlue;
        }

        if (color == Color.Magenta)
        {
            return BackgroundIntensity | BackgroundRed | BackgroundBlue;
        }

        if (color == Color.DarkMagenta)
        {
            return BackgroundRed | BackgroundBlue;
        }

        if (color == Color.Cyan)
        {
            return BackgroundIntensity | BackgroundGreen | BackgroundBlue;
        }

        if (color == Color.DarkCyan)
        {
            return BackgroundGreen | BackgroundBlue;
        }

        if (color == Color.White)
        {
            return BackgroundIntensity | BackgroundRed | BackgroundGreen | BackgroundBlue;
        }

        if (color == Color.Grey)
        {
            return BackgroundRed | BackgroundGreen | BackgroundBlue;
        }

        if (color == Color.Reset)
        {
            var originalColor = Interlocked.Read(ref _originalConsoleColor);
            const ushort removeForegroundMask =
                ForegroundIntensity | ForegroundRed | ForegroundGreen | ForegroundBlue;

            return (ushort)((ushort)originalColor | ~removeForegroundMask);
        }

        return 0;
    }

    private static void InitConsoleColor()
    {
        var originalColor = Interlocked.Read(ref _originalConsoleColor);
        if (originalColor == uint.MaxValue)
        {
            var screenBufferInfo = ScreenBuffer.CurrentOutput;
            var attributes = screenBufferInfo.Info.Attributes;
            Interlocked.Exchange(ref _originalConsoleColor, attributes);
        }
    }

    public void Reset()
    {
        var originalColor = Interlocked.Read(ref _originalConsoleColor);
        if (originalColor <= ushort.MaxValue)
        {
            SetTextAttribute((ushort)originalColor);
        }
    }

    internal const uint EnableMouseMode = 0x0010 | 0x0080 | 0x0008;
    private static ulong _originalConsoleMode = ulong.MaxValue;

    public void EnableMouseCapture()
    {
        Interlocked.CompareExchange(ref _originalConsoleMode, Mode, ulong.MaxValue);
        Mode = EnableMouseMode;
    }

    public void DisableMouseCapture()
    {
        var originalMode = Interlocked.Read(ref _originalConsoleMode);
        Mode = (uint)originalMode;
    }
}