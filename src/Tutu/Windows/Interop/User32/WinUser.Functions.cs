using System.Runtime.InteropServices;

namespace Tutu.Windows.Interop.User32;

internal static partial class Windows
{
    public static partial class Interop
    {
        public static partial class User32
        {
            [LibraryImport(LibraryName, SetLastError = true)]
            public static partial nint GetForegroundWindow();

            [LibraryImport(LibraryName, SetLastError = true)]
            public static partial uint GetWindowThreadProcessId(nint hWnd, out uint lpdwProcessId);

            [LibraryImport(LibraryName, SetLastError = true)]
            public static partial nint GetKeyboardLayout(uint idThread);

            [LibraryImport(LibraryName, SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
            public static partial int ToUnicodeEx(
                uint wVirtKey,
                uint wScanCode,
                byte[] lpKeyState,
                [MarshalAs(UnmanagedType.LPWStr)] string pwszBuff,
                int cchBuff,
                uint wFlags,
                nint dwhkl);
        }
    }
}