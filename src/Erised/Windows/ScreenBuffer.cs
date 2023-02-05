using System.Runtime.InteropServices;
using static Erised.Windows.Interop;
using static Erised.Windows.Interop.Kernel32;

namespace Erised;

internal static partial class Windows
{
    public readonly record struct ScreenBuffer(Handle Handle)
    {
        private const uint ConsoleTextModeBuffer = 1;

        /// Create new console screen buffer.
        ///
        /// This wraps
        /// [`CreateConsoleScreenBuffer`](https://docs.microsoft.com/en-us/windows/console/createconsolescreenbuffer)
        public static unsafe ScreenBuffer Create()
        {
            var security = new SECURITY_ATTRIBUTES
            {
#pragma warning disable CS8500
                nLength = (uint)sizeof(SECURITY_ATTRIBUTES),
#pragma warning restore CS8500
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

            return new(Handle.Create(newScreenBuffer));
        }

        /// <summary>
        /// Get the current console screen buffer
        /// </summary>
        /// <returns></returns>
        public static ScreenBuffer Current => new(Handle.Create(HandleType.CurrentOutputHandle));

        /// <summary>
        /// Set this screen buffer to the current one.
        ///
        /// This wraps
        /// [`SetConsoleActiveScreenBuffer`](https://docs.microsoft.com/en-us/windows/console/setconsoleactivescreenbuffer).
        /// </summary>
        public void Show()
        {
            if (!SetConsoleActiveScreenBuffer(Handle))
            {
                throw GetExceptionForWin32Error(Marshal.GetLastPInvokeError());
            }
        }


        /// <summary>
        ///  Get the screen buffer information like terminal size, cursor position, buffer size.
        ///
        /// This wraps
        /// [`GetConsoleScreenBufferInfo`](https://docs.microsoft.com/en-us/windows/console/getconsolescreenbufferinfo).
        /// </summary>
        /// <returns></returns>
        public ScreenBufferInfo Info
        {
            get
            {
                if (!GetConsoleScreenBufferInfo(Handle, out var buffer))
                {
                    throw GetExceptionForWin32Error(Marshal.GetLastPInvokeError());
                }

                return new(buffer);
            }
        }

        /// <summary>
        /// Set the console screen buffer size to the given size.
        ///
        /// This wraps
        /// [`SetConsoleScreenBufferSize`](https://docs.microsoft.com/en-us/windows/console/setconsolescreenbuffersize).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetSize(short x, short y)
        {
            if (!SetConsoleScreenBufferSize(Handle, new COORD { X = x, Y = y }))
            {
                throw GetExceptionForWin32Error(Marshal.GetLastPInvokeError());
            }
        }
    }
}