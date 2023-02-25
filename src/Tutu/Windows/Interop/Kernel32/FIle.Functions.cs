using System.Runtime.InteropServices;

namespace Tutu.Windows.Interop.Kernel32;

internal static partial class Windows
{
    public static partial class Interop
    {
        public static partial class Kernel32
        {
            public const uint CREATE_NEW = 1;
            public const uint CREATE_ALWAYS = 2;
            public const uint OPEN_EXISTING = 3;
            public const uint OPEN_ALWAYS = 4;
            public const uint TRUNCATE_EXISTING = 5;
            
            [LibraryImport(LibraryName, SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
            public static partial nint CreateFileW(string lpFileName,
                uint dwDesiredAccess,
                uint dwShareMode,
                nint lpSecurityAttributes,
                uint dwCreationDisposition,
                uint dwFlagsAndAttributes,
                nint hTemplateFile);
        }
    }
}