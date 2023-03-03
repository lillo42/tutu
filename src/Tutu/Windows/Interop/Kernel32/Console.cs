using System.Runtime.InteropServices;

namespace Tutu.Windows.Interop.Kernel32;

internal static partial class Kernel32
{
    [LibraryImport(LibraryName, SetLastError = true)]
    public static partial nint CreateConsoleScreenBuffer(uint dwDesiredAccess, uint dwShareMode,
        SECURITY_ATTRIBUTES lpSecurityAttributes, uint dwFlags, nint lpScreenBufferData);

    [LibraryImport(LibraryName, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool FillConsoleOutputAttribute(nint hConsoleOutput, ushort wAttribute, uint nLength,
        COORD dwWriteCoord, out uint lpNumberOfAttrsWritten);

    [LibraryImport(LibraryName, SetLastError = true, EntryPoint = "FillConsoleOutputCharacterA",
        StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool FillConsoleOutputCharacter(nint hConsoleOutput, char cCharacter, uint nLength,
        COORD dwWriteCoord, out uint lpNumberOfCharsWritten);

    [LibraryImport(LibraryName, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool GetConsoleMode(nint hConsoleOutput, out uint dwMode);

    [LibraryImport(LibraryName, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool GetConsoleScreenBufferInfo(nint hConsoleOutput,
        out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);

    [LibraryImport(LibraryName, SetLastError = true, EntryPoint = "SetConsoleTitleW",
        StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SetConsoleTitle(string title);

    [LibraryImport(LibraryName, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool GetNumberOfConsoleInputEvents(nint hConsoleOutput, out uint lpNumberOfEvents);

    [LibraryImport(LibraryName, SetLastError = true)]
    public static partial COORD GetLargestConsoleWindowSize(nint hConsoleOutput);

    [LibraryImport(LibraryName, EntryPoint = "ReadConsoleInputW", SetLastError = true,
        StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool ReadConsoleInput(nint hConsoleInput,
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]
        INPUT_RECORD[] buffer, uint nLength,
        out uint lpNumberOfEventsRead);

    [LibraryImport(LibraryName, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SetConsoleActiveScreenBuffer(nint hConsoleOutput);

    [LibraryImport(LibraryName, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SetConsoleCursorInfo(nint hConsoleOutput,
        ref CONSOLE_CURSOR_INFO lpConsoleCursorInfo);

    [LibraryImport(LibraryName, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SetConsoleCursorPosition(nint hConsoleOutput, COORD dwCursorPosition);

    [LibraryImport(LibraryName, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SetConsoleTextAttribute(nint hConsoleOutput, ushort wAttributes);

    [LibraryImport(LibraryName, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SetConsoleMode(nint hConsoleOutput, uint dwMode);

    [LibraryImport(LibraryName, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SetConsoleScreenBufferSize(nint hConsoleOutput, COORD dwSize);

    [LibraryImport(LibraryName, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static unsafe partial bool SetConsoleWindowInfo(nint hConsoleOutput,
        [MarshalAs(UnmanagedType.Bool)] bool absolute, ref SMALL_RECT consoleWindow);

    [LibraryImport(LibraryName, SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static unsafe partial bool WriteConsoleW(
        nint hConsoleOutput,
        ReadOnlySpan<byte> lpBuffer,
        uint nNumberOfCharsToWrite,
        out uint lpNumberOfCharsWritten,
        nint lpReserved);
}

[StructLayout(LayoutKind.Sequential)]
internal struct CONSOLE_SCREEN_BUFFER_INFO
{
    public COORD dwSize;
    public COORD dwCursorPosition;
    public ushort wAttributes;
    public SMALL_RECT srWindow;
    public COORD dwMaximumWindowSize;
}

[StructLayout(LayoutKind.Sequential)]
internal struct COORD
{
    public short X;
    public short Y;
}

[StructLayout(LayoutKind.Sequential)]
internal struct SMALL_RECT
{
    public short Left;
    public short Top;
    public short Right;
    public short Bottom;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct INPUT_RECORD
{
    public ushort EventType;
    public INPUT_EVENT_RECORD Event;
}

[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
internal struct INPUT_EVENT_RECORD
{
    #region KEY_EVENT_RECORD

    [FieldOffset(0)]
    [MarshalAs(UnmanagedType.Bool)]
    public bool bKeyDown;

    [FieldOffset(4)] public ushort wRepeatCount;

    [FieldOffset(6)] public ushort wVirtualKeyCode;

    [FieldOffset(8)] public ushort wVirtualScanCode;

    [FieldOffset(10)] public ushort UChar; // Union between WCHAR and ASCII char

    [FieldOffset(12)] public uint dwControlKeyState;

    #endregion

    #region MOUSE_EVENT_RECORD

    [FieldOffset(0)] public COORD dwMousePosition;

    [FieldOffset(4)] public int dwButtonState;

    [FieldOffset(8)] public uint dwControlKeyStateMouse;

    [FieldOffset(12)] public uint dwEventFlags;

    #endregion

    #region FOCUS_EVENT_RECORD

    [FieldOffset(0)]
    [MarshalAs(UnmanagedType.Bool)]
    public bool bSetFocus;

    #endregion

    #region MENU_EVENT_RECORD

    [FieldOffset(0)] public uint dwCommandId;

    #endregion

    #region WINDOW_BUFFER_SIZE_RECORD

    [FieldOffset(0)] public COORD dwSize;

    #endregion
}

[StructLayout(LayoutKind.Sequential)]
internal struct CONSOLE_CURSOR_INFO
{
    public uint dwSize;
    public int bVisible;
}

internal enum EventType : ushort
{
    FocusEvent = 0x0010,
    KeyEvent = 0x0001,
    MenuEvent = 0x0008,
    MouseEvent = 0x0002,
    WindowBufferSizeEvent = 0x0004
}
