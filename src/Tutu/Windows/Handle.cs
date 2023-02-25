using System.Runtime.InteropServices;
using static Tutu.Windows.Interop.Windows.Interop;
using static Tutu.Windows.Interop.Kernel32.Windows.Interop.Kernel32;

namespace Tutu.Windows;

internal static partial class Windows
{
    /// <summary>
    /// This abstracts away some WinAPI calls to set and get some console handles.
    ///
    /// It wraps WinAPI's [`HANDLE`] type.
    /// </summary>
    /// <param name="Inner"></param>
    public readonly record struct Handle(Inner Inner)
    {
        public nint HandleValue => Inner.Handler;
        
        /// <summary>
        /// Construct a handle from a raw handle.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static Handle Create(nint handle) => new(new Inner(handle, true));

        /// <summary>
        /// Create a new handle of a certaint type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static Handle Create(HandleType type) => type switch
        {
            HandleType.OutputHandle => OutputHandler(),
            HandleType.InputHandle => InputHandle(),
            HandleType.CurrentOutputHandle => CurrentOutHandle(),
            HandleType.CurrentInputHandle => CurrentInHandle(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        /// <summary>
        /// Get the handle of the standard output.
        ///
        /// On success this function returns the `HANDLE` to `STD_OUTPUT_HANDLE`.
        ///
        /// This wraps [`GetStdHandle`](https://docs.microsoft.com/en-us/windows/console/getstdhandle)
        /// called with `STD_OUTPUT_HANDLE`.
        /// </summary>
        /// <returns></returns>
        public static Handle OutputHandler()
            => new(new Inner(GetStdHandle(STD_OUTPUT_HANDLE), false));

        /// <summary>
        /// Get the handle of the input screen buffer.
        ///
        /// On success this function returns the `HANDLE` to `STD_INPUT_HANDLE`.
        ///
        /// This wraps [`GetStdHandle`](https://docs.microsoft.com/en-us/windows/console/getstdhandle)
        /// called with `STD_INPUT_HANDLE`.
        /// </summary>
        /// <returns></returns>
        public static Handle InputHandle()
            => new(new Inner(GetStdHandle(STD_INPUT_HANDLE), false));

        /// <summary>
        /// Get the handle of the active screen buffer.
        /// When using multiple screen buffers this will always point to the to the current screen output buffer.
        ///
        /// This function uses `CONOUT$` to create a file handle to the current output buffer.
        ///
        /// This wraps
        /// [`CreateFileW`](https://docs.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-createfilew).
        /// </summary>
        /// <returns></returns>
        public static Handle CurrentOutHandle()
        {
            var handle = CreateFileW("CONOUT$",
                GENERIC_READ | GENERIC_WRITE,
                FILE_SHARE_READ | FILE_SHARE_WRITE,
                nint.Zero,
                OPEN_EXISTING,
                0,
                nint.Zero);

            var a = Marshal.GetLastPInvokeError();
            var tmp = Marshal.GetLastPInvokeErrorMessage();
            return new(new Inner(handle, true));
        }


        /// <summary>
        /// Get the handle of the console input buffer.
        ///
        /// This function uses `CONIN$` to create a file handle to the current input buffer.
        ///
        /// This wraps
        /// [`CreateFileW`](https://docs.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-createfilew).
        /// </summary>
        /// <returns></returns>
        public static Handle CurrentInHandle()
        {
            var handle = CreateFileW("CONIN$",
                GENERIC_READ | GENERIC_WRITE,
                FILE_SHARE_READ | FILE_SHARE_WRITE,
                nint.Zero,
                OPEN_EXISTING,
                0,
                nint.Zero);

            return new(new Inner(handle, true));
        }

        public static implicit operator nint(Handle handle)
            => handle.Inner.Handler;
    }
}