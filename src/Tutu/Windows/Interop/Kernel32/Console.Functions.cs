using System.Runtime.InteropServices;

namespace Tutu.Windows.Interop.Kernel32;

internal static partial class Windows
{
    public static partial class Interop
    {
        public static partial class Kernel32
        {
            [LibraryImport(LibraryName, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool SetConsoleActiveScreenBuffer(nint hConsoleOutput);

            [LibraryImport(LibraryName, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool GetConsoleScreenBufferInfo(nint hConsoleOutput,
                out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);

            [LibraryImport(LibraryName, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool SetConsoleScreenBufferSize(nint hConsoleOutput, COORD dwSize);

            [LibraryImport(LibraryName, SetLastError = true)]
            public static partial nint CreateConsoleScreenBuffer(uint dwDesiredAccess, uint dwShareMode,
                Tutu.Windows.Interop.Windows.Interop.SECURITY_ATTRIBUTES lpSecurityAttributes, uint dwFlags, nint lpScreenBufferData);

            [LibraryImport(LibraryName, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool SetConsoleMode(nint hConsoleOutput, uint dwMode);

            [LibraryImport(LibraryName, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool GetConsoleMode(nint hConsoleOutput, out uint dwMode);

            [LibraryImport(LibraryName, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool SetConsoleTextAttribute(nint hConsoleOutput, ushort wAttributes);

            [LibraryImport(LibraryName, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static unsafe partial bool SetConsoleWindowInfo(nint hConsoleOutput,
                [MarshalAs(UnmanagedType.Bool)] bool absolute, ref SMALL_RECT consoleWindow);

            [LibraryImport(LibraryName, SetLastError = true, EntryPoint = "FillConsoleOutputCharacterA", StringMarshalling = StringMarshalling.Utf16)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool FillConsoleOutputCharacter(nint hConsoleOutput, char cCharacter, uint nLength,
                COORD dwWriteCoord, out uint lpNumberOfCharsWritten);

            [LibraryImport(LibraryName, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool FillConsoleOutputAttribute(nint hConsoleOutput, ushort wAttribute, uint nLength,
                COORD dwWriteCoord, out uint lpNumberOfAttrsWritten);

            [LibraryImport(LibraryName, SetLastError = true)]
            public static partial COORD GetLargestConsoleWindowSize(nint hConsoleOutput);

            [LibraryImport(LibraryName, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool GetNumberOfConsoleInputEvents(nint hConsoleOutput, out uint lpNumberOfEvents);

            [LibraryImport(LibraryName, SetLastError = true, EntryPoint = "SetConsoleTitleA", StringMarshalling = StringMarshalling.Utf16)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool SetConsoleTitle(string title);

            [LibraryImport(LibraryName, EntryPoint = "ReadConsoleInputW", SetLastError = true,
                StringMarshalling = StringMarshalling.Utf16)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool ReadConsoleInput(nint hConsoleInput,
                [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]
                INPUT_RECORD[] buffer, uint nLength,
                out uint lpNumberOfEventsRead);

            [LibraryImport(LibraryName, SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static unsafe partial bool WriteConsoleW(
                nint hConsoleOutput,
                byte* lpBuffer,
                uint nNumberOfCharsToWrite,
                out uint lpNumberOfCharsWritten,
                nint lpReserved);

            [LibraryImport(LibraryName, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool SetConsoleCursorInfo(nint hConsoleOutput,
                ref CONSOLE_CURSOR_INFO lpConsoleCursorInfo);

            [LibraryImport(LibraryName, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool SetConsoleCursorPosition(nint hConsoleOutput, COORD dwCursorPosition);
        }
    }
}