using System.Diagnostics;
using System.Runtime.InteropServices;
using static Tutu.Windows.Interop.Kernel32.Windows.Interop.Kernel32;

namespace Tutu.Windows;

internal static partial class Windows
{
    /// <summary>
    /// A wrapper around a screen buffer.
    /// </summary>
    /// <param name="Handle"></param>
    public readonly record struct Console(Handle Handle)
    {
        public const ushort EnableWrapAtEolOutput = 0x0002;

        /// <summary>
        /// Create new instance of `Console`.
        ///
        /// This created instance will use the default output handle (STD_OUTPUT_HANDLE) as handle for the function call it wraps.
        /// </summary>
        /// <returns></returns>
        public static Console Output => new(Handle.Create(HandleType.OutputHandle));

        public static Console Input => new(Handle.Create(HandleType.InputHandle));

        public static Console CurrentIn => new(Handle.CurrentInHandle());
        
        public static Console CurrentOut => new(Handle.CurrentOutHandle());

        public uint Mode
        {
            get
            {
                if (GetConsoleMode(Handle, out var mode))
                {
                    return mode;
                }

                throw GetExceptionForWin32Error(Marshal.GetLastWin32Error());
            }
            set
            {
                if (!SetConsoleMode(Handle, value))
                {
                    throw GetExceptionForWin32Error(Marshal.GetLastWin32Error());
                }
            }
        }

        /// <summary>
        /// Sets the attributes of characters written to the console screen buffer by the `WriteFile` or `WriteConsole` functions, or echoed by the `ReadFile` or `ReadConsole` functions.
        /// This function affects text written after the function call.
        ///
        /// The attributes is a bitmask of possible [character
        /// attributes](https://docs.microsoft.com/en-us/windows/console/console-screen-buffers#character-attributes).
        ///
        /// This wraps
        /// [`SetConsoleTextAttribute`](https://docs.microsoft.com/en-us/windows/console/setconsoletextattribute).
        /// </summary>
        public void SetTextAttribute(ushort value)
        {
            if (!SetConsoleTextAttribute(Handle, value))
            {
                throw GetExceptionForWin32Error(Marshal.GetLastPInvokeError());
            }
        }

        /// <summary>
        /// Sets the current size and position of a console screen buffer's window.
        ///
        /// This wraps
        /// [`SetConsoleWindowInfo`](https://docs.microsoft.com/en-us/windows/console/setconsolewindowinfo).
        /// </summary>
        /// <param name="absolute"></param>
        /// <param name="positions"></param>
        public void SetConsoleInfo(bool absolute, WindowPositions positions)
        {
            var rect = positions.ToSmallRect();
            if (!SetConsoleWindowInfo(Handle, absolute, ref rect))
            {
                throw GetExceptionForWin32Error(Marshal.GetLastPInvokeError());
            }
        }

        /// <summary>
        /// Writes a character to the console screen buffer a specified number of times, beginning at the specified coordinates.
        /// Returns the number of characters that have been written.
        ///
        /// This wraps
        /// [`FillConsoleOutputCharacterA`](https://docs.microsoft.com/en-us/windows/console/fillconsoleoutputcharacter).
        /// </summary>
        public uint FillWhitCharacter(Coordinate startLocation, uint cellToWrite, char fillingChar)
        {
            if (FillConsoleOutputCharacter(Handle, fillingChar, cellToWrite, startLocation.ToCoord(),
                    out var chars))
            {
                return chars;
            }

            throw GetExceptionForWin32Error(Marshal.GetLastPInvokeError());
        }

        /// <summary>
        /// Sets the character attributes for a specified number of character cells, beginning at the specified coordinates in a screen buffer.
        /// Returns the number of cells that have been modified.
        ///
        /// This wraps
        /// [`FillConsoleOutputAttribute`](https://docs.microsoft.com/en-us/windows/console/fillconsoleoutputattribute).
        /// </summary>
        /// <returns></returns>
        public uint FillWitAttribute(Coordinate startLocation, uint cellToWrite, ushort attribute)
        {
            if (FillConsoleOutputAttribute(Handle, attribute, cellToWrite, startLocation.ToCoord(),
                    out var cellsWritten))
            {
                return cellsWritten;
            }

            throw GetExceptionForWin32Error(Marshal.GetLastPInvokeError());
        }

        /// <summary>
        /// Retrieves the size of the largest possible console window, based on the current text and the size of the display.
        ///
        /// This wraps [`GetLargestConsoleWindowSize`](https://docs.microsoft.com/en-us/windows/console/getlargestconsolewindowsize)
        /// </summary>
        /// <returns></returns>
        public Coordinate LargestWindowsSize()
        {
            var coord = GetLargestConsoleWindowSize(Handle);
            var error = Marshal.GetLastPInvokeError();
            if (error != 0)
            {
                throw GetExceptionForWin32Error(error);
            }

            return Coordinate.From(coord);
        }

        // We know that if we are using console APIs rather than file APIs, then the encoding
        // is Encoding.Unicode implying 2 bytes per character:
        private const int BytesPerWChar = 2;

        /// <summary>
        /// Writes a character string to a console screen buffer beginning at the current cursor location.
        ///
        /// This wraps
        /// [`WriteConsoleW`](https://docs.microsoft.com/en-us/windows/console/writeconsole).
        /// </summary>
        /// <returns></returns>
        public unsafe int WriteCharBuffer(ReadOnlySpan<byte> bytes)
        {
            fixed (byte* p = bytes)
            {
                var nLength = Convert.ToUInt32(bytes.Length / BytesPerWChar);
                if (!WriteConsoleW(Handle, p, nLength, out _, nint.Size))
                {
                    throw GetExceptionForWin32Error(Marshal.GetLastPInvokeError());
                }
            }

            return bytes.Length;
        }

        public INPUT_RECORD ReadSingleInputEvent()
        {
            var numRead = ReadInput(out var buf, 1);
            Debug.Assert(numRead == 1);

            var error = Marshal.GetLastPInvokeError();
            if (error != 0)
            {
                throw GetExceptionForWin32Error(error);
            }

            return buf[0];
        }

        /// <summary>
        /// Read all available input events without blocking.
        ///
        /// This wraps
        /// [`ReadConsoleInputW`](https://docs.microsoft.com/en-us/windows/console/readconsoleinput).
        /// </summary>
        /// <returns></returns>
        public INPUT_RECORD[] ReadMultipleInput()
        {
            var bufLen = NumberOfConsoleInputEvent();
            if (bufLen == 0)
            {
                return Array.Empty<INPUT_RECORD>();
            }

            ReadInput(out var buf, bufLen);
            return buf;
        }

        /// <summary>
        /// Get the number of available input events that can be read without blocking.
        ///
        /// This wraps
        /// [`GetNumberOfConsoleInputEvents`](https://docs.microsoft.com/en-us/windows/console/getnumberofconsoleinputevents).
        /// </summary>
        /// <returns></returns>
        public uint NumberOfConsoleInputEvent()
        {
            if (GetNumberOfConsoleInputEvents(Handle, out var events))
            {
                return events;
            }

            throw GetExceptionForWin32Error(Marshal.GetLastPInvokeError());
        }

        /// <summary>
        /// Read input (via ReadConsoleInputW) into buf and return the number
        /// of events read. ReadConsoleInputW guarantees that at least one event
        /// is read, even if it means blocking the thread. buf.len() must fit in
        /// a u32.
        /// </summary>
        /// <returns></returns>
        private uint ReadInput(out INPUT_RECORD[] buffer, uint length)
        {
            var tmp = new INPUT_RECORD[length];
            if (!ReadConsoleInput(Handle, tmp, length, out var numRecords))
            {
                throw GetExceptionForWin32Error(Marshal.GetLastPInvokeError());
            }

            buffer = tmp;
            return numRecords;
        }
    }
}